using UnityEngine;

public class TypeNova : MonoBehaviour, ISkill
{
    public void Execute(SkillData skillData, LayerMask targetLayer, Quaternion targetQuaternion)
    {
        var go = Instantiate(SkillManager.Instance.projectilePrefab, GameManager.player.transform.position, targetQuaternion);
        var sp = go.GetComponent<SkillProjectile>();

        sp.targetLayer = targetLayer;
        sp.speed = skillData.speed;
        sp.splash = skillData.splash;
        sp.damage = skillData.damage;
        sp.lifeTime = skillData.lifeTime;
    }
}