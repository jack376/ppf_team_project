[System.Serializable]
public class ItemData
{
    public int ID = 91000000;
    public int Character = 0;
    public string Name = "ShortAmountOfTime";
    public ItemRank Rank = 0;
    public float Attack = 10f;
    public float HP = 100f;
    public float Defense = 1f;
    public float AttackSpeed = 2f;
    public float ShotTypeValue = 1f;
    public float MoveSpeed = 3f;
    public float Drop = 0.1f;
    public string Icon = "Icon_Stopwatch";
    public bool IsEquipped = false;
    public int Type = 0;
}

public enum ItemRank
{
    None   = 0,
    Normal = 1,
    Rare   = 2,
    Unique = 3,
}