using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   //사용자 입력에 따라 플레이어 캐릭터를 움직이는 스크립트

    //방향성 벡터는 PlayerInput에서 받아서 옴
    public float moveSpeed = 5f; //앞뒤 움직임의 속도
    public float rotateSpeed = 180f; //좌위 회전 속도

    ///[SerializeField] private float moveSpeed = 5f;
    ///은닉성은 지키되 유니티 에디터내(inspector)에서는 보여진다
    ///지연시간에서도 쓰여요 코루틴 함수같은 (envoke보단 코루틴) 
    ///원래는 데이터 전송부분에서 쓰임, 직렬화/역직렬화, 추상적인 데이터를 0과1로 만들어서 전송하는 것

    private PlayerInput playerInput; //플레이어 입력을 알려주는 컴포넌트

    private Rigidbody playerRigidbody; //플레이어 캐릭터의 리지드바디
    //움직임 처리 = 물리(Risdbody)   / AddForce, Velocity -물리 무시하는 상황 발생 할 수 있음
    //           = 위치(transform)  / 순간이동position, transfate
    //물리 갱신주기 보호하면서 움직임 처리 / 물리에 있는 위치값

    private Animator playerAnimator; //플레이어 캐릭터의 애니메이터


    private void Start() 
        //사용할 컴포넌트들의 참조 가져오기
    {
        playerInput = GetComponent<PlayerInput>(); 
        playerRigidbody = GetComponent<Rigidbody>(); 
        playerAnimator = GetComponent<Animator>(); 
    }


    private void FixedUpdate()
        //FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
        //물리갱신 주기마다 움직임, 회전, 애니메이션 처리 실행
        //Rotate, Move 메서드다 리지드바디를 건들기때문에 물리적 오차를 줄이기 위해 FixedUpdate에 쓴다

    {
        Rotate(); //회전 실행
       
        Move(); //움직임 실행

        playerAnimator.SetFloat("Move", playerInput.move);
        //입력값(playerInput.move는 프로퍼티)에 따라 애니메이터의 Move 파라미터값 변경
    }


    private void Move() 
        //입력값에 따라 캐릭터를 앞뒤로 움직임
    {
        Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        //상대적으로 이동할 거리 계산
        //playerInput.move(방향성) * transform.forward(벡터 속기, 앞뒤 움직임만 존재) * moveSpeed(속도) * Time.deltaTime(시간)

        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance); 
        //리지드바디를 이용해 게임 오브젝트 위치 변경
    }


    private void Rotate() 
        //입력값에 따라 캐릭터를 좌우로 회전
    {
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime; //상대적으로 회전할 수치 계산 //Y축 고정이라서 

        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0f);
        //리지드바디를 이용해 게임 오브젝트 회전 변경
    }

}
