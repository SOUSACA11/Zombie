using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   //����� �Է¿� ���� �÷��̾� ĳ���͸� �����̴� ��ũ��Ʈ

    //���⼺ ���ʹ� PlayerInput���� �޾Ƽ� ��
    public float moveSpeed = 5f; //�յ� �������� �ӵ�
    public float rotateSpeed = 180f; //���� ȸ�� �ӵ�

    ///[SerializeField] private float moveSpeed = 5f;//////////////////////////////////

    private PlayerInput playerInput; //�÷��̾� �Է��� �˷��ִ� ������Ʈ

    private Rigidbody playerrigidbody; //�÷��̾� ĳ������ ������ٵ�
    //������ ó�� = ����(Risdbody)   / AddForce, Velocity -���� �����ϴ� ��Ȳ �߻� �� �� ����
    //           = ��ġ(transform)  / �����̵�position, transfate
    //���� �����ֱ� ��ȣ�ϸ鼭 ������ ó�� / ������ �ִ� ��ġ��

    private Animator playerAnimator; //�÷��̾� ĳ������ �ִϸ�����


    private void Start() 
        //����� ������Ʈ���� ���� ��������
    {
        playerInput = GetComponent<PlayerInput>();
        playerrigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }


    private void FixedUpdate() 
        //FixedUpdate�� ���� ���� �ֱ⿡ ���� �����
        //�������� �ֱ⸶�� ������, ȸ��, �ִϸ��̼� ó�� ����
    {
        
    }


    private void Move() 
        //�Է°��� ���� ĳ���͸� �յڷ� ������
    {
        
    }


    private void Rotate() 
        //�Է°��� ���� ĳ���͸� �¿�� ȸ��
    {

    }

}
