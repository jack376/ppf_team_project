using System;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerBuffData
{
    public int buffID;
    public float buffStartTime;
    public float buffDuration;
    public float buffMaxHealth;
    public float buffArmorPoint;
    public float buffMoveSpeed;
    public float buffSkillDamageEnhance;
    public float buffCooldownReduction;
}

public class PlayerData : MonoBehaviour
{
    [Header("기본 공격 스킬 프리팹 ID")]
    public int baseSkillID = 10000002;

    [Header("최대 체력, 방어력, 이동속도")]
    public float maxHealth = 100f;
    public float armorPoint = 0f;
    public float moveSpeed = 5.0f;

    [Header("스킬 공격력 강화, 스킬 가속")]
    public float skillDamageEnhance = 1f;
    public float cooldownReduction = 1f;

    private Dictionary<int, PlayerBuffData> buffs = new Dictionary<int, PlayerBuffData>();

    private void Update()
    {
        if(GameManager.isGameover)
        {
            return;
        }

        if(buffs.Count > 0)
        {
            foreach (var buff in buffs)
            {
                if (buff.Value.buffStartTime + buff.Value.buffDuration < Time.time)
                {
                    RemoveBuff(buff.Key);
                }
            }
        }
    }

    public void AddBuff(PlayerBuffData playerBuffData)
    {
        buffs.Add(playerBuffData.buffID, playerBuffData);
    }

    public void RemoveBuff(int buffID)
    {
        buffs.Remove(buffID);
    }

    private void ApplyBuffStats()
    {
        foreach (var buff in buffs)
        {
            var buffData = buff.Value;

            maxHealth          += buffData.buffMaxHealth;
            armorPoint         += buffData.buffArmorPoint;
            moveSpeed          += buffData.buffMoveSpeed;
            skillDamageEnhance += buffData.buffSkillDamageEnhance;
            cooldownReduction  += buffData.buffCooldownReduction;
        }
    }
}