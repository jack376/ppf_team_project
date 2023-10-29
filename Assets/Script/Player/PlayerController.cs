using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;

    public float moveSpeed = 5.0f;
    public float rotateSpeed = 5.0f;

    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator  = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (GameManager.isGameover) 
        {
            return;
        }

        float horizontal = joystick.Horizontal;
        float vertical   = joystick.Vertical;

        var inputVector  = new Vector3(horizontal, 0f, vertical).normalized;
        var moveVelocity = inputVector * moveSpeed;
        var newPosition  = playerRigidbody.position + moveVelocity * Time.fixedDeltaTime;

        playerRigidbody.MovePosition(newPosition);

        if (inputVector != Vector3.zero)
        {
            playerRigidbody.rotation = Quaternion.LookRotation(inputVector);
        }

        playerAnimator.SetFloat("Move", inputVector.magnitude);
    }
}
