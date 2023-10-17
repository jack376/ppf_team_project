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
    private float flowTime = 0f;
    private Vector3 direction = Vector3.forward;

    private void Start()
    { 
        if (type == SkillType.Nova)
        {
            lifeTime = 0f;
        }

        direction = Vector3.forward;
    }

    private void Update()
    {
        flowTime += Time.deltaTime;
        if (flowTime >= lifeTime)
        {
            PlayHitParticle();
            ReleaseProjectile();
        }
    }

    private void FixedUpdate()
    {
        if (targetTransform != null)
        {
            direction = (targetTransform.position - transform.position).normalized;
        }

        ProjectileMovement();
        ProjectileHover();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((targetLayer & (1 << collision.gameObject.layer)) == 0)
        {
            return;
        }

        if (isPierceHitPlay == 1)
        {
            PlayFlashParticle();
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, splash, targetLayer);
        MultiTarget(hitColliders);

        if (type != SkillType.Pierce)
        {
            PlayHitParticle();
            ReleaseProjectile();
        }
    }

    private void OnEnable()
    {
        flowTime = 0f;
        if (flash != null)
        {
            PlayFlashParticle();
        }
    }

    private void OnDisable()
    {
        //PlayHitParticle();
    }

    public void ProjectileMovement()
    {
        if (targetTransform != null)
        {
            transform.LookAt(targetTransform.position);
        }
        else
        {
            PlayHitParticle();
            ReleaseProjectile();
        }

        transform.Translate(Vector3.forward * (speed * Time.fixedDeltaTime));
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
            GameObject hitInstance = PoolManager.Instance.GetPool(hitName).Get();
            hitInstance.transform.position = transform.position;

            Invoke("ReleaseHitParticle", hitInstance.GetComponent<ParticleSystem>().main.duration);
        }
    }

    public void PlayFlashParticle()
    {
        if (flash != null)
        {
            GameObject flashInstance = PoolManager.Instance.GetPool(flashName).Get();
            flashInstance.transform.position = transform.position;
            flashInstance.transform.forward = gameObject.transform.forward;

            Invoke("ReleaseFlashParticle", flashInstance.GetComponent<ParticleSystem>().main.duration);
        }
    }

    private void ReleaseHitParticle()
    {
        PoolManager.Instance.GetPool(hitName).Release(PoolManager.Instance.GetPool(hitName).Get());
    }

    private void ReleaseFlashParticle()
    {
        PoolManager.Instance.GetPool(flashName).Release(PoolManager.Instance.GetPool(flashName).Get());
    }

    private void ReleaseProjectile()
    {
        if (gameObject.activeInHierarchy)
        {
            PoolManager.Instance.GetPool(projectileName).Release(gameObject);
        }
    }
}
