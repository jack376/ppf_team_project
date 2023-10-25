using UnityEngine;

public class SkillBehavior : MonoBehaviour
{
    public ISkill skill;
    public SkillData skillData;

    public LayerMask targetLayer;
    public LayerMask groundLayer;

    private float searchRadius = 1500f;
    private Collider[] overlapResults = new Collider[250];

    private Transform targetTransform;
    private Quaternion targetQuaternion;

    public void Activate()
    {
        var playerPosition = GameManager.player.transform.position;
        FindNearTarget(playerPosition);

        if (targetTransform != null)
        {
            targetQuaternion = Quaternion.LookRotation(targetTransform.position - playerPosition);

            skill = SkillFactory.GetSkillType(skillData.type);
            if (skill != null)
            {
                skill.Execute(skillData, targetLayer, targetQuaternion);
            }
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