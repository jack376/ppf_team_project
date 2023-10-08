using UnityEngine;

public class GroundLoop : MonoBehaviour
{
    public GameObject player;

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Loop"))
        {
            return;
        }

        Vector3 playerPosition = player.transform.position;
        Vector3 groundPosition = transform.position;

        Vector3 direction = (playerPosition - groundPosition).normalized;

        Vector3 moveDirection = Vector3.zero;

        float threshold = 0.666f;

        if (Mathf.Abs(direction.x) > threshold)
        {
            moveDirection += Vector3.right * Mathf.Sign(direction.x);
        }

        if (Mathf.Abs(direction.z) > threshold)
        {
            moveDirection += Vector3.forward * Mathf.Sign(direction.z);
        }

        transform.Translate(moveDirection * 160f);
    }
}
