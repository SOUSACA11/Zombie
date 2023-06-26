using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour //총 자체의 기능 구현
{
    public enum State //총의 상태를 표현하는 데 사용할 타입을 선언 /총의 상태 정의
    {
        Ready, Empty, Reloading //발사 준비됨, 탄창이 빔, 재장전 중
    }

    public State state { get; private set; } //현재 총의 상태

    private Transform fireTransform; //탄알이 발사될 위치

    public ParticleSystem muzzleFlashEffect; //총구 화염 효과
    public ParticleSystem shellEjectEffext; //탄피 배출 효과

    private LineRenderer bulletLineRenderer; //탄알 궤적을 그리기 위한 렌더러

    private AudioSource gunAudioplayer; //총 소리 재생기

    public GunData gunData; //총의 현재 데이터

    private float fireDistance = 50f; //사정거리

    public int ammoRemain = 100; //총 자체의 남은 전체 탄알 / GunData의 ammoRemain는 처음에 주어질 탄알
    public int magAmmo; //현재 탄창에 남아 있는 탄알

    private float lastFireTime; //총을 마지막으로 발사한 시점 

    private void Awake() //사용할 컴포넌트의 참조 가져오기
    {
        gunAudioplayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2; //사용할 점을 두개로 변경
        bulletLineRenderer.enabled = false; //라인 렌더러를 비활성화

        //fireTransform = transform.Find("FireTransform"); //자식 객체 중에서 "FireTransform"이라는 이름을 가진 Transform 컴포넌트 찾기
        //fireTransform = GetComponentInChildren<Transform>()[9]; //배열 로서 가져오니깐 9번째 /다차원
        fireTransform = transform.GetChild(3); //1차원
    }

    private void OnEnable() //총 상태 초기화 /컴포넌트 활성화.비활성화
    {
        ammoRemain = gunData.startAmmoRemain; //전체 예비 탄알 양을 초기화

    }

    public void Fire() //발사 시도 /현재 발사 가능한 상태인지 점검(총알이 있나 없나)
    {

    }

    private void Shot() //실제 발사 처리 /발사 가능한 상태일경우 발사
    {

    }

    private IEnumerator ShotEffect(Vector3 hitPosition) //발사 이펙트와 소리를 재생하고 탄알 궤적을 그림
    {

        bulletLineRenderer.enabled = true; //라인 렌더러를 활성화하여 탄알 궤적을 그림

        yield return new WaitForSeconds(0.03f); //0.03초 동안 잠시 처리를 대기
                       //WaitForEndOfFrame - 예외처리(조건식) 상황에서 적용 /한프레임만 넘기겠다

        bulletLineRenderer.enabled = false; //라인 렌더러를 비활성화하여 탄알 궤적을 지움
    }

    public bool Reload() //재장전 시도 / 재장전 가능한 상태인지 점검
    {
        return false;
    }

    private IEnumerator ReloadRoutine() //실제 재장전 처리를 진행
    {
        state = State.Reloading; //현재 상태를 재장전 중 상태로 전환

        yield return new WaitForSeconds(gunData.reloadTime); //재장전 소요 시간만큼 처리 쉬기

        state = State.Ready; //총의 현재 상태를 발사 준비된 상태로 변경 

    }

}
