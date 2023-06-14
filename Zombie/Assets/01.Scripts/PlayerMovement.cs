using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   //사용자 입력에 따라 플레이어 캐릭터를 움직이는 스크립트

    //방향성 벡터는 PlayerInput에서 받아서 옴
    public float moveSpeed = 5f; //앞뒤 움직임의 속도
    public float rotateSpeed = 180f; //좌위 회전 속도

    ///[SerializeField] private float moveSpeed = 5f;//////////////////////////////////

    private PlayerInput playerInput; //플레이어 입력을 알려주는 컴포넌트

    private Rigidbody playerrigidbody; //플레이어 캐릭터의 리지드바디
    //움직임 처리 = 물리(Risdbody)   / AddForce, Velocity -물리 무시하는 상황 발생 할 수 있음
    //           = 위치(transform)  / 순간이동position, transfate
    //물리 갱신주기 보호하면서 움직임 처리 / 물리에 있는 위치값

    private Animator playerAnimator; //플레이어 캐릭터의 애니메이터


    private void Start() 
        //사용할 컴포넌트들의 참조 가져오기
    {
        playerInput = GetComponent<PlayerInput>();
        playerrigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }


    private void FixedUpdate() 
        //FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
        //물리갱신 주기마다 움직임, 회전, 애니메이션 처리 실행
    {
        
    }


    private void Move() 
        //입력값에 따라 캐릭터를 앞뒤로 움직임
    {
        
    }


    private void Rotate() 
        //입력값에 따라 캐릭터를 좌우로 회전
    {

    }

}
