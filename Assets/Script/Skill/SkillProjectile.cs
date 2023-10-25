using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    internal GameObject originalPrefab;

    internal LayerMask targetLayer;
    internal int pierceCount = 1;

    internal float speed    = 15f;
    internal float splash   = 2f;
    internal float damage   = 100f;
    internal float lifeTime = 1f;

    private float flowTime = 0f;

    private void OnEnable()
    {
        flowTime = 0f;
    }

    private void Update()
    {
        flowTime += Time.deltaTime;
        if(flowTime >= lifeTime)
        {
            PoolManager.Instance.GetPool(originalPrefab).Release(gameObject);
            return;
        }

        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((targetLayer & 1 << collision.gameObject.layer) == 0)
        {
            return;
        }

        pierceCount--;
        if (pierceCount <= 0)
        {
            PoolManager.Instance.GetPool(originalPrefab).Release(gameObject);
            return;
        }
        
        var hitColliders = Physics.OverlapSphere(transform.position, splash, targetLayer);
        foreach (Collider hitCollider in hitColliders)
        {
            var target = hitCollider.gameObject.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}