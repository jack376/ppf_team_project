using UnityEngine;

public class SkillBehaviorFix : MonoBehaviour
{
    public SkillData skillData;
    public GameObject projectile;

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
            CreateProjectile();
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

    public void CreateProjectile()
    {
        GameObject skill = Instantiate(projectile, GameManager.weapon.transform.position, targetQuaternion);
        SkillProjectile skillProjectile = skill.GetComponent<SkillProjectile>();

        skillProjectile.projectileName = projectile.name;

        skillProjectile.targetTransform = targetTransform;
        skillProjectile.type = skillData.skillType;

        skillProjectile.speed = skillData.speed;
        skillProjectile.splash = skillData.splash;
        skillProjectile.damage = skillData.damage;
        skillProjectile.lifeTime = skillData.lifeTime;
    }

    public void UpdateSkillData(SkillData newSkillData)
    {
        skillData = newSkillData;
    }
}