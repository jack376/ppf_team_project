//using UnityEngine;

//public class PlayerController : MonoBehaviour
//{
//    public Joystick joystick;

//    private Rigidbody playerRigidbody;
//    private Animator playerAnimator;
//    private PlayerData playerData;

//    void Start()
//    {
//        playerRigidbody = GetComponent<Rigidbody>();
//        playerAnimator  = GetComponent<Animator>();
//        playerData      = GetComponent<PlayerData>();
//    }

//    void FixedUpdate()
//    {
//        if (GameManager.isGameover) 
//        {
//            return;
//        }

//        float horizontal = joystick.Horizontal;
//        float vertical   = joystick.Vertical;

//        var inputVector  = new Vector3(horizontal, 0f, vertical).normalized;
//        var moveVelocity = inputVector * playerData.finalMoveSpeed;
//        var newPosition  = playerRigidbody.position + moveVelocity * Time.fixedDeltaTime;

//        playerRigidbody.MovePosition(newPosition);

//        if (inputVector != Vector3.zero)
//        {
//            playerRigidbody.rotation = Quaternion.LookRotation(inputVector);
//        }

//        playerAnimator.SetFloat("Move", inputVector.magnitude);
//    }
//}

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;
    private PlayerData playerData;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator  = GetComponent<Animator>();
        playerData      = GetComponent<PlayerData>();
    }

    private void FixedUpdate()
    {
        if (GameManager.isGameover)
        {
            return;
        }

        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        var inputVector = new Vector3(horizontal, 0, vertical).normalized;
        var moveVelocity = inputVector * playerData.moveSpeed;

        playerRigidbody.velocity = new Vector3(moveVelocity.x, playerRigidbody.velocity.y, moveVelocity.z);

        if (inputVector != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(inputVector);
            playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, targetRotation, Time.fixedDeltaTime * 100f);
        }

        playerAnimator.SetFloat("Move", inputVector.magnitude);
    }
}

