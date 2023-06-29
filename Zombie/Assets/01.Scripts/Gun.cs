using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour //총 자체의 기능 구현
{
    public enum State //총의 상태를 표현하는 데 사용할 타입을 선언 /총의 상태 정의
    {
        Ready, Empty, Reloading //발사 준비됨, 탄창이 빔, 재장전 중
    }

    public State state { get; private set; } //현재 총의 상태

    private Transform fireTransform; //탄알이 발사될 위치

    public ParticleSystem muzzleFlashEffect; //총구 화염 효과
    public ParticleSystem shellEjectEffext; //탄피 배출 효과

    private LineRenderer bulletLineRenderer; //탄알 궤적을 그리기 위한 렌더러

    private AudioSource gunAudioplayer; //총 소리 재생기

    public GunData gunData; //총의 현재 데이터

    private float fireDistance = 50f; //사정거리

    public int ammoRemain = 100; //총 자체의 남은 전체 탄알 / GunData의 ammoRemain는 처음에 주어질 탄알
    public int magAmmo; //현재 탄창에 남아 있는 탄알

    private float lastFireTime; //총을 마지막으로 발사한 시점 

    private void Awake() //사용할 컴포넌트의 참조 가져오기
    {
        gunAudioplayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2; //사용할 점을 두개로 변경
        bulletLineRenderer.enabled = false; //라인 렌더러를 비활성화

        //fireTransform = transform.Find("FireTransform"); //자식 객체 중에서 "FireTransform"이라는 이름을 가진 Transform 컴포넌트 찾기
        //fireTransform = GetComponentsInChildren<Transform>()[9]; //배열 로서 가져오니깐 10번째 /다차원
        fireTransform = transform.GetChild(4); //1차원
    }

    private void OnEnable() //총 상태 초기화 /컴포넌트 활성화시 발동
    {
        ammoRemain = gunData.startAmmoRemain; //전체 예비 탄알 양을 초기화
        magAmmo = gunData.magCapacity; //현재 탄창을 가득 채우기

        state = State.Ready; //총의 현재 상태를 총을 쏠 준비가 된 상태로 변경
        lastFireTime = 0; //마지막으로 총을 쏜 시점을 초기화

    }

    public void Fire() //발사 시도 /현재 발사 가능한 상태인지 점검(총알이 있나 없나)
    {
      if (state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        // 현재 발사 가능한 상태 &&(이면서) 마지막 총 발사 시점에서 gunData.timeBetFire 이상의 시간이 지남
        {
            lastFireTime = Time.time; //마지막 총 발사 시점 갱신

            Shot(); //실제 발사 처리 실행
        }
    }

    private void Shot() //실제 발사 처리 /발사 가능한 상태일경우 발사
    {
        RaycastHit hit; //레이캐스트에 의한 충돌 정보를 저장하는 컨테이너

        //-Ray ray = new Ray(fireTransform.position, fireTransform.forward);
        //-레이캐스트에서 사용할 레이의 정보를 저장하는 변수

        Vector3 hitPosition = Vector3.zero; //탄알이 맞은 곳을 저장할 변수   
        
         //-if (Physics.Raycast(ray, out hit, fireDistance))
        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
         //레이캐스트(시작 지점, 방향, 충돌 정보 컨테이너, 사정거리        
        {
            //레이가 어떤 물체와 충돌한 경우

            IDamageble target = hit.collider.GetComponent<IDamageble>();
            //충돌한 상대방으로부터 IDamageble 오브젝트 가져오기 시도
            //지역변수 target

            if (target  != null) //상대방으로부터 IDamageble 오브젝트를 가져오는 데 성공했다면
            {
                target.OnDamage(gunData.damage, hit.point, hit.normal);
                //상대방의 OnDamage 함수를 실행시켜 상대방에 대미지 주기
            }

            hitPosition = hit.point; //레이가 충돌한 위치 저장
        }

        else //레이가 다른 물체와 충돌하지 않았다면
        {
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
            //탄알이 최대 사정거리까지 날아갔을 때의 위치를 충돌 위치로 사용
        }

        StartCoroutine(ShotEffect(hitPosition)); //발사 이펙트 재생 시작
                                                 //코루틴 /매개변수 hitPosition 넘겨주기

        magAmmo--; //남은 탄알 수를 -1
        if(magAmmo <= 0)
        {
            state = State.Empty; //탄창에 남은 탄알이 없다면 총의 현재 상태를 Empty(대기상태)로 갱신
        }
        //if (--magAmmo <= 0) state = State.Empty;
    }

    private IEnumerator ShotEffect(Vector3 hitPosition) //발사 이펙트와 소리를 재생하고 탄알 궤적을 그림
    {
        muzzleFlashEffect.Play(); //총구 화염 효과 재생
        shellEjectEffext.Play(); //탄피 배출 효과 재생

        gunAudioplayer.PlayOneShot(gunData.shotClip); //총격 소리 재생

        bulletLineRenderer.SetPosition(0, fireTransform.position); //선의 시작점은 총구의 위치
        bulletLineRenderer.SetPosition(1, hitPosition); //선의 끝점은 입력으로 들어온 충돌 위치
        bulletLineRenderer.enabled = true; //라인 렌더러를 활성화하여 탄알 궤적을 그림

        yield return new WaitForSeconds(0.03f); //0.03초 동안 잠시 처리를 대기
                       //WaitForEndOfFrame - 예외처리(조건식) 상황에서 적용 /한프레임만 넘기겠다
       
        bulletLineRenderer.enabled = false; //라인 렌더러를 비활성화하여 탄알 궤적을 지움
    }

    public bool Reload() //재장전 시도 / 재장전 가능한 상태인지 점검
    {
       if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= gunData.magCapacity)
            //이미 재장전 중이거나 /남은 탄알이 없거나 /탄창에 탄알이 이미 가득한 경우 재장전 할 수 없음
        {
            return false; //점프문(return: 메서드 벗어남 / breack : 스코프 벗어남)
        }

        StartCoroutine(ReloadRoutine()); //재장전 처리 시작
        return true;
    }

    private IEnumerator ReloadRoutine() //실제 재장전 처리를 진행
    {
        state = State.Reloading; //현재 상태를 재장전 중 상태로 전환

        gunAudioplayer.PlayOneShot(gunData.reloadClip); //재장전 소리 재생

        yield return new WaitForSeconds(gunData.reloadTime); //재장전 소요 시간만큼 처리 쉬기

       
        int ammoToFill = gunData.magCapacity - magAmmo; //탄창에 채울 탄알 계산 /ammoToFill 지역변수
                       //탄창 최대 용량 - 현재 탄창에 남아있는 탄알

        if (ammoRemain < ammoToFill) 
            //탄창에 채워야 할 탄알이 남은 탄알보다 많다면 채워야 할 탄알 수를 남은 탄알 수에 맞춰 줄임
        {
            ammoToFill = ammoRemain;
        }

        magAmmo += ammoToFill; //탄창을 채움
        ammoRemain -= ammoToFill; //남은 탄알에서 탄창에 채운만큼 탄알을 뺌
        
        state = State.Ready; //총의 현재 상태를 발사 준비된 상태로 변경 

    }

}
