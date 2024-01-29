using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    internal List<GameObject> learnSkillPrefab = new List<GameObject>();
    private Dictionary<GameObject, float> skillCooldowns = new Dictionary<GameObject, float>();
    
    private PlayerData playerData;

    private Queue<GameObject> skillReadyQueue = new Queue<GameObject>();
    private Dictionary<GameObject, SkillBehavior> skillBehaviorCache = new Dictionary<GameObject, SkillBehavior>();

    private void Start()
    {
        playerData = GetComponent<PlayerData>();

        LearnSkill(playerData.currentSkillID);
    }

    private void Update()
    {
        if (GameManager.isGameover)
        {
            return;
        }

        // 학습된 스킬을 확인 후 쿨다운 업데이트
        foreach (var skillPrefab in learnSkillPrefab)
        {
            if (!skillBehaviorCache.TryGetValue(skillPrefab, out var skillBehavior))
            {
                skillBehavior = skillPrefab.GetComponent<SkillBehavior>();
                skillBehaviorCache[skillPrefab] = skillBehavior;
            }

            if (skillBehavior == null)
            {
                continue;
            }

            skillCooldowns.TryGetValue(skillPrefab, out var currentCooldown);
            if (currentCooldown >= skillBehavior.skillData.cooldown)
            {
                skillReadyQueue.Enqueue(skillPrefab);
            }
            else
            {
                skillCooldowns[skillPrefab] = currentCooldown + Time.deltaTime;
            }
        }

        // 준비된 스킬 활성화
        while (skillReadyQueue.Count > 0)
        {
            var skillPrefab = skillReadyQueue.Dequeue();
            var skillBehavior = skillBehaviorCache[skillPrefab];

            skillBehavior.Activate();
            skillCooldowns[skillPrefab] = 0f;
        }
    }


    public void LearnSkill(int id)
    {
        var gravityCannonId = 10000003; // 그라비티캐논 ID
        var thunderBoltId = 10000005;   // 썬더볼트 ID

        var skillPrefab = SkillManager.Instance.GetSkillPrefab(id);
        if (skillPrefab == null)
        {
            Debug.LogError("Skill not found for ID: " + id);
            return;
        }

        if (id == gravityCannonId || id == thunderBoltId)
        {
            learnSkillPrefab.Add(skillPrefab);
            skillCooldowns[skillPrefab] = 0f;
            return;
        }

        if (learnSkillPrefab.Contains(skillPrefab))
        {
            var skillBehavior = skillPrefab.GetComponent<SkillBehavior>();
            if (skillBehavior != null)
            {
                int currentId = skillBehavior.skillData.ID;
                int newId = currentId + 10;

                if (newId < currentId + 50)
                {
                    learnSkillPrefab.Remove(skillPrefab);

                    var upgradedSkillPrefab = SkillManager.Instance.GetSkillPrefab(newId);
                    if (upgradedSkillPrefab != null)
                    {
                        learnSkillPrefab.Add(upgradedSkillPrefab);
                        skillCooldowns[upgradedSkillPrefab] = 0f;
                    }
                    else
                    {
                        Debug.LogError("Upgraded skill not found for ID: " + newId);
                    }
                }
                else
                {
                    Debug.LogError("Maximum skill level reached for ID: " + currentId);
                }
            }
            return;
        }

        learnSkillPrefab.Add(skillPrefab);
        skillCooldowns[skillPrefab] = 0f;
    }
}