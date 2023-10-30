using System.Collections.Generic;
using UnityEngine;

public class SkillFactory
{
    public static Dictionary<SkillType, ISkill> skillCache = new Dictionary<SkillType, ISkill>();

    public static ISkill GetSkillType(SkillType type)
    {
        if (!skillCache.ContainsKey(type))
        {
            var skillInstance = CreateSkill(type);
            skillCache[type] = skillInstance;
        }

        return skillCache[type];
    }

    public static ISkill CreateSkill(SkillType type)
    {
        switch (type)
        {
            case SkillType.None  : return new TypeNone();
            case SkillType.Multi : return new TypeMulti();
            case SkillType.Nova  : return new TypeNova();
            case SkillType.Area  : return new TypeArea();
            case SkillType.Buff  : return new TypeBuff();

            default: 
                Debug.LogError("스킬을 찾을 수 없어");
                return null;
        }
    }
}
