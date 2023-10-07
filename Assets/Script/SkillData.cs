[System.Serializable]
public class SkillData
{
    public int ID = 200000;
    public SkillType type = 0;
    public string name = "스킬 이름"; 
    public string info = "스킬 정보";

    public float size = 1f;
    public float speed = 5f;
    public float splash = 1f;
    public float damage = 10f;
    public float cooldown = 1f;
    public float duration = 0f;
    public float lifeTime = 3f;

    public int currentLevel = 1; 
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