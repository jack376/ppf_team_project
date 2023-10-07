using UnityEngine;

public class SkillBehavior : MonoBehaviour
{
    public SkillData skillData;
    public LayerMask targetLayer;
    public LayerMask groundLayer;

    public GameObject hit;
    public GameObject flash;
    public GameObject[] Detached;

    private Vector3 startRadius = Vector3.zero;
    private Rigidbody rigidBody;
    private float hoverHeight = 1f;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        float modifyScale = skillData.size;
        transform.localScale = new Vector3(modifyScale, modifyScale, modifyScale);

        if (flash != null)
        {
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;

            var flashParticle = flashInstance.GetComponent<ParticleSystem>();

            Destroy(flashInstance, flashParticle.main.duration);
        }
        Destroy(gameObject, skillData.lifeTime);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * (skillData.speed * Time.fixedDeltaTime));

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 fixPosition = hit.point;
            fixPosition.y += hoverHeight;
            transform.position = fixPosition;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
        if ((targetLayer & (1 << collision.gameObject.layer)) == 0)
        {
            return;
        }
        Debug.Log("Particle On");

        var hitInstance = Instantiate(hit, collision.contacts[0].point, Quaternion.identity);
        var hitParticle = hitInstance.GetComponent<ParticleSystem>();
        Destroy(hitInstance, hitParticle.main.duration);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        var hitInstance = Instantiate(hit, transform.position, Quaternion.identity);
        var hitParticle = hitInstance.GetComponent<ParticleSystem>();
        Destroy(hitInstance, hitParticle.main.duration);
    }
}