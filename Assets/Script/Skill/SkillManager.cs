using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public List<GameObject> allSkillPrefabs = new List<GameObject>();

    private Dictionary<int, GameObject> skillDictionary = new Dictionary<int, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        foreach (GameObject skillPrefab in allSkillPrefabs)
        {
            SkillBehavior skillBehavior = skillPrefab.GetComponent<SkillBehavior>();
            if (skillBehavior != null)
            {
                skillDictionary.Add(skillBehavior.skillData.ID, skillPrefab);
            }
        }
    }

    public GameObject GetSkillPrefab(int id)
    {
        skillDictionary.TryGetValue(id, out GameObject skillPrefab);
        return skillPrefab;
    }

    public SkillData GetSkillData(int id)
    {
        skillDictionary.TryGetValue(id, out GameObject skillPrefab);
        SkillBehavior skillBehavior = skillPrefab.GetComponent<SkillBehavior>();

        return skillBehavior.skillData;
    }
}
