using UnityEngine;

public class TypeNone : ISkill
{
    public void Execute(SkillData skillData, LayerMask targetLayer, Quaternion targetQuaternion)
    {
        var go = PoolManager.Instance.GetPool(SkillManager.Instance.projectilePrefab).Get();
        go.transform.position = GameManager.player.transform.position;
        go.transform.rotation = targetQuaternion;

        var sp = go.GetComponent<SkillProjectile>();
        sp.originalPrefab = SkillManager.Instance.projectilePrefab;
        sp.targetLayer    = targetLayer;
        sp.speed          = skillData.speed;
        sp.splash         = skillData.splash;
        sp.damage         = skillData.damage;
        sp.lifeTime       = skillData.lifeTime;
    }
}