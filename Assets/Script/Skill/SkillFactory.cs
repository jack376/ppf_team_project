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

            default: 
                return new TypeNone();
        }
    }
}