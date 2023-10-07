using UnityEngine;

public class SkillBehavior : MonoBehaviour
{
    public SkillData skillData;
    public LayerMask targetLayer;

    public GameObject hit;
    public GameObject flash;
    public GameObject[] Detached;

    public Vector3 startRadius = Vector3.zero;
    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

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
        transform.position += transform.forward * (skillData.projectileSpeed * Time.fixedDeltaTime);
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