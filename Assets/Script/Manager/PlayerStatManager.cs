using UnityEngine;

[System.Serializable]
public class PlayerStat
{
    [Header("클래스 타입, 정보"), Space(5f)]
    public ClassType type = 0;
    public string info = "직업 정보";

    [Header("최대 생명력, 방어력, 공격력"), Space(5f)]
    public float maxHealth = 100f;
    public float defense = 0f;
    public float attackDamage = 100f;

    [Header("공격속도, 공격간격, 이동속도"), Space(5f)]
    public float attackSpeed = 1f;
    public float attackInterval = 0.5f;
    public float moveSpeed = 15f;
}

public enum ClassType
{
    Archer = 0,
    Wizard = 1,
    Warrior = 2,
}

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager Instance { get; private set; }
    public PlayerStat playerStat;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public PlayerStat GetPlayerStatData()
    {





        return playerStat;
    }
}
