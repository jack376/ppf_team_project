[System.Serializable]
public class ItemData
{
    public int ID = 9100000;
    public int Character = 1;
    public string Name = "ShortAmountOfTime";
    public ItemRank Rank = (ItemRank)1;
    public float Attack = 10f;
    public float HP = 100f;
    public float Defense = 1f;
    public float AttackSpeed = 2f;
    public float ShotTypeValue = 1f;
    public float MoveSpeed = 3f;
    public float Drop = 0.1f;
    public string Icon = "Icon_Stopwatch";
}

public enum ItemRank
{
    Normal = 1,
    Rare   = 2,
    Unique = 3,
}