using UnityEngine;

public class TypeNone : ISkill
{
    public void Execute(SkillData skillData, LayerMask targetLayer, Quaternion targetQuaternion, GameObject hitFx = null, GameObject projectileFx = null)
    {
        var projectile = PoolManager.Instance.GetPool(SkillManager.Instance.projectilePrefab).Get();
        projectile.transform.position = GameManager.player.transform.position;
        projectile.transform.rotation = targetQuaternion;

        var sp = projectile.GetComponent<SkillProjectile>();
        sp.originalPrefab     = SkillManager.Instance.projectilePrefab;
        sp.projectileFxPrefab = projectileFx;
        sp.hitFxPrefab        = hitFx;
        sp.targetLayer        = targetLayer;
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