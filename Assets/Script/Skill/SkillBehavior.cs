using UnityEngine;

public class SkillBehavior : MonoBehaviour
{
    [Header("스킬 데이터"), Space(5f)]
    public SkillData skillData;

    [Header("추적 대상 레이어 Enemy, Ground"), Space(5f)]
    public LayerMask targetLayer;
    public LayerMask groundLayer;

    [Header("스킬 추적 범위 (현재 최대치)"), Space(5f)]
    public float searchRadius = Mathf.Infinity;

    [Header("스킬 피격 이펙트"), Space(5f)]
    public GameObject hit;

    [Header("스킬 발사 이펙트"), Space(5f)]
    public GameObject flash;

    [Header("스킬 트레일(꼬리) 이펙트"), Space(5f)]
    public GameObject[] Detached;

    private Vector3 targetDirection;
    private float hoverHeight = 1f;

    private void Awake()
    {
        ProjectileDirection();
    }

    // 발사체가 발사될 때 플래시 파티클 재생 
    private void Start()
    { 
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

    // 발사체가 타겟 레이어에게 충돌 시 데미지 적용 및 히트 파티클 재생
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

    // 발사체가 사라질 때 히트 파티클 재생
    private void OnDestroy()
    {
        var hitInstance = Instantiate(hit, transform.position, Quaternion.identity);
        var hitParticle = hitInstance.GetComponent<ParticleSystem>();
        Destroy(hitInstance, hitParticle.main.duration);
    }
    
    // 발사체 시작 방향
    public void ProjectileDirection()
    {
        if(targetDirection != null)
        {
            targetDirection = (FindNearTarget().position - transform.position).normalized;
        }
        else
        {
            targetDirection = Vector3.forward;
        }
    }

    // 발사체 실시간 움직임
    public void ProjectileMovement()
    {
        transform.Translate(Vector3.forward * (skillData.speed * Time.fixedDeltaTime));
    }

    // 발사체 실시간 방향
    public void ProjectileLookRotation()
    {
        transform.rotation = Quaternion.LookRotation(targetDirection);
    }

    // 발사체가 땅에 부딪히지 않도록 y축으로 1f만큼 공중 부양 
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

    // 리스트 내에서 가장 가까운 타겟 검사
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