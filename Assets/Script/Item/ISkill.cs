using UnityEngine;

public interface ISkill
{
    void Execute(SkillData skillData, LayerMask targetLayer, Quaternion targetQuaternion, GameObject hitFx, GameObject projectileFx);
}