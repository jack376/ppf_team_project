[System.Serializable]
public class SkillData
{
    public int ID;
    public SkillType type;
    public string name;
    public string info;

    public int projectileCount;

    public float projectileMinSize;
    public float projectileMaxSize;
    public float projectileSpeed;

    public float startPoint;
    public float targetPoint;
    public float splash;

    public float damage;
    public float cooldown;
    public float duration;
    public float lifeTime;

    public int currentLevel;
    public int minLevel = 1;
    public int maxLevel = 5;
}

public enum SkillType
{
    None      = 0,
    Pierce    = 1,
    Explosion = 2,
    Bounce    = 3,
}