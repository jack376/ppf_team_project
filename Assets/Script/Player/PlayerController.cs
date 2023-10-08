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
        playerAnimator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
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
