using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 5f;    // 캐릭터의 이동 속도

    [Header("sprites")]
    public Sprite idleSprite;           // 기본 서 있는 모습
    public Sprite jumpSprite;           // 제자리 점프 모습
    public Sprite jumpForwardSprite;    // 앞(오른쪽)으로 점프하는 모습

    [Header("Jump Settings")]
    public float jumpHeight = 2f;       // 점프의 최대 높이
    public float jumpDuration = 0.5f;   // 점프가 유지되는 시간 (초)

    private SpriteRenderer spriteRenderer;
    private bool isJumping = false;     // 현재 점프 중인지 체크
    private float jumpTimer = 0f;       // 점프 경과 시간 측정용
    private Vector3 startPosition;      // 점프를 시작한 지점의 위치

    private bool isMovingRight = false; // 현재 오른쪽으로 이동 중인지 체크

    /// <summary>
    /// 점프를 시작할 때 호출되는 함수
    /// </summary>
    void StartJump()
    {
        isJumping = true;
        jumpTimer = 0f;
        startPosition = transform.position;     // 점프 시작 높이를 기억
        
        if (spriteRenderer != null)
        {
            // 오른쪽 이동 중이면서 전용 스프라이트가 있다면 해당 이미지로 교체
            if(isMovingRight && jumpForwardSprite != null)
            {
                spriteRenderer.sprite = jumpForwardSprite;
            }
            // 그 외에는 일반 점프 이미지 사용
            else if (jumpSprite != null)
            {
                spriteRenderer.sprite = jumpSprite;
            }
        }
        
    }
    /// <summary>
    /// 점프 도중 매 프레임 위치와 상태를 갱신하는 함수
    /// </summary>
    void UpdateJump()
    {
        jumpTimer += Time.deltaTime;
        float progress = jumpTimer / jumpDuration;      // 점프 진행률 (0.0 ~ 1.0)

        if (progress >= 1f)     // 점프 시간이 다 되었을 때
        {
            // 정확히 시작 높이로 복구하고 점프 종료
            transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
            isJumping = false;

            // 다시 기본 상태 이미지로 복구
            if(spriteRenderer != null && idleSprite != null)
            {
                spriteRenderer.sprite = idleSprite;
            }
        }
        else
        {   
            // 사인 곡선을 이용한 부드러운 상하 이동 계산 
            float height = Mathf.Sin(progress * Mathf.PI) * jumpHeight;
            transform.position = new Vector3(transform.position.x, startPosition.y + height, transform.position.z);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 시작 시 기본 이미지 설정
        if (spriteRenderer != null && idleSprite != null )
        {
            spriteRenderer.sprite = idleSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveDirection = Vector2.zero;

        // 상하좌우 입력 처리
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            moveDirection.x -= 1f;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            isMovingRight = true;   // 오른쪽 이동 상태 기록
            moveDirection.x += 1f;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            moveDirection.y += 1f;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            moveDirection.y -= 1f;
        }

        // 대각선 이동 시 속도가 빨라지지 않도록 규정화
        moveDirection = moveDirection.normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // 점프 로직 실행
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartJump();
        }
        if (isJumping)
        {
            UpdateJump();
        }

        // 매 프레임 마지막에 오른쪽 이동 체크 변수 초기화
        isMovingRight = false;
    }
}
