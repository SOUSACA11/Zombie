using System.Collections;
using UnityEngine;
using UnityEngine.AI; //AI, ������̼� �ý��� ���� �ڵ� ��������

public class Zombie : LivingEntity //���� AI ����
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
 
    private bool hasTarget //������ ����� �����ϴ��� �˷��ִ� ������Ƽ
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
    {
        while (!dead) //��� �ִ� ���� ���� ����
        {
            yield return new WaitForSeconds(0.25f); //0.25�� �ֱ�� ó�� �ݺ�
        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
        //������� �Ծ��� �� ������ ó��
    {
        base.OnDamage(damage, hitPoint, hitNormal);
        //LivingEntity�� OnDamage()�� �����Ͽ� ����� ����
    }

    public override void Die() //��� ó��
    {
        base.Die();
        //LivingEntity�� Die()�� �����Ͽ� �⺻ ��� ó�� ����
    }

    private void OnTriggerStay(Collider other)
        //Ʈ���� �浹�� ���� ���� ������Ʈ�� ���� ����̶�� ���� ����
    {
        
    }
}
