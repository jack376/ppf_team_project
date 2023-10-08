using UnityEngine;

public class SkillBehavior : MonoBehaviour
{
    public SkillData skillData;
    public LayerMask targetLayer;
    public LayerMask groundLayer;

    public float searchRadius = 1000f;
    public Vector3 targetDirection;

    public GameObject hit;
    public GameObject flash;
    public GameObject[] Detached;

    private Rigidbody rigidBody;
    private float hoverHeight = 1f;

    private void Awake()
    {
        ProjectileDirection();
    }

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
        ProjectileMovement();
        ProjectileLookRotation();
        ProjectileHover();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((targetLayer & (1 << collision.gameObject.layer)) == 0)
        {
            return;
        }

        IDamageable target = collision.gameObject.GetComponent<IDamageable>();
        if (target != null)
        {
            target.OnDamage(skillData.damage, collision.contacts[0].point, collision.contacts[0].normal);
        }

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
    
    public void ProjectileDirection()
    {
        targetDirection = (FindNearTarget().position - transform.position).normalized;
    }

    public void ProjectileMovement()
    {
        transform.Translate(Vector3.forward * (skillData.speed * Time.fixedDeltaTime));
    }

    public void ProjectileLookRotation()
    {
        transform.rotation = Quaternion.LookRotation(targetDirection);
    }

    public void ProjectileHover()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 fixPosition = hit.point;
            fixPosition.y += hoverHeight;
            transform.position = fixPosition;
        }
    }

    public Transform FindNearTarget()
    {
        Transform neartarget = null;
        float nearDistance = Mathf.Infinity;

        foreach (Enemy enemy in EnemySpawner.currentEnemies)
        {
            float distanceToTarget = (enemy.transform.position - transform.position).sqrMagnitude;

            if (distanceToTarget < nearDistance)
            {
                nearDistance = distanceToTarget;
                neartarget = enemy.transform;
            }
        }

        return neartarget;
    }
}