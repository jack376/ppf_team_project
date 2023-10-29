using UnityEngine;

public class TypeBuff : ISkill
{
    public void Execute(SkillData skillData, LayerMask targetLayer, Quaternion targetQuaternion, GameObject hitFx, GameObject projectileFx, Vector3 targetPosition)
    {
        ApplyBuff(skillData);
    }

    private void ApplyBuff(SkillData skillData)
    {



    }
}