using System.Collections;
using UnityEngine;
using UnityEngine.AI; //AI, ������̼� �ý��� ���� �ڵ� ��������

public class Zombie: LivingEntity //���� AI ����
{
    public LayerMask whatIsTarget; //���� ��� ���̾�

    private LivingEntity targetEntity; //�������
    private NavMeshAgent navMeshAgent; //��� ��� AI ������Ʈ

    public ParticleSystem hitEffect; //�ǰ� �� ����� ��ƼŬ ȿ��
    public AudioClip deathSound; //��� �� ����� �Ҹ�
    public AudioClip hitSound; //�ǰ� �� ����� �Ҹ�

    private Animator zombieAnimator; //�ִϸ����� ������Ʈ
    private AudioSource zombieAudioPlayer; //����� �ҽ� ������Ʈ
    private Renderer zombieRenderer; //������ ������Ʈ

    public float damage = 20f; //���ݷ�
    public float timeBetAttack = 0.5f; //���� ����
    private float lastAttackTime; //������ ���� ����
 
    private bool hasTarget //������ ����� �����ϴ��� �˷��ִ� ������Ƽ(�б� ���� get ����, set ����)
    {
       get
        {
            if (targetEntity != null && !targetEntity.dead) 
                //������ ����� �����ϰ�, ����� ������� �ʾҴٸ� true
            {
                return true;
            }

            return false; //�׷��� �ʴٸ� false
        }
    }

    private void Awake() //�ʱ�ȭ
    {
        //���� ������Ʈ�κ��� ����� ������Ʈ ��������
        navMeshAgent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieAudioPlayer = GetComponent<AudioSource>();

        //������ ������Ʈ�� �ڽ� ���� ������Ʈ�� �����Ƿ� GetComponentInChildren() �޼��� ���
        zombieRenderer = GetComponentInChildren<Renderer>();
    }

    public void Setup(ZombieData zombieData) //���� AI�� �ʱ� ������ �����ϴ� �¾� �޼���
    {
        //ü�� ����
        startingHealth = zombieData.health;
        health = zombieData.damage;

        //���ݷ� ����
        damage = zombieData.damage;

        //����޽� ������Ʈ�� �̵� �ӵ� ����
       /// pathMeshAgent.speed = zombieData.speed;

        //�������� ��� ���� ���͸����� �÷��� ����, ���� ���� ����
        zombieRenderer.material.color = zombieData.skinColor;

    }

    private void Start() //���� ������Ʈ Ȱ��ȭ�� ���ÿ� AI�� ���� ��ƾ ����
    {
        StartCoroutine(UpdatePath());
    }

    private void Update() //���� ����� ���� ���ο� ���� �ٸ� �ִϸ��̼� ���
    {
        zombieAnimator.SetBool("HasTarget", hasTarget);
    }

    private IEnumerator UpdatePath() //�ֱ������� ������ ����� ��ġ�� ã�� ��� ����
                                     //IEnumerator(������) �ڷ�ƾ �ߵ� �޼���, �ð� ����
    {
        while (!dead) //��� �ִ� ���� ���� ����
        {
          
            if (hasTarget)
            {
                //���� ��� ���� : ��θ� �����ϰ� AI �̵��� ��� ����
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetEntity.transform.position);
            }
            else
            {
                //���� ��� ���� : AI �̵� ����
                navMeshAgent.isStopped = true;

                //20������ �������� ���� ������ ���� �׷��� �� ���� ��ġ�� ��� �ݶ��̴��� ������
                //�� whatIsTarget ���̾ ���� �ݶ��̴��� ���������� ���͸�
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);

                //��� �ݶ��̴��� ��ȸ�ϸ鼭 ��� �ִ� LivingEntity ã��
                for (int i =0; i<colliders.Length; i++)
                {
                    //�ݶ��̴��κ��� LivingEntity ������Ʈ ��������
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    //LivingEntity ������Ʈ�� �����ϸ�, �ش� LivingEntity�� ��� �ִٸ�
                    if(livingEntity != null && !livingEntity.dead)
                    {
                        //���� ����� �ش� LivingEntity�� ����
                        targetEntity = livingEntity;

                        //for�� ���� ��� ����
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(0.25f); //0.25�� �ֱ�� ó�� �ݺ�
        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
        //������� �Ծ��� �� ������ ó��
    {
       //���� ������� ���� ��쿡�� �ǰ� ȿ�� ���
       if (!dead)
        {
            //���ݹ��� ������ �������� ��ƼŬ ȿ�� ���
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            //�ǰ� ȿ���� ���
            zombieAudioPlayer.PlayOneShot(hitSound);
        }
        
        base.OnDamage(damage, hitPoint, hitNormal);
        //LivingEntity�� OnDamage()�� �����Ͽ� ����� ����
    }

    public override void Die() //��� ó��
    {
        base.Die();
        //LivingEntity�� Die()�� �����Ͽ� �⺻ ��� ó�� ����

        Collider[] zombieColliders = GetComponents<Collider>();
        for (int i = 0; i<zombieColliders.Length; i++)
        {
            zombieColliders[i].enabled = false;
        }

        //AI ������ �����ϰ� ����޽� ������Ʈ ��Ȱ��ȭ
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        zombieAnimator.SetTrigger("Die"); //��� �ִϸ��̼� ���
        zombieAudioPlayer.PlayOneShot(deathSound); //��� ȿ���� ���
    }

    private void OnTriggerStay(Collider other)
        //Ʈ���� �浹�� ���� ���� ������Ʈ�� ���� ����̶�� ���� ����
    {
        //�ڽ��� ������� �ʾ�����, �ֱ� ���� �������� timeBetAttack �̻� �ð��� �����ٸ� ���� ����
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            //������ LivingEntity Ÿ�� �������� �õ�
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();

            //������ LivingEntity�� �ڽ��� ���� ����̶�� ���� ����
            if (attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time; //�ֱ� ���� �ð� ����

                //������ �ǰ� ��ġ�� �ǰ� ���⤷�� �ٻ����� ���
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;

                //���� ����
                attackTarget.OnDamage(damage, hitPoint, hitNormal);
            }
        }


    }
}
