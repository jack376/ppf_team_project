using UnityEngine;

[System.Serializable]
public class SkillData
{
    [Header("스킬 ID, 스킬 타입, 해당 타입에 필요한 값들")]
    public int ID = 10000001;
    public SkillType type = 0;
    public float multiShotAngle = 45f;
    public int multiShotBulletCount = 5;

    [Header("스킬 이름, 스킬 상세 정보")]
    public string name = "스킬 이름"; 
    public string info = "스킬 정보";

    [Header("관통 제한 횟수")]
    public int pierceCount = 1;

    [Header("쿨다운, 탄속, 폭발범위, 데미지, 생명주기")]
    public float cooldown = 1f;
    public float speed    = 15f;
    public float splash   = 3f;
    public float damage   = 20f;
    public float lifeTime = 0.5f;

    [Header("현재 레벨, 최소 레벨, 최대 레벨")]
    public int currentLevel = 0; 
    public int minLevel = 0;
    public int maxLevel = 5;
}

public enum SkillType
{
    None  = 0,
    Multi = 1,
    Nova  = 2,
    Area  = 3,
}