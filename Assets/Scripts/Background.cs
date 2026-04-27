using UnityEngine;

/// <summary>
/// 카메라 이동에 따라 배경을 일정 비율로 움직여 원근감을 주는 스크립트
/// </summary>
public class Background : MonoBehaviour
{
    public Transform mainCamera; // 추적할 메인 카메라의 트랜스폼

    public float scrollSpeed = 0.5f;
    // 배경이 카메라를 따라가는 속도 비율 (0이면 고정, 1이면 카메라와 동일하게 이동)

    private Vector3 lastCameraPosition; // 이전 프레임의 카메라 위치 기록용
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // 게임 시작 시, 오브젝트 활성화 시 첫 프레임 업데이트 전에 호출
    void Start()
    {
        // 스크립트 시작 시점의 카메라 위치를 초기화하여 갑작스러운 배경 튀김 방지
        lastCameraPosition = mainCamera.position;
    }

    // 매 프레임마다 호출
    // Update is called once per frame
    void Update()
    {   // 1. 카메라가 이전 프레임으로부터 얼마나 움직였는지 계산(변위값)
        Vector3 deltaMovement = mainCamera.position - lastCameraPosition;
        // 2. 배경의 위치를 업데이트
        // X축 방향으로만 카메라 이동량의 scrollSpeed 비율만큼 이동
        // Y축과 Z축은 우너래 위치를 유지 (0f)
        transform.position += new Vector3(deltaMovement.x * scrollSpeed, 0f, 0f);
        // 3. 현재 카메라 위치를 저장하여 다음 프레임 계산에 사용
        lastCameraPosition = mainCamera.position;
    }
}
