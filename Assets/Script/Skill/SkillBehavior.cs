using System.Collections;
using UnityEngine;

public class SkillBehavior : MonoBehaviour
{
    public SkillData skillData;
    public GameObject skillProjectilePrefab;

    public float searchRadius = 1000f;

    public LayerMask targetLayer;
    public LayerMask groundLayer;

    private Transform targetTransform;
    private Quaternion targetQuaternion;

    private void Start()
    {
        targetTransform = FindNearTarget();
        targetQuaternion = Quaternion.LookRotation(targetTransform.position - transform.position);

        if (skillData.count > 1)
        {
            if (skillData.shotType == ShotType.Burstshot)
            {
                StartCoroutine(CreateProjectileBurst());
            }
            else if (skillData.shotType == ShotType.Multishot)
            {
                CreateProjectileMulti();
            }
        }
        else
        {
            CreateProjectile();
        }
    }

    public Transform FindNearTarget() // 리스트 내에서 가장 가까운 타겟 검사
    {
        Transform neartarget = null;
        float nearDistance = searchRadius;

        foreach (Enemy enemy in EnemySpawner.currentEnemies)
        {
            if (enemy == null)
            {
                continue; // 널 체크로 땜빵, 널 발생 시 엄청난 렉 발생 중임
            }

            float distanceToTarget = (enemy.transform.position - transform.position).sqrMagnitude;

            if (distanceToTarget < nearDistance)
            {
                nearDistance = distanceToTarget;
                neartarget = enemy.transform;
            }
        }

        return neartarget;
    }

    public void CreateProjectile()
    {
        var skill = Instantiate(skillProjectilePrefab, GameManager.weapon.transform.position, targetQuaternion);
        var skillProjectile = skill.GetComponent<SkillProjectile>();

        skillProjectile.type = skillData.skillType;
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
        for (int i = 0; i < skillData.count; i++)
        {
            CreateProjectile();
            yield return new WaitForSeconds(skillData.interval);
        }
    }

    private void CreateProjectileMulti()
    {
        Quaternion originalRotation = targetQuaternion;

        float totalSpread = skillData.angle;
        float deltaAngle = totalSpread / (skillData.count - 1);
        float startAngle = -totalSpread / 2;

        for (int i = 0; i < skillData.count; i++)
        {
            float currentAngle = startAngle + (deltaAngle * i);
            targetQuaternion = originalRotation * Quaternion.Euler(0f, currentAngle, 0f);
            CreateProjectile();
        }
        targetQuaternion = originalRotation;
    }
}