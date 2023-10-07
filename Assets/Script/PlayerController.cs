using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;

    public float moveSpeed = 5.0f;
    public float rotateSpeed = 5.0f;

    private Rigidbody playerRigidbody;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 inputVector  = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);
        Vector3 moveVelocity = inputVector.normalized * moveSpeed;
        Vector3 newPosition  = playerRigidbody.position + moveVelocity * Time.fixedDeltaTime;

        playerRigidbody.MovePosition(newPosition);

        if (inputVector != Vector3.zero)
        {
            playerRigidbody.rotation = Quaternion.LookRotation(inputVector);
        }
    }
}
