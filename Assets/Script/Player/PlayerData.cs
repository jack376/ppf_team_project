using System;
using System.Collections.Generic;
using UnityEditor;
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

    [Header("기본 최대 체력, 방어력, 이동속도")]
    public float maxHealth = 100f;
    public float armorPoint = 0f;
    public float moveSpeed = 5.0f;

    [Header("기본 스킬 공격력 강화 계수, 스킬 가속 계수")]
    public float skillDamageEnhance = 1f;
    public float cooldownReduction = 1f;

    internal int currentSkillID;
    internal float fianlMaxHealth;
    internal float finalArmorPoint;
    internal float finalMoveSpeed;
    internal float finalSkillDamageEnhance;
    internal float finalCooldownReduction;

    internal event Action<float> OnSkillDamageEnhance;
    internal event Action<float> OnCooldownReduction;

    private Dictionary<int, PlayerBuffData> buffs = new Dictionary<int, PlayerBuffData>();
    private List<int> timeOutBuffs = new List<int>();

    private void Awake()
    {
        currentSkillID = baseSkillID;

        fianlMaxHealth          = maxHealth;
        finalArmorPoint         = armorPoint;
        finalMoveSpeed          = moveSpeed;
        finalSkillDamageEnhance = skillDamageEnhance;
        finalCooldownReduction  = cooldownReduction;
    }

    private void Update()
    {
        if (GameManager.isGameover)
        {
            return;
        }

        timeOutBuffs.Clear();
        foreach (var buff in buffs)
        {
            if (buff.Value.buffStartTime + buff.Value.buffDuration < Time.time)
            {
                timeOutBuffs.Add(buff.Key);
            }
        }

        foreach (int buffID in timeOutBuffs)
        {
            RemoveBuff(buffID);
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            var playerBuffData = new PlayerBuffData();
            playerBuffData.buffID = 1;
            playerBuffData.buffStartTime = Time.time;
            playerBuffData.buffDuration = 5f;
            playerBuffData.buffMaxHealth = 1000f;
            playerBuffData.buffArmorPoint = 10f;
            playerBuffData.buffMoveSpeed = 25f;
            playerBuffData.buffSkillDamageEnhance = 2f;
            playerBuffData.buffCooldownReduction = 1f;

            AddBuff(playerBuffData);
        }
    }

    public void AddBuff(PlayerBuffData playerBuffData)
    {
        buffs[playerBuffData.buffID] = playerBuffData;
        ApplyBuffStats();
    }

    public void RemoveBuff(int buffID)
    {
        if (buffs.ContainsKey(buffID))
        {
            buffs.Remove(buffID);
            ApplyBuffStats();
        }
    }

    private void ApplyBuffStats()
    {
        fianlMaxHealth          = maxHealth;
        finalArmorPoint         = armorPoint;
        finalMoveSpeed          = moveSpeed;
        finalSkillDamageEnhance = skillDamageEnhance;
        finalCooldownReduction  = cooldownReduction;

        foreach (var buff in buffs)
        {
            fianlMaxHealth          += buff.Value.buffMaxHealth;
            finalArmorPoint         += buff.Value.buffArmorPoint;
            finalMoveSpeed          += buff.Value.buffMoveSpeed;
            finalSkillDamageEnhance += buff.Value.buffSkillDamageEnhance;
            finalCooldownReduction  += buff.Value.buffCooldownReduction;
        }

        if (OnSkillDamageEnhance != null)
        {
            OnSkillDamageEnhance(finalSkillDamageEnhance);
        }

        if (OnCooldownReduction != null)
        {
            OnCooldownReduction(finalCooldownReduction);
        }
    }
}