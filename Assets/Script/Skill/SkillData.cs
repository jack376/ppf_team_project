using UnityEngine;

[System.Serializable]
public class SkillData
{
    [Header("ID, 타입, 이름, 정보")]
    public int ID = 200000;
    public SkillType type = 0;
    public string name = "스킬 이름"; 
    public string info = "스킬 정보"; 

    [Header("발사체 크기, 탄속, 폭발범위, 데미지, 쿨다운, 생명주기")]
    public float size = 1f;
    public float speed = 5f;
    public float splash = 1f;
    public float damage = 10f;
    public float cooldown = 1f;
    public float lifeTime = 3f;

    [Header("현재 레벨, 최소 레벨, 최대 레벨")]
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