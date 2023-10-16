using System.Collections;
using UnityEngine;

public class SkillBehavior : MonoBehaviour
{
    public SkillData skillData;

    public GameObject projectile;
    public GameObject hit;
    public GameObject flash;
    internal GameObject[] Detached;

    public LayerMask targetLayer;
    public LayerMask groundLayer;

    private float searchRadius = 1500f;
    private Transform targetTransform;
    private Quaternion targetQuaternion;

    private void Start()
    {
        SetTarget();

        if (skillData.skillType == SkillType.Nova)
        {
            CreateProjectile();
            NovaTarget();
            return;
        }

        switch (skillData.shotType)
        {
            case ShotType.Burst:
                StartCoroutine(CreateProjectileBurst());
                break;
            case ShotType.Multi:
                CreateProjectileMulti();
                break;
            default:
                CreateProjectile();
                break;
        }
    }

    private void SetTarget()
    {
        FindNearTarget();

        if (targetTransform != null)
        {
            targetQuaternion = Quaternion.LookRotation(targetTransform.position - transform.position);
        }
    }

    private void FindNearTarget() // 리스트 내에서 가장 가까운 타겟 검사
    {
        float nearDistance = searchRadius;

        foreach (Enemy enemy in EnemySpawner.currentEnemies)
        {
            if (enemy == null)
            {
                continue;
            }

            float distanceToTarget = (enemy.transform.position - transform.position).sqrMagnitude;
            if (distanceToTarget < nearDistance)
            {
                nearDistance = distanceToTarget;
                targetTransform = enemy.transform;
            }
        }
    }

    public void CreateProjectile()
    {
        GameObject skill = Instantiate(projectile, GameManager.weapon.transform.position, targetQuaternion);
        SkillProjectile skillProjectile = skill.GetComponent<SkillProjectile>();

        skillProjectile.targetTransform = targetTransform;
        skillProjectile.type = skillData.skillType;

        skillProjectile.hit = hit;
        skillProjectile.flash = flash;
        skillProjectile.Detached = Detached;

        skillProjectile.isPierceHitPlay = skillData.isPierceHitPlay;

        skillProjectile.targetLayer = targetLayer;
        skillProjectile.groundLayer = groundLayer;

        skillProjectile.speed = skillData.speed;
        skillProjectile.splash = skillData.splash;
        skillProjectile.damage = skillData.damage;
        skillProjectile.lifeTime = skillData.lifeTime;
    }

    private IEnumerator CreateProjectileBurst()
    {
        float burstInterval = skillData.shotTypeValue * 0.1f;
        for (int i = 0; i < skillData.count; i++)
        {
            CreateProjectile();
            yield return new WaitForSeconds(burstInterval);
        }
    }

    private void CreateProjectileMulti()
    {
        Quaternion originalRotation = targetQuaternion;

        float totalAngle = skillData.shotTypeValue;
        float deltaAngle = totalAngle / (skillData.count - 1);
        float startAngle = -totalAngle / 2;

        for (int i = 0; i < skillData.count; i++)
        {
            float currentAngle = startAngle + (deltaAngle * i);
            targetQuaternion = originalRotation * Quaternion.Euler(0f, currentAngle, 0f);
            CreateProjectile();
        }
        targetQuaternion = originalRotation;
    }

    public void NovaTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(GameManager.weapon.transform.position, skillData.splash, targetLayer);
        foreach (Collider collision in hitColliders)
        {
            IDamageable target = collision.gameObject.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(skillData.damage);
            }
        }
    }

    public void UpdateSkillData(SkillData newSkillData)
    {
        skillData = newSkillData;
    }
}