public class SkillFactory
{
    public static ISkill GetSkillType(SkillType type)
    {
        switch (type)
        {
            case SkillType.None : 
                return new TypeNone();
            case SkillType.Multi :
                return new TypeMulti();
            case SkillType.Nova:
                return new TypeNova();
            case SkillType.Area:
                return new TypeArea();

            default: 
                return new TypeNone();
        }
    }
}