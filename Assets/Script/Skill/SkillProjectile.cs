using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    internal GameObject hit;
    internal GameObject flash;
    internal GameObject[] Detached;

    internal SkillType type = 0;
    internal int isPierceHitPlay = 0;

    internal LayerMask targetLayer;
    internal LayerMask groundLayer;

    internal float speed = 5f;
    internal float splash = 1f;
    internal float damage = 10f;
    internal float lifeTime = 3f;

    internal Transform targetTransform;

    internal string projectileName;
    internal string hitName;
    internal string flashName;

    private float hoverHeight = 1f;
    private Vector3 direction = Vector3.forward;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        ProjectileMovement();
        ProjectileHover();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((targetLayer & (1 << collision.gameObject.layer)) == 0)
        {
            return;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, splash, targetLayer);
        MultiTarget(hitColliders);

        if (type != SkillType.Pierce)
        {
            Destroy(gameObject);
        }
        else
        {
            PlayFlashParticle();
        }
    }

    private void OnDestroy()
    {
        PlayHitParticle();
    }

    public void ProjectileMovement()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    public void ProjectileHover()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 fixPosition = transform.position;
            fixPosition.y = hit.point.y + hoverHeight;
            transform.position = fixPosition;
        }
    }

    public void MultiTarget(Collider[] hitColliders)
    {
        foreach (Collider collision in hitColliders)
        {
            IDamageable target = collision.gameObject.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }

    public void PlayHitParticle()
    {
        if (hit != null)
        {
            GameObject hitInstance = Instantiate(hit, transform.position, Quaternion.identity);
            hitInstance.transform.position = transform.position;

            Destroy(hitInstance, hitInstance.GetComponent<ParticleSystem>().main.duration);
        }
    }

    public void PlayFlashParticle()
    {
        if (flash != null)
        {
            GameObject flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.position = transform.position;
            flashInstance.transform.forward = gameObject.transform.forward;

            Destroy(flashInstance, flashInstance.GetComponent<ParticleSystem>().main.duration);
        }
    }
}
