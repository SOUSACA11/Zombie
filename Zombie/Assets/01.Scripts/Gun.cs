using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour //�� ��ü�� ��� ����
{
    public enum State //���� ���¸� ǥ���ϴ� �� ����� Ÿ���� ���� /���� ���� ����
    {
        Ready, Empty, Reloading //�߻� �غ��, źâ�� ��, ������ ��
    }

    public State state { get; private set; } //���� ���� ����

    private Transform fireTransform; //ź���� �߻�� ��ġ

    public ParticleSystem muzzleFlashEffect; //�ѱ� ȭ�� ȿ��
    public ParticleSystem shellEjectEffext; //ź�� ���� ȿ��

    private LineRenderer bulletLineRenderer; //ź�� ������ �׸��� ���� ������

    private AudioSource gunAudioplayer; //�� �Ҹ� �����

    public GunData gunData; //���� ���� ������

    private float fireDistance = 50f; //�����Ÿ�

    public int ammoRemain = 100; //�� ��ü�� ���� ��ü ź�� / GunData�� ammoRemain�� ó���� �־��� ź��
    public int magAmmo; //���� źâ�� ���� �ִ� ź��

    private float lastFireTime; //���� ���������� �߻��� ���� 

    private void Awake() //����� ������Ʈ�� ���� ��������
    {
        gunAudioplayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2; //����� ���� �ΰ��� ����
        bulletLineRenderer.enabled = false; //���� �������� ��Ȱ��ȭ

        //fireTransform = transform.Find("FireTransform"); //�ڽ� ��ü �߿��� "FireTransform"�̶�� �̸��� ���� Transform ������Ʈ ã��
        //fireTransform = GetComponentInChildren<Transform>()[9]; //�迭 �μ� �������ϱ� 9��° /������
        fireTransform = transform.GetChild(3); //1����
    }

    private void OnEnable() //�� ���� �ʱ�ȭ /������Ʈ Ȱ��ȭ�� �ߵ�
    {
        ammoRemain = gunData.startAmmoRemain; //��ü ���� ź�� ���� �ʱ�ȭ
        magAmmo = gunData.magCapacity; //���� źâ�� ���� ä���

        state = State.Ready; //���� ���� ���¸� ���� �� �غ� �� ���·� ����
        lastFireTime = 0; //���������� ���� �� ������ �ʱ�ȭ

    }

    public void Fire() //�߻� �õ� /���� �߻� ������ �������� ����(�Ѿ��� �ֳ� ����)
    {
      if (state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        // ���� �߻� ������ ���� &&(�̸鼭) ������ �� �߻� �������� gunData.timeBetFire �̻��� �ð��� ����
        {
            lastFireTime = Time.time; //������ �� �߻� ���� ����

            Shot(); //���� �߻� ó�� ����
        }
    }

    private void Shot() //���� �߻� ó�� /�߻� ������ �����ϰ�� �߻�
    {
        RaycastHit hit; //����ĳ��Ʈ�� ���� �浹 ������ �����ϴ� �����̳�
        Vector3 hitPosition = Vector3.zero; //ź���� ���� ���� ������ ����

        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
         //����ĳ��Ʈ(���� ����, ����, �浹 ���� �����̳�, �����Ÿ�)
        {
           
            //���̰� � ��ü�� �浹�� ���

            IDamageble target = hit.collider.GetComponent<IDamageble>();
            //�浹�� �������κ��� IDamageble ������Ʈ �������� �õ�

            if (target  != null) //�������κ��� IDamageble ������Ʈ�� �������� �� �����ߴٸ�
            {
                target.OnDamage(gunData.damage, hit.point, hit.normal);
                //������ OnDamage �Լ��� ������� ���濡 ����� �ֱ�
            }

            hitPosition = hit.point; //���̰� �浹�� ��ġ ����
        }

        else //���̰� �ٸ� ��ü�� �浹���� �ʾҴٸ�
        {
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
            //ź���� �ִ� �����Ÿ����� ���ư��� ���� ��ġ�� �浹 ��ġ�� ���
        }

        StartCoroutine(ShotEffect(hitPosition)); //�߻� ����Ʈ ��� ����

        magAmmo--; //���� ź�� ���� -1
        if(magAmmo <= 0)
        {
            state = State.Empty; //źâ�� ���� ź���� ���ٸ� ���� ���� ���¸� Empty�� ����
        }
    }

    private IEnumerator ShotEffect(Vector3 hitPosition) //�߻� ����Ʈ�� �Ҹ��� ����ϰ� ź�� ������ �׸�
    {
        muzzleFlashEffect.Play(); //�ѱ� ȭ�� ȿ�� ���
        shellEjectEffext.Play(); //ź�� ���� ȿ�� ���

        gunAudioplayer.PlayOneShot(gunData.shotClip); //�Ѱ� �Ҹ� ���

        bulletLineRenderer.SetPosition(0, fireTransform.position); //���� �������� �ѱ��� ��ġ
        bulletLineRenderer.SetPosition(1, hitPosition); //���� ������ �Է����� ���� �浹 ��ġ
        bulletLineRenderer.enabled = true; //���� �������� Ȱ��ȭ�Ͽ� ź�� ������ �׸�

        yield return new WaitForSeconds(0.03f); //0.03�� ���� ��� ó���� ���
                       //WaitForEndOfFrame - ����ó��(���ǽ�) ��Ȳ���� ���� /�������Ӹ� �ѱ�ڴ�
       
        bulletLineRenderer.enabled = false; //���� �������� ��Ȱ��ȭ�Ͽ� ź�� ������ ����
    }

    public bool Reload() //������ �õ� / ������ ������ �������� ����
    {
        return false;
    }

    private IEnumerator ReloadRoutine() //���� ������ ó���� ����
    {
        state = State.Reloading; //���� ���¸� ������ �� ���·� ��ȯ

        yield return new WaitForSeconds(gunData.reloadTime); //������ �ҿ� �ð���ŭ ó�� ����

        state = State.Ready; //���� ���� ���¸� �߻� �غ�� ���·� ���� 

    }

}
