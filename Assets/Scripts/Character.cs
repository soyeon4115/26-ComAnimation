using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    // ^ 이동 파라미터
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isGrounded = true;
    // ^ 컴포넌트 관리

    // 애니메이션 컨트롤러
    public RuntimeAnimatorController idleController;
    public RuntimeAnimatorController runController;
    public RuntimeAnimatorController jumpController;
    void Movement()
    {
        float moveInput = 0f;

        // A키, D키 또는 방향키(왼쪽, 오른쪽)로 이동
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveInput = 1f;
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        // rigidbody2d의 속도 옵션을 적용한 자연스러운 움직임 구현

        // 캐릭터 방향 전환 (스프라이트플립)
        if (moveInput != 0 && spriteRenderer != null )
        {
            spriteRenderer.flipX = moveInput < 0;
        }

        // 바닥에 있을 때만 이동/정지 애니메이션 전환
        if (isGrounded)
        {
            if (moveInput != 0)
            {
                SetAnimatorController(runController);
            }
            else
            {
                SetAnimatorController(idleController);
            }
        }
    }
    void Jump()
    {
        // 스페이스 이벤트 입력
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;

            // Jump 컨트롤러로 변경
            SetAnimatorController(jumpController);
            // 애니메이션 변경 처리
        }
    }
    void SetAnimatorController(RuntimeAnimatorController controller)
    {
        if (animator != null && controller != null)
        {
            animator.runtimeAnimatorController = controller;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       // 충돌 감지
       // 충돌 대상이 타일맵이라면
       if (collision.gameObject.GetComponent<UnityEngine.Tilemaps.Tilemap>() != null)
        {
            isGrounded = true;

            // 착지 시 Idle 컨트롤러로 변경
            SetAnimatorController(idleController);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // 시작 시 기본 애니메이션으로 Idle 컨트롤러 설정
        SetAnimatorController(idleController);
    }

    // Update is called once per frame
    void Update()
    {
        // 매 프레임마다 이동/점프 적용
        Movement();
        Jump();
    }
}
