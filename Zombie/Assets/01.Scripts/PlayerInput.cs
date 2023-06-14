using UnityEngine;

public class PlayerInput : MonoBehaviour
{   //플레이어 캐릭터를 조작하기 위한 사용자 입력을 감지
    //감지된 입력값을 다른 컴포넌트가 사용할 수 있도록 제공


    //입력매니저에 등록된 이름 
    public string moveAxisName = "Vertical"; //앞 뒤 움직임을 위한 입력축 이름
    public string rotateAxisName = "Horizontal"; //좌우 회전을 위한 입력축 이름
    public string fireButtonName = "Fire1"; //발사를 위한 입력 버튼 이름
    public string reloadButtonName = "Reload"; //재장전을 위한 입력 버튼 이름
    //private const string moveAxisName = "Vertical"; 
    //바뀌는 안되는 것을 상수(const)로 선언


    //자동 구현 프로퍼티 
    //값 할당은 내부에서만 가능
    public float move { get; private set; } //감지된 움직임 입력값
    public float rotate { get; private set; } //감지된 회전 입력값
    public bool fire { get; private set; } //감지된 발사 입력값
    public bool reload { get; private set; } //감지된 재장전 입력값


    //매프레임 사용자 입력을 감지
    private void Update()
    {    
        // 게임오버 상태에서는 사용자 입력을 감지하지 않음
        if (GameManager.instance != null && GameManager.instance.isGameover) //게임매니저 싱글톤 처리
        {
            move = 0;
            rotate = 0;
            fire = false;
            reload = false;
            return;
        }

        move = Input.GetAxis(moveAxisName); //move에 관한 입력 감지
        rotate = Input.GetAxis(rotateAxisName); //rotate에 관한 입력 감지
        fire = Input.GetButton(fireButtonName); //fire에 관한 입력 감지
        reload = Input.GetButtonDown(reloadButtonName); //reload에 관한 입력 감지

    }

}
