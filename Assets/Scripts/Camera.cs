using UnityEngine;

/// <summary>
/// 지정된 타겟을 부드럽게 추적하는 카메라 스크립트
/// </summary>
public class Camera : MonoBehaviour
{
    public Transform target;    // 카메라가 따라갈 대상 (플레이어 등)

    public float smoothSpeed = 5f;  // 카메라가 타겟에 도달하는 속도 (값이 클수록 민첩하게 반응)

    public Vector3 offset = new Vector3(0f, 0, -10f);   // 타겟으로부터 떨어져 있을 거리 (Z축 -10은 2D게임 기본값)
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 타겟이 할당되어 있는지 확인
        if (target != null)
        {   
            // 1. 카메라가 최종적으로 도달해야 할 목표 위치 계산
            Vector3 desiredPosition = target.position + offset;
            // 2. 현재 위치에서 목표 위치까지 smoothSpeed의 비율로 부드럽게 이동(선형보간)
            // Vector3.Lerp(시작점,끝점,비율)
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            // 3. 계산된 부드러운 위치를 카메라에 적용
            transform.position = smoothedPosition;
        }
    }
}
