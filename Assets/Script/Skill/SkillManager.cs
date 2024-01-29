using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    public List<SkillData> skillList = new List<SkillData>();

    public GameObject projectilePrefab;
    public List<GameObject> allSkillPrefabs = new List<GameObject>();
    public List<GameObject> allParticlePrefabs = new List<GameObject>();

    public Dictionary<int, GameObject> skillDictionary = new Dictionary<int, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        SkillPrefabUpdate(Resources.LoadAll<GameObject>("Prefabs/"));

        LoadSkillDataFromCSV("Assets/Resources/SkillSaveData.csv"); 

        LinkedSkillPrefab();
        UpdateSkillDataInPrefab();
    }

    private void SkillPrefabUpdate(GameObject[] prefabs)
    {
        for (var i = 0; i < prefabs.Length; i++)
        {
            allSkillPrefabs.Add(prefabs[i]);
        }
    }

    private void LinkedSkillPrefab()
    {
        foreach (var skillPrefab in allSkillPrefabs)
        {
            if (skillPrefab.TryGetComponent<SkillBehavior>(out var skillBehevior))
            {
                skillDictionary[skillBehevior.skillData.ID] = skillPrefab;
                Debug.Log("LinkedSkillPrefab" + skillBehevior.skillData.ID);
            }
        }
    }

    private void UpdateSkillDataInPrefab()
    {
        foreach (var skill in skillList)
        {
            if (skillDictionary.TryGetValue(skill.ID, out var skillPrefab))
            {
                if (skillPrefab.TryGetComponent<SkillBehavior>(out var skillBehevior))
                {
                    skillBehevior.UpdateSkillData(skill);
                    Debug.Log("UpdateSkillDataInPrefab" + skill.ID);
                }
            }
        }
    }

    public GameObject GetSkillPrefab(int id)
    {
        skillDictionary.TryGetValue(id, out var skillPrefab);
        return skillPrefab;
    }

    public SkillData GetSkillData(int id)
    {
        skillDictionary.TryGetValue(id, out var skillPrefab);
        return skillPrefab.GetComponent<SkillBehavior>().skillData;
    }

    public void LoadSkillDataFromCSV(string filePath)
    {
        var sr = new StreamReader(filePath, Encoding.Default);
        var isFirstRow = true;

        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();
            if (isFirstRow)
            {
                isFirstRow = false;
                continue;
            }

            var rowData = line.Split(',');

            var newData = new SkillData();

            newData.ID   = int.Parse(rowData[0]);
            newData.type = (SkillType)int.Parse(rowData[1]);

            newData.name = rowData[2];
            newData.info = rowData[3];

            newData.multiBulletCount = int.Parse(rowData[4]);
            newData.pierceCount      = int.Parse(rowData[5]);

            newData.cooldown = float.Parse(rowData[6]);
            newData.speed    = float.Parse(rowData[7]);
            newData.splash   = float.Parse(rowData[8]);
            newData.damage   = float.Parse(rowData[9]);
            newData.lifeTime = float.Parse(rowData[10]);

            newData.currentLevel = int.Parse(rowData[11]);
            newData.minLevel     = int.Parse(rowData[12]);
            newData.maxLevel     = int.Parse(rowData[13]);

            skillList.Add(newData);
        }

        sr.Close();
    }
}