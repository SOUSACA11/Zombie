using UnityEngine;

public class PlayerShooter : MonoBehaviour
    //�־��� GUN ������Ʈ�� ��ų� ������
    //�˸��� �ִϸ��̼��� ����ϰ� IK�� ����� ĳ���� ����� �ѿ� ��ġ�ϵ��� ����
{
    public Gun gun; //����� ��
    public Transform gunPivot; //�� ��ġ�� ������
    public Transform leftHandMount; //���� ���� ������, �޼��� ��ġ�� ����
    public Transform rightHandMount; //���� ������ ������, �������� ��ġ�� ����

    private PlayerInput playerInput; //�÷��̾��� �Է�
    private Animator playerAnimator; //�ִϸ����� ������Ʈ

    private void Start() //����� ������Ʈ ��������
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
    }

    private void OnEnable() //���Ͱ� Ȱ��ȭ�� �� �ѵ� �Բ� Ȱ��ȭ
    {
        gun.gameObject.SetActive(true);
    }

    private void Update() //�Է��� �����ϰ� ���� �߻��ϰų� ������
    {
        if (playerInput.fire) //������Ƽ�� Ȯ��
        {
            gun.Fire(); //�߻� �Է� ���� �� �� �߻� /Gun�� Fire �޼���
        }
        else if (playerInput.reload) //������Ƽ�� Ȯ��
        {
            if (gun.Reload()) //������ �Է� ���� �� ������ /Gun�� Reload �޼���
            {
                playerAnimator.SetTrigger("Reload"); //������ ���� �ÿ��� ������ �ִϸ��̼� ���
            }
        }

        UpdateUI(); //���� ź�� UI ����
    }

    private void UpdateUI() //ź�� UI ����
    {
        if (gun != null && UIManager.instance != null)
        {
            UIManager.instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
            //UI �Ŵ����� ź�� �ؽ�Ʈ�� źâ�� ź�˰� ���� ��ü ź�� ǥ��
        }
    }

    private void OnAnimatorIK(int layerIndex) //�ִϸ������� IK ����
    {
        gunPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);
        //���� ������ gunPivot�� 3D ���� ������ �Ȳ�ġ ��ġ�� �̵�
        //gunPivot�� �ִϸ����Ϳ��� ��ġ�� �����ͼ� ������Ʈ�� ����
        //������Ʈ ��ġ ����

        
        //�ִϸ����� ��ġ ����
        //IK�� ����Ͽ� �޼��� ��ġ�� ȸ���� ���� ���� �����̿� ����
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
       
        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);
 
        
        //IK�� ����Ͽ� �������� ��ġ�� ȸ���� ���� ������ �����̿� ����
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
       
        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);


    }
}
