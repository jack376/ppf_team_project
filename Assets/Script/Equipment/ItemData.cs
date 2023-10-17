[System.Serializable]
public class ItemData
{
    public int ID;
    public int Character;
    public string Name;
    public ItemRank Rank;
    public float Attack;
    public float HP;
    public float Defense;
    public float AttackSpeed;
    public float ShotTypeValue;
    public float MoveSpeed;
    public float Drop;
    public string Icon;
}

public enum ItemRank
{
    Normal = 1,
    Rare   = 2,
    Unique = 3,
}