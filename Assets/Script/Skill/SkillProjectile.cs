using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SkillProjectile : MonoBehaviour
{
    internal LayerMask targetLayer;
    internal int pierceCount = 1;

    internal float speed    = 15f;
    internal float splash   = 2f;
    internal float damage   = 100f;
    internal float lifeTime = 1f;

    private void Start()
    {
        //Destroy(gameObject, lifeTime);
        StartCoroutine(Release());
    }

    private void FixedUpdate()
    {
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
            //Destroy(gameObject);
            PoolManager.Instance.GetPool(gameObject).Release(gameObject);
            return;
        }

        var hitColliders = Physics.OverlapSphere(transform.position, splash, targetLayer);
        foreach (Collider hitCollider in hitColliders)
        {
            IDamageable target = hitCollider.gameObject.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }

    private IEnumerator Release()
    {
        yield return new WaitForSeconds(lifeTime);
        PoolManager.Instance.GetPool(gameObject).Release(gameObject);
    }
}