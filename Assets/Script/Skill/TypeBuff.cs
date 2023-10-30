using UnityEngine;

public class TypeBuff : ISkill
{
    public void Execute(SkillData skillData, LayerMask targetLayer, Quaternion targetQuaternion, GameObject hitFx, GameObject projectileFx, Vector3 targetPosition)
    {
        ApplyBuff(skillData);
    }

    private void ApplyBuff(SkillData skillData)
    {
        float duration = skillData.buffDuration;

        var playerData = GameManager.player.GetComponent<PlayerData>();
        var playerExp  = GameManager.player.GetComponent<PlayerExp>();

        playerData.skillDamageEnhance += skillData.buffAddDamage;
        playerData.cooldownReduction  += skillData.buffAddAttackSpeed;
        playerData.moveSpeed          += skillData.buffAddMoveSpeed;
        playerExp.expRatio            += skillData.buffAddExp;
    }

    /*
    private void RemoveBuff()
    {


        // 버프 지속 시간을 설정하고, 버프 활성화 여부를 true로 설정합니다.
        buffDuration = skillData.duration;
        isBuffActive = true;

        // 플레이어의 공격력, 이동 속도, 경험치, 루팅 범위를 원래대로 돌려놓습니다.
        GameManager.player.attackPower -= addDamage;
        GameManager.player.moveSpeed -= addMoveSpeed;
        GameManager.player.expRate -= addExp;
        GameManager.player.lootRange -= addLootRange;

        // 버프 지속 시간을 초기화하고, 버프 활성화 여부를 false로 설정합니다.
        buffDuration = 0f;
        isBuffActive = false;
    }
    */
}