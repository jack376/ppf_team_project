using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    public List<SkillData> skillList = new List<SkillData>();

    public GameObject projectilePrefab;
    public List<GameObject> allSkillPrefabs = new List<GameObject>();
    public Dictionary<int, GameObject> skillDic = new Dictionary<int, GameObject>();

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

        //LoadSkillDataFromCSV("Assets/Resources/SkillSaveData.csv");

        LinkedSkillPrefab();
        UpdateSkillDataInPrefab();
    }

    private void LinkedSkillPrefab()
    {
        foreach (var skillPrefab in allSkillPrefabs)
        {
            var sb = skillPrefab.GetComponent<SkillBehavior>();
            if (sb != null)
            {
                skillDic[sb.skillData.ID] = skillPrefab;
                Debug.Log("LinkedSkillPrefab" + sb.skillData.ID);
            }
        }
    }

    private void UpdateSkillDataInPrefab()
    {
        foreach (var skill in skillList)
        {
            if (skillDic.TryGetValue(skill.ID, out var skillPrefab))
            {
                var sb = skillPrefab.GetComponent<SkillBehavior>();
                if (sb != null)
                {
                    sb.UpdateSkillData(skill);
                    Debug.Log("UpdateSkillDataInPrefab" + skill.ID);
                }
            }
        }
    }

    public GameObject GetSkillPrefab(int id)
    {
        skillDic.TryGetValue(id, out var skillPrefab);
        return skillPrefab;
    }

    public SkillData GetSkillData(int id)
    {
        skillDic.TryGetValue(id, out var skillPrefab);
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

            newData.bulletCount  = int.Parse(rowData[4]);
            newData.pierceCount  = int.Parse(rowData[5]);

            newData.cooldown     = float.Parse(rowData[6]);
            newData.speed        = float.Parse(rowData[7]);
            newData.splash       = float.Parse(rowData[8]);
            newData.damage       = float.Parse(rowData[9]);
            newData.lifeTime     = float.Parse(rowData[10]);

            newData.currentLevel = int.Parse(rowData[11]);
            newData.minLevel     = int.Parse(rowData[12]);
            newData.maxLevel     = int.Parse(rowData[13]);

            skillList.Add(newData);
        }

        sr.Close();
    }
}

