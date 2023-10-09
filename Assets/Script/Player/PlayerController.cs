using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("조이스틱 (HUD 캔버스에 있음)"), Space(5f)]
    public Joystick joystick;

    [Header("플레이어 이동속도, 회전속도"), Space(5f)]
    public float moveSpeed = 5.0f;
    public float rotateSpeed = 5.0f;

    // 리지드바디, 애니메이터 컴포넌트 필드
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    // 플레이어 조이스틱 이동 로직 및 무브 애니메이션 재생, 게임 오버 시 이동 불가
    void FixedUpdate()
    {
        if (GameManager.isGameover) 
        {
            return;
        }

        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 inputVector  = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 moveVelocity = inputVector * moveSpeed;
        Vector3 newPosition  = playerRigidbody.position + moveVelocity * Time.fixedDeltaTime;

        playerRigidbody.MovePosition(newPosition);

        if (inputVector != Vector3.zero)
        {
            playerRigidbody.rotation = Quaternion.LookRotation(inputVector);
        }

        playerAnimator.SetFloat("Move", inputVector.magnitude);
    }
}
