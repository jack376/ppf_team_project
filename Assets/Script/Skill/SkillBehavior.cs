using UnityEngine;

public class SkillBehavior : MonoBehaviour
{
    public ISkill skillInterface;
    public SkillData skillData;

    public LayerMask targetLayer;
    public LayerMask groundLayer;

    private float searchRadius = 1500f;
    private Collider[] overlapResults = new Collider[250];

    private Transform targetTransform;
    private Quaternion targetQuaternion;

    private void Start()
    {
        FindNearTarget();

        if (targetTransform != null)
        {
            targetQuaternion = Quaternion.LookRotation(targetTransform.position - transform.position);

            skillInterface = SkillFactory.GetSkillType(skillData.skillType);
            if (skillInterface != null)
            {
                skillInterface.Execute(skillData, targetLayer, targetQuaternion);
            }
        }
    }

    private void FindNearTarget()
    {
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, searchRadius, overlapResults, targetLayer);
        float minDistance = float.MaxValue;

        for (int i = 0; i < numColliders; i++)
        {
            float distance = (overlapResults[i].transform.position - transform.position).sqrMagnitude;
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