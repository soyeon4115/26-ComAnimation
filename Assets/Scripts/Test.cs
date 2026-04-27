
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Test : MonoBehaviour { 

    // 물리 및 점프 설정
    float jumpForce = 5.0f;         // 점프 시 위로 솟구치는 힘
    float gravity = 9.8f;           // 하락하는 중력의 세기
    float groundY = 0.0f;           // 착지할 땅의 높이
    bool isJumping = false;         // 현재 점프 중인지 여부
    float verticalVelocity = 0f;    // 수직 속도 (상승/하강 계산용)
    bool groundYSet = false;        // 시작 높이가 설정되었는지 체크

    // 보이지 않는 벽 (X 범위 제한)
    const float minX = -2f; // 왼쪽 이동 한계점
    const float maxX = 2f;  // 오른쪽 이동 한계점

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 트랜스폼의 기본 정보들을 콘솔에 출력 (디버깅용)
        Debug.Log("Positin: " + transform.position);
        Debug.Log("Rotation (Quaternion): " + transform.rotation);
        Debug.Log("Euler Angles: " + transform.eulerAngles);
        Debug.Log("Forward: " + transform.forward);
        Debug.Log("Up: " + transform.up);
        Debug.Log("Right: " + transform.right);
    }

    // Update is called once per frame
    void Update()
    {        
        float moveSpeed = 5f;       // 이동 속도
        float rotateSpeed = 100f;   // 회전 속도

        // 오른쪽 화살표 입력 처리
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // 월드 좌표계 기준 오른쪽으로 이동
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.World);
        }

        // 왼쪽 화살표 입력 처리
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // 월드 좌표계 기준 왼쪽으로 이동
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
        }

        // R키 입력 처리 (회전)
        if (Input.GetKey(KeyCode.R))
        {
            // Y축을 기준으로 월드 좌표계에서 회전
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
        }
        
        
        // 점프 시작 위치 기록 (최초 1회)
        if (!groundYSet)
        {
            groundY = transform.position.y; // 게임 시작 시점의 y축 높이를 지면으로 저장
            groundYSet = true;
        }

        // 스페이스바를 누르면 점프
        // 스페이스 바를 누르고, 현재 점프 상태가 아닐 때만 실행
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            verticalVelocity = jumpForce;   // 수직 속도를 점프 힘만큼 설정
        }

        // 점프 중이면 물리 적용
        if (isJumping)
        {  
            // 속도에서 중력을 지속적으로 차감 (가속도)
            verticalVelocity -= gravity * Time.deltaTime;
            Vector3 pos = transform.position;
            pos.y += verticalVelocity * Time.deltaTime; // 계산된 속도를 위치에 반영

            // 땅 이하로 내려가면 정지
            if (pos.y <= groundY)
            {
                pos.y = groundY;        // 위치를 지면에 고정
                isJumping = false;      // 점프 상태 해제
                verticalVelocity = 0f;  // 속도 초기화
            }
            transform.position = pos;
        }
        
        // X -2 ~ 2 범위 밖으로 나가지 못하도록 제한 (if 처리)
        Vector3 xpos = transform.position;
        // if문을 사용하여 위치가 제한 범위를 벗어나면 강제로 경계값에 고정
        if (xpos.x < minX)
            xpos.x = minX;
        if (xpos.x > maxX)
            xpos.x = maxX;
        transform.position = xpos;
    }
}
