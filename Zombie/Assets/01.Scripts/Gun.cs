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

    private void OnEnable() //�� ���� �ʱ�ȭ /������Ʈ Ȱ��ȭ.��Ȱ��ȭ
    {
        ammoRemain = gunData.startAmmoRemain; //��ü ���� ź�� ���� �ʱ�ȭ

    }

    public void Fire() //�߻� �õ� /���� �߻� ������ �������� ����(�Ѿ��� �ֳ� ����)
    {

    }

    private void Shot() //���� �߻� ó�� /�߻� ������ �����ϰ�� �߻�
    {

    }

    private IEnumerator ShotEffect(Vector3 hitPosition) //�߻� ����Ʈ�� �Ҹ��� ����ϰ� ź�� ������ �׸�
    {

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
