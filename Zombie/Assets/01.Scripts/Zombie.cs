using System.Collections;
using UnityEngine;
using UnityEngine.AI; //AI, 내비게이션 시스템 관련 코드 가져오기

public class Zombie: LivingEntity //좀비 AI 구현
{
    public LayerMask whatIsTarget; //추적 대상 레이어

    private LivingEntity targetEntity; //추적대상
    private NavMeshAgent navMeshAgent; //경로 계산 AI 에이전트

    public ParticleSystem hitEffect; //피격 시 재생할 파티클 효과
    public AudioClip deathSound; //사망 시 재생할 소리
    public AudioClip hitSound; //피격 시 재생할 소리

    private Animator zombieAnimator; //애니메이터 컴포넌트
    private AudioSource zombieAudioPlayer; //오디오 소스 컴포넌트
    private Renderer zombieRenderer; //렌더러 컴포넌트

    public float damage = 20f; //공격력
    public float timeBetAttack = 0.5f; //공격 간격
    private float lastAttackTime; //마지막 공격 시점
 
    private bool hasTarget //추적할 대상이 존재하는지 알려주는 프로퍼티(읽기 전용 get 있음, set 없음)
    {
       get
        {
            if (targetEntity != null && !targetEntity.dead) 
                //추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            {
                return true;
            }

            return false; //그렇지 않다면 false
        }
    }

    private void Awake() //초기화
    {
        //게임 오브젝트로부터 사용할 컴포넌트 가져오기
        navMeshAgent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieAudioPlayer = GetComponent<AudioSource>();

        //렌더러 컴포넌트는 자식 게임 오브젝트에 있으므로 GetComponentInChildren() 메서드 사용
        zombieRenderer = GetComponentInChildren<Renderer>();
    }

    public void Setup(ZombieData zombieData) //좀비 AI의 초기 스펙을 결정하는 셋업 메서드
    {
        //체력 설정
        this.startingHealth = zombieData.health;
        this.health = zombieData.health;

        //공격력 설정
        this.damage = zombieData.damage;

        //내비메시 에이전트의 이동 속도 설정
        this.navMeshAgent.speed = zombieData.speed;

        //렌더러가 사용 중인 머터리얼의 컬러를 변경, 외형 색이 변함
        this.zombieRenderer.material.color = zombieData.skinColor;

    }

    private void Start() //게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
    {
        StartCoroutine(UpdatePath()); //코루틴 메서드 발동
    }

    private void Update() //추적 대상의 존재 여부에 따라 다른 애니메이션 재생
    {
        zombieAnimator.SetBool("HasTarget", hasTarget);
    }

    private IEnumerator UpdatePath() //주기적으로 추적할 대상의 위치를 찾아 경로 갱신
                                     //IEnumerator(열거자) 코루틴 발동 메서드, 시간 지연
    {
        while (!dead) //살아 있는 동안 무한 루프
                      ///죽기 전까지
        {
          
            if (hasTarget)
            {
                //추적 대상 존재 : 경로를 갱신하고 AI 이동을 계속 진행
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetEntity.transform.position);
            }
            else
            {
                //추적 대상 없음 : AI 이동 중지
                navMeshAgent.isStopped = true;

                //20유닛의 반지름을 가진 가상의 구를 그렸을 때 / 구와 겹치는 모든 콜라이더를 가져옴
                //단 whatIsTarget 레이어를 가진 콜라이더만 가져오도록 필터링
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);
                                      //물리안의 런타임중(실시간) 가상의 구 생성
                                      //레이어마스크는 int 타입으로 반환 (인덱스 번호)
                                      //멀티 네트워크를 위해 배열로 가져와여


                //모든 콜라이더를 순회하면서 살아 있는 LivingEntity 찾기
                for (int i =0; i<colliders.Length; i++)
                {
                    //콜라이더로부터 LivingEntity 컴포넌트 가져오기
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    //LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아 있다면
                    if(livingEntity != null && !livingEntity.dead)
                    {
                        //추적 대상을 해당 LivingEntity로 설정
                        targetEntity = livingEntity;

                        //for문 루프 즉시 정지
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(0.25f); //0.25초 주기로 처리 반복
        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
        //대미지를 입었을 때 실행할 처리
    {
       //아직 사망하지 않은 경우에만 피격 효과 재생
       if (!dead)
        {
            //공격받은 지점과 방향으로 파티클 효과 재생
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            //피격 효과음 재생
            zombieAudioPlayer.PlayOneShot(hitSound);
        }
        
        base.OnDamage(damage, hitPoint, hitNormal);
        //LivingEntity의 OnDamage()를 실행하여 대미지 적용
    }

    public override void Die() //사망 처리
    {
        base.Die();
        //LivingEntity의 Die()를 실행하여 기본 사망 처리 실행

        Collider[] zombieColliders = GetComponents<Collider>();
        for (int i = 0; i<zombieColliders.Length; i++)
        {
            zombieColliders[i].enabled = false;
        }

        //AI 추적을 중지하고 내비메시 컴포넌트 비활성화
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        zombieAnimator.SetTrigger("Die"); //사망 애니메이션 재생
        zombieAudioPlayer.PlayOneShot(deathSound); //사망 효과음 재생
    }

    private void OnTriggerStay(Collider other)
        //트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
    {
        //자신이 사망하지 않았으며, 최근 공격 시점에서 timeBetAttack 이상 시간이 지났다면 공격 가능
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            //상대방의 LivingEntity 타입 가져오기 시도
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();

            //상대방의 LivingEntity가 자신의 추적 대상이라면 공격 실행
            if (attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time; //최근 공격 시간 갱신

                //상대방의 피격 위치와 피격 방향ㅇ을 근삿값으로 계산
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;

                //공격 실행
                attackTarget.OnDamage(damage, hitPoint, hitNormal);
            }
        }


    }
} 
