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
        var skillPrefab = SkillManager.Instance.GetSkillPrefab(id);
        if (skillPrefab && !learnSkillPrefab.Contains(skillPrefab))
        {
            learnSkillPrefab.Add(skillPrefab);
            skillCooldowns[skillPrefab] = 0f;
        }

        Debug.Log("LearnSkill" + id);
    }
}
