using UnityEngine;

public class SkillBehavior : MonoBehaviour
{
    public LayerMask targetLayer;
    public LayerMask groundLayer;

    public GameObject projectileParticle;
    public GameObject hitParticle;

    public ISkill skill;
    public SkillData skillData;

    private float searchRadius = 1500f;
    private Collider[] overlapResults = new Collider[250];

    private Transform targetTransform;
    private Quaternion targetQuaternion;
    private Vector3 playerPosition;

    private ISkill typeNone  = new TypeNone();
    private ISkill typeMulti = new TypeMulti();
    private ISkill typeNova  = new TypeNova();
    private ISkill typeArea  = new TypeArea();
    private ISkill typeBuff  = new TypeBuff();

    public void Activate()
    {
        playerPosition = GameManager.player.transform.position;
        FindNearTarget(playerPosition);

        if (targetTransform != null)
        {
            targetQuaternion = Quaternion.LookRotation(targetTransform.position - playerPosition);

            skill = GetSkill(skillData.type);
            if (skill != null)
            {
                skill.Execute(skillData, targetLayer, targetQuaternion, hitParticle, projectileParticle, targetTransform.position);
            }
        }
    }

    public ISkill GetSkill(SkillType type)
    {
        switch (type)
        {
            case SkillType.None : return typeNone;
            case SkillType.Multi: return typeMulti;
            case SkillType.Nova : return typeNova;
            case SkillType.Area : return typeArea;
            case SkillType.Buff : return typeBuff;

            default: return typeNone;
        }
    }

    private void FindNearTarget(Vector3 position)
    {
        var numColliders = Physics.OverlapSphereNonAlloc(position, searchRadius, overlapResults, targetLayer);
        var minDistance = float.MaxValue;

        for (int i = 0; i < numColliders; i++)
        {
            var distance = (overlapResults[i].transform.position - position).sqrMagnitude;
            if (distance < minDistance)
            {
                targetTransform = overlapResults[i].transform;
                minDistance = distance;
            }
        }
    }

    public void UpdateSkillData(SkillData newSkillData)
    {
        skillData = newSkillData;
    }
}