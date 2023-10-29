using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("기본 공격 스킬 프리팹 ID")]
    public int baseSkillID = 10000002;

    [Header("최대 체력, 방어력, 스킬 가속, 이동속도")]
    public float maxHealth = 100;
    public float armorPoint = 0;
    public float cooldownReduction = 1f;
    public float moveSpeed = 5.0f;
}