using UnityEngine;

public class PlayerInput : MonoBehaviour
{   //�÷��̾� ĳ���͸� �����ϱ� ���� ����� �Է��� ����
    //������ �Է°��� �ٸ� ������Ʈ�� ����� �� �ֵ��� ����

    public string moveAxisName = "Vertical"; //�� �� �������� ���� �Է��� �̸�
    public string rotateAxisName = "Horizontal"; //�¿� ȸ���� ���� �Է��� �̸�
    public string fireButtonName = "Fire1"; //�߻縦 ���� �Է� ��ư �̸�
    public string reloadButtonName = "Reload"; //�������� ���� �Է� ��ư �̸�


    //�� �Ҵ��� ���ο����� ����
    public float move { get; private set; } //������ ������ �Է°�
    public float rotate { get; private set; } //������ ȸ�� �Է°�
    public bool fire { get; private set; } //������ �߻� �Է°�
    public bool reload { get; private set; } //������ ������ �Է°�


    //�������� ����� �Է��� ����
    private void Update()
    {
       if (GameManager.instance != null && GameManager.instance.isGameover) //���ӸŴ��� �̱��� ó��
        {
            move = 0;
            rotate = 0;
            fire = false;
            reload = false;
            return;
        }

        move = Input.GetAxis(moveAxisName); //move�� ���� �Է� ����
        rotate = Input.GetAxis(rotateAxisName); //rotate�� ���� �Է� ����
        fire = Input.GetButton(fireButtonName); //fire�� ���� �Է� ����
        reload = Input.GetButtonDown(reloadButtonName); //reload�� ���� �Է� ����

    }

}
