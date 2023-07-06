using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
    //�÷��̾� ĳ������ ����ü�μ��� ������ ���
{
    public Slider healthSlider; //ü���� ǥ���� UI �����̴�

    public AudioClip deathClip; //��� �Ҹ�
    public AudioClip hitClip; //�ǰ� �Ҹ�
    public AudioClip itemPickupClip; //������ ���� �Ҹ�

    private AudioSource playerAudioPlayer; //�÷��̾� �Ҹ� �����
    private Animator playerAnimator; //�÷��̾� �ִϸ�����

    private PlayerMovement playerMovement; //�÷��̾� ������ ������Ʈ
    private PlayerShooter playerShooter; //�÷��̾� ���� ������Ʈ

    private void Awake() 
        //����� ������Ʈ ��������
    {
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();

        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable() 
        //LivingEntity�� OnEnable()����,���� �ʱ�ȭ
    {
        base.OnEnable();

        healthSlider.gameObject.SetActive(true);
        //ü�� �����̴� Ȱ��ȭ
        healthSlider.maxValue = startingHealth;
        //ü�� �����̴��� �ִ��� �⺻ ü�°����� ����
        healthSlider.value = health;
        //ü�� �����̴��� ���� ���� ü�°����� ����

        playerMovement.enabled = true;
        playerShooter.enabled = true;
        //�÷��̾� ������ �޴� ������Ʈ Ȱ��ȭ
    }

    public override void RestoreHealth(float newHealth)
        //ü�� ȸ��
        //LivingEntity�� RestoreHealth()����, ü�� ����
    {
        base.RestoreHealth(newHealth);

        healthSlider.value = health;
        //���ŵ� ü������ ü�� �����̴� ����
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
        //����� ó��
        //LivingEntity�� OnDamage()����, ����� ����
    {
        if (!dead) //������� ���� ��쿡�� ȿ���� ���
        {
            playerAudioPlayer.PlayOneShot(hitClip);
        }

        //if (dead) return;
        
        base.OnDamage(damage, hitPoint, hitDirection);

        healthSlider.value = health; //���ŵ� ü���� ü�� �����̴��� �ݿ�

    }

    public override void Die()
        //���ó��
        //LivingEntity�� Die()����, ��� ����
    {
        base.Die();

        healthSlider.gameObject.SetActive(false); //ü�� �����̴� ��Ȱ��ȭ
                                                  //(SetActive = ������Ʈ Ȱ����Ȱ��ȭ)
                                                  //(enabled = ������Ʈ Ȱ����Ȱ��ȭ)

        playerAudioPlayer.PlayOneShot(deathClip); //����� ���
        playerAnimator.SetTrigger("Die"); //�ִϸ������� Die Ʈ���Ÿ� �ߵ����� ��� �ִϸ��̼� ���

        playerMovement.enabled = false;
        playerShooter.enabled = false;
        //�÷��̾� ������ �޴� ������Ʈ ��Ȱ��Ȱ
    }

    private void OnTriggerEnter(Collider other)
        //�����۰� �浹�� ��� �ش� �������� ����ϴ� ó��
    {
        if(!dead)
        {
            IItem item = other.GetComponent<IItem>();
            //�浹�� �������κ��� IItem ������Ʈ �������� �õ�

            if (item != null)
            //�浹�� �������κ��� IItem ������Ʈ�� �������� �� �����ߴٸ�
            {
                item.Use(gameObject); //Use �޼��带 �����Ͽ� ������ ���
                playerAudioPlayer.PlayOneShot(itemPickupClip); //������ ���� �Ҹ� ���*
            }
        }
    }
}
//������ : �θ� ��ӹ��� �ڽ��� �θ�Ÿ�����ε� ��� ����