using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   //����� �Է¿� ���� �÷��̾� ĳ���͸� �����̴� ��ũ��Ʈ

    //���⼺ ���ʹ� PlayerInput���� �޾Ƽ� ��
    public float moveSpeed = 5f; //�յ� �������� �ӵ�
    public float rotateSpeed = 180f; //���� ȸ�� �ӵ�

    ///[SerializeField] private float moveSpeed = 5f;
    ///���м��� ��Ű�� ����Ƽ �����ͳ�(inspector)������ ��������
    ///�����ð������� ������ �ڷ�ƾ �Լ����� (envoke���� �ڷ�ƾ) 
    ///������ ������ ���ۺκп��� ����, ����ȭ/������ȭ, �߻����� �����͸� 0��1�� ���� �����ϴ� ��

    private PlayerInput playerInput; //�÷��̾� �Է��� �˷��ִ� ������Ʈ

    private Rigidbody playerRigidbody; //�÷��̾� ĳ������ ������ٵ�
    //������ ó�� = ����(Risdbody)   / AddForce, Velocity -���� �����ϴ� ��Ȳ �߻� �� �� ����
    //           = ��ġ(transform)  / �����̵�position, transfate
    //���� �����ֱ� ��ȣ�ϸ鼭 ������ ó�� / ������ �ִ� ��ġ��

    private Animator playerAnimator; //�÷��̾� ĳ������ �ִϸ�����


    private void Start() 
        //����� ������Ʈ���� ���� ��������
    {
        playerInput = GetComponent<PlayerInput>(); 
        playerRigidbody = GetComponent<Rigidbody>(); 
        playerAnimator = GetComponent<Animator>(); 
    }


    private void FixedUpdate()
        //FixedUpdate�� ���� ���� �ֱ⿡ ���� �����
        //�������� �ֱ⸶�� ������, ȸ��, �ִϸ��̼� ó�� ����
        //Rotate, Move �޼���� ������ٵ� �ǵ�⶧���� ������ ������ ���̱� ���� FixedUpdate�� ����

    {
        Rotate(); //ȸ�� ����
       
        Move(); //������ ����

        playerAnimator.SetFloat("Move", playerInput.move);
        //�Է°�(playerInput.move�� ������Ƽ)�� ���� �ִϸ������� Move �Ķ���Ͱ� ����
    }


    private void Move() 
        //�Է°��� ���� ĳ���͸� �յڷ� ������
    {
        Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        //��������� �̵��� �Ÿ� ���
        //playerInput.move(���⼺) * transform.forward(���� �ӱ�, �յ� �����Ӹ� ����) * moveSpeed(�ӵ�) * Time.deltaTime(�ð�)

        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance); 
        //������ٵ� �̿��� ���� ������Ʈ ��ġ ����
    }


    private void Rotate() 
        //�Է°��� ���� ĳ���͸� �¿�� ȸ��
    {
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime; //��������� ȸ���� ��ġ ��� //Y�� �����̶� 

        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0f);
        //������ٵ� �̿��� ���� ������Ʈ ȸ�� ����
    }

}
