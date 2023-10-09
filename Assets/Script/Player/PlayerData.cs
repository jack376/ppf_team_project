using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [Header("클래스 타입, 정보"), Space(5f)]
    public ClassType type = 0;
    public string info = "직업 정보";

    [Header("최대 생명력, 방어력, 공격력, 공격속도, 이동속도"), Space(5f)]
    public float maxHealth = 100f;
    public float defense = 0f;
    public float attackDamage = 100f;
    public float attackSpeed = 1f;
    public float moveSpeed = 15f;
}

public enum ClassType
{
    Archer = 0,
    Wizard = 1,
    Warrior = 2,
}