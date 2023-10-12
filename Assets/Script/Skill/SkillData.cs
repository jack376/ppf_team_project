using UnityEngine;

[System.Serializable]
public class SkillData
{
    [Header("ID, 타입, 이름, 정보")]
    public int ID = 12340001;
    public SkillType skillType = 0;
    public string name = "스킬 이름"; 
    public string info = "스킬 정보";

    [Header("발사 타입, 각도 & 연발간격 값")]
    public ShotType shotType = 0;
    public float shotTypeValue = 15f;

    [Header("총알 갯수, 쿨다운")]
    public int count = 1;
    public float cooldown = 1f;

    [Header("탄속, 폭발범위, 데미지, 생명주기")]
    public float speed = 5f;
    public float splash = 1f;
    public float damage = 10f;
    public float lifeTime = 3f;

    [Header("관통 시 Hit 파티클 재생 여부")]
    public bool isPierceHitPlay = false;

    [Header("현재 레벨, 최소 레벨, 최대 레벨")]
    public int currentLevel = 1; 
    public int minLevel = 1;
    public int maxLevel = 5;
}

public enum SkillType
{
    Explosion = 0,
    Pierce    = 1,
    Nova      = 2,
    Airstrike = 3,
}
public enum ShotType
{
    Single = 0,
    Burst  = 1,
    Multi  = 2,
}