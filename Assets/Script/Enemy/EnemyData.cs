using UnityEngine;

[System.Serializable]
public class EnemyData
{
    [Header("ID, 타입, 이름, 정보"), Space(5f)]
    public int ID = 500000;
    public EnemyType type = 0;
    public string name = "몬스터 이름";
    public string info = "몬스터 정보";

    [Header("소환 주기, 최대 생명력, 방어력, 공격력, 공격속도, 이동속도"), Space(5f)]
    public float spawnCycleSecond = 0.5f;
    public float maxHealth = 100f;
    public float defense = 0f;
    public float attackDamage = 5f;
    public float attackSpeed = 0.5f;
    public float moveSpeed = 2f;

    [Header("마지막 공격 이후 재공격 가능 시간, 몸 크기 배율"), Space(5f)]
    public float attackDelay = 1f;
    public float bodyScale = 1f;

    [Header("성장 계수(배율) - 최대 생명력, 이동속도, 공격력"), Space(5f)]
    public float maxHealthRatio = 1.02f;
    public float moveSpeedRatio = 1.02f;
    public float attackDamageRatio = 1.02f;

    [Header("경험치, 경험치 증가량 계수, 아이템 드랍률 계수"), Space(5f)]
    public float expPoint = 25f;
    public float expRate = 1.02f;
    public float itemDropRate = 1.05f;

    [Header("레벨 - 현재, 최소, 최대"), Space(5f)]
    public int currentLevel = 1;
    public int minLevel = 1;
    public int maxLevel = 99;
}

public enum EnemyType
{
    Normal = 0,
    Tanker = 1,
    Faster = 2,
}