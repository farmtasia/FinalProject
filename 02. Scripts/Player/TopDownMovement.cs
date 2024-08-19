using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    // TopDownController로부터 전달된 이동 방향에 따라 실제 물리 이동을 처리

    [SerializeField] private VirtualJoystick joystick;

    [SerializeField] private float moveSpeed; // 5f 이동속도
    [SerializeField] private float saveSpeed;
    public bool isMoving = true;

    [SerializeField] private SpriteRenderer spriteRenderer;
    private TopDownController controller;
    private Rigidbody2D movementRigidbody;

    private Vector2 movementDirection = Vector2.zero;
    private Vector2 lastMovementDirection = Vector2.zero; // 최근 이동 방향 저장

    //[SerializeField] private SEType footstepSoundEffect = SEType.FOOTSTEPS; // 이동 시 효과음
    //private float footstepInterval = 0.5f; // 발걸음 소리 간격 (초 단위)
    //private float footstepTimer = 0f;

    private void Awake()
    {
        controller = GetComponent<TopDownController>();
        movementRigidbody = GetComponent<Rigidbody2D>();
        isMoving = true;
    }

    private void Start()
    {
        controller.OnMoveEvent += Move; // TopDownController의 OnMoveEvent에 Move()메서드 등록
    }

    private void Move(Vector2 direction)
    {
        // 이동방향만 정해두고 실제로 움직이지는 않음.
        // 움직이는 것은 물리 업데이트에서 진행(rigidbody가 물리니까)
        //movementDirection = direction;
        //isMoving = direction != Vector2.zero;   // 움직인다면 isMoving은 true, 안 움직인다면 false


        if (!isMoving)
        {
            // 움직임이 비활성화된 상태라면 움직임을 멈춤
            direction = Vector2.zero;
        }

        movementDirection = direction;

        if (direction != Vector2.zero)
        {
            lastMovementDirection = direction; // 마지막 이동 방향 업데이트
        }
    }

    private void FixedUpdate() // 물리 업데이트에서 움직임 적용
    {
        ApplyMovement(movementDirection);
    }

    private void ApplyMovement(Vector2 direction)
    {
        direction = direction * moveSpeed;

        movementRigidbody.velocity = direction;

        if (isMoving) // 움직임 방향에 맞춰서 캐릭터 이미지 좌우반전, 발걸음소리 재생
        {
            //footstepTimer -= Time.fixedDeltaTime;
            //if (footstepTimer <= 0f)
            //{
            //    SoundManager.Instance.PlayEffect(footstepSoundEffect);
            //    footstepTimer = footstepInterval; // 타이머 리셋
            //}

            if (direction.x < 0)
                spriteRenderer.flipX = true; // 왼쪽으로 가면 뒤집음
            else if (direction.x > 0)
                spriteRenderer.flipX = false; // 오른쪽으로 가면 원상태
        }
        //else
        //{
        //    footstepTimer = 0f; // 멈췄을 때 타이머 리셋
        //}
    }

    public void HoldOnMoveSpeed(float newSpeed)
    {
        if (moveSpeed == newSpeed) return;

        if (moveSpeed != saveSpeed)
        {
            saveSpeed = moveSpeed;
        }
        moveSpeed = newSpeed;
    }

    public void ResetMoveSpeed()
    {
        moveSpeed = saveSpeed;
    }

    public Vector2 GetMovementDirection() // 플레이어 이동 방향 반환
    {
        return movementDirection != Vector2.zero ? movementDirection : lastMovementDirection; // 멈췄을 때 마지막 방향 반환
    }
}
