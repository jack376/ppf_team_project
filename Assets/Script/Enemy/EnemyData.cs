[System.Serializable]
public class EnemyData
{
    public int ID = 500000;
    public EnemyType type = 0;
    public string name = "몬스터 이름";
    public string info = "몬스터 정보";

    public float spawnCycleSecond = 0.5f;
    public float maxHealth = 100f;
    public float defense = 0f;
    public float bodyScale = 1f;
    public float moveSpeed = 2f;
    public float attackDamage = 5f;
    public float attackSpeed = 0.5f;
    public float attackDelay = 1f;
    public float maxHealthRatio = 1.02f;
    public float moveSpeedRatio = 1.02f;
    public float attackDamageRatio = 1.02f;

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