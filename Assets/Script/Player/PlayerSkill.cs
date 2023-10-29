using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    internal List<GameObject> learnSkillPrefab = new List<GameObject>();
    private Dictionary<GameObject, float> skillCooldowns = new Dictionary<GameObject, float>();
    private PlayerData playerData;

    private void Start()
    {
        playerData = GetComponent<PlayerData>();
        LearnSkill(playerData.baseSkillID);
    }

    private void Update()
    {
        if (GameManager.isGameover)
        {
            return;
        }

        List<GameObject> skillReadyList = new List<GameObject>();
        foreach (var skillPrefab in learnSkillPrefab)
        {
            skillCooldowns.TryGetValue(skillPrefab, out float currentCooldown);

            var skillBehavior = skillPrefab.GetComponent<SkillBehavior>();

            //float cooldown = skillBehavior.skillData.cooldown * playerData.cooldownReduction;
            if (skillBehavior && currentCooldown >= skillBehavior.skillData.cooldown)
            {
                skillReadyList.Add(skillPrefab);
            }
            else
            {
                skillCooldowns[skillPrefab] = currentCooldown + Time.deltaTime;
            }
        }

        foreach (var skillPrefab in skillReadyList)
        {
            var skillBehavior = skillPrefab.GetComponent<SkillBehavior>();
            if (skillBehavior)
            {
                skillBehavior.Activate();
                skillCooldowns[skillPrefab] = 0f;
            }
        }
    }

    public void LearnSkill(int id)
    {
        var skillPrefab = SkillManager.Instance.GetSkillPrefab(id);
        if (skillPrefab && !learnSkillPrefab.Contains(skillPrefab))
        {
            learnSkillPrefab.Add(skillPrefab);
            skillCooldowns[skillPrefab] = 0f;
        }

        Debug.Log("LearnSkill" + id);
    }
}
