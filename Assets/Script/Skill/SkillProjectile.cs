using System;
using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    internal GameObject originalPrefab;
    internal GameObject projectileFxPrefab;
    internal GameObject hitFxPrefab;

    internal LayerMask targetLayer;
    internal int pierceCount = 1;

    internal float speed    = 15f;
    internal float splash   = 15f;
    internal float damage   = 100f;
    internal float lifeTime = 1f;

    private float flowTime = 0f;
    private bool isEnable = true;

    private void OnEnable()
    {
        flowTime = 0f;
        isEnable = true;
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        var holdPosition = transform.position;
        holdPosition.y = 1f;
        transform.position = holdPosition;

        flowTime += Time.deltaTime;
        if (flowTime >= lifeTime)
        {
            if (isEnable)
            {
                OnHitParticle(hitFxPrefab);
                OnSplashDamage();
                ReleaseProjectileFx();
                isEnable = false;

                return;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if ((targetLayer & 1 << collision.gameObject.layer) == 0)
        {
            return;
        }

        if (pierceCount > 1)
        {
            pierceCount--;
            OnHitParticle(hitFxPrefab);
            OnSplashDamage();
        }

        if (pierceCount <= 0 && isEnable)
        {
            OnHitParticle(hitFxPrefab);
            OnSplashDamage();
            ReleaseProjectileFx();
            isEnable = false;

            return;
        }
    }

    private void OnSplashDamage()
    {
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

    private void OnHitParticle(GameObject hitFxPrefab)
    {
        var hitParticleGo = PoolManager.Instance.GetPool(hitFxPrefab).Get();
        hitParticleGo.transform.position = transform.position;
        hitParticleGo.transform.rotation = Quaternion.identity;

        var hitParticle = hitParticleGo.GetComponent<HitParticleHandler>();
        hitParticle.onFinish += ReleaseParticle;

        void ReleaseParticle()
        {
            hitParticle.onFinish -= ReleaseParticle;
            PoolManager.Instance.GetPool(hitFxPrefab).Release(hitParticleGo);
        }
    }

    private void ReleaseProjectileFx()
    {
        PoolManager.Instance.GetPool(projectileFxPrefab).Release(gameObject.transform.GetChild(0).gameObject);
        gameObject.transform.GetChild(0).parent = null;

        PoolManager.Instance.GetPool(originalPrefab).Release(gameObject);
    }
}