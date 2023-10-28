using UnityEngine;

public class TypeMulti : ISkill
{
    public void Execute(SkillData skillData, LayerMask targetLayer, Quaternion targetQuaternion, GameObject hitFx, GameObject projectileFx, Vector3 targetPosition)
    {
        var originalRotation = targetQuaternion;

        float totalAngle = skillData.multiShotAngle;
        float deltaAngle = totalAngle / (skillData.multiShotBulletCount - 1);
        float startAngle = -totalAngle / 2;

        for (int i = 0; i < skillData.multiShotBulletCount; i++)
        {
            float currentAngle = startAngle + (deltaAngle * i);
            SpawnProjectile(skillData, targetLayer, originalRotation * Quaternion.Euler(0f, currentAngle, 0f), hitFx, projectileFx);
        }
    }

    private void SpawnProjectile(SkillData skillData, LayerMask targetLayer, Quaternion targetQuaternion, GameObject hitFx, GameObject projectileFx)
    {
        var projectile = PoolManager.Instance.GetPool(SkillManager.Instance.projectilePrefab).Get();
        projectile.transform.rotation = targetQuaternion;

        var sp = projectile.GetComponent<SkillProjectile>();
        sp.originalPrefab     = SkillManager.Instance.projectilePrefab;
        sp.projectileFxPrefab = projectileFx;
        sp.hitFxPrefab        = hitFx;
        sp.targetLayer        = targetLayer;
        sp.pierceCount        = skillData.pierceCount;
        sp.speed              = skillData.speed;
        sp.splash             = skillData.splash;
        sp.damage             = skillData.damage;
        sp.lifeTime           = skillData.lifeTime;

        var projectileParticle = PoolManager.Instance.GetPool(projectileFx).Get();
        projectileParticle.transform.SetParent(projectile.transform);
        projectileParticle.transform.localPosition = Vector3.zero;
        projectileParticle.transform.localRotation = Quaternion.identity;
    }
}