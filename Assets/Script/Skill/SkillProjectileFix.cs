using UnityEngine;

public class SkillProjectileFix : MonoBehaviour
{
    internal LayerMask targetLayer;

    internal float speed = 15f;
    internal float splash = 2f;
    internal float damage = 100f;
    internal float lifeTime = 1f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((targetLayer & (1 << collision.gameObject.layer)) == 0)
        {
            return;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, splash, targetLayer);
        foreach (Collider hitCollision in hitColliders)
        {
            IDamageable target = hitCollision.gameObject.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}