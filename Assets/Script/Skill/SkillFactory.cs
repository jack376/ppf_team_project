public class SkillFactory
{
    public static ISkill GetSkillType(SkillType type)
    {
        switch (type)
        {
            case SkillType.None : 
                return new TypeNone();
            default: 
                return new TypeNone();
        }
    }
}