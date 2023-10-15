using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;
    public List<GameObject> allSkillPrefabs = new List<GameObject>();
    public Dictionary<int, GameObject> skillDictionary = new Dictionary<int, GameObject>();

    public List<SkillData> skillList = new List<SkillData>();

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

        foreach (GameObject skillPrefab in allSkillPrefabs)
        {
            SkillBehavior skillBehavior = skillPrefab.GetComponent<SkillBehavior>();
            if (skillBehavior != null)
            {
                skillDictionary[skillBehavior.skillData.ID] = skillPrefab;
            }
        }

        LoadSkillDataFromCSV("Assets/Resources/SkillSaveData.csv");
    }

    void Start()
    {
        foreach (SkillData skill in skillList)
        {
            if (skillDictionary.TryGetValue(skill.ID, out GameObject skillPrefab))
            {
                SkillBehavior skillBehavior = skillPrefab.GetComponent<SkillBehavior>();
                if (skillBehavior != null)
                {
                    skillBehavior.UpdateSkillData(skill);
                }
            }
            else
            {
                Debug.LogWarning("해당 ID 프리펩 없어");
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

    void LoadSkillDataFromCSV(string filePath)
    {
        StreamReader sr = new StreamReader(filePath, Encoding.Default);

        bool isFirstRow = true;
        while (!sr.EndOfStream)
        {
            string line = sr.ReadLine();

            if (isFirstRow)
            {
                isFirstRow = false;
                continue;
            }

            string[] rowData = line.Split(',');

            SkillData newData = new SkillData();

            newData.ID              = int.Parse(rowData[0]);
            newData.skillType       = (SkillType)int.Parse(rowData[1]);
            newData.name            = rowData[2];
            newData.info            = rowData[3];

            newData.shotType        = (ShotType)int.Parse(rowData[4]);
            newData.shotTypeValue   = float.Parse(rowData[5]);

            newData.count           = int.Parse(rowData[6]);
            newData.cooldown        = float.Parse(rowData[7]);
            newData.speed           = float.Parse(rowData[8]);
            newData.splash          = float.Parse(rowData[9]);
            newData.damage          = float.Parse(rowData[10]);
            newData.lifeTime        = float.Parse(rowData[11]);

            newData.isPierceHitPlay = int.Parse(rowData[12]);

            newData.currentLevel    = int.Parse(rowData[13]);
            newData.minLevel        = int.Parse(rowData[14]);
            newData.maxLevel        = int.Parse(rowData[15]);

            skillList.Add(newData);
        }

        sr.Close();
    }

    void SaveSkillDataToCSV(string filePath)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("ID,skillType,name,info,shotType,shotTypeValue,count,cooldown,speed,splash,damage,lifeTime,isPierceHitPlay,currentLevel,minLevel,maxLevel");

        foreach (var skill in skillList)
        {
            sb.AppendLine
            (
                $"{skill.ID}," +
                $"{(int)skill.skillType}," +
                $"{skill.name}," +
                $"{skill.info}," +
                $"{(int)skill.shotType}," +
                $"{skill.shotTypeValue}," +
                $"{skill.count}," +
                $"{skill.cooldown}," +
                $"{skill.speed}," +
                $"{skill.splash}," +
                $"{skill.damage}," +
                $"{skill.lifeTime}," +
                $"{skill.isPierceHitPlay}," +
                $"{skill.currentLevel}," +
                $"{skill.minLevel}," +
                $"{skill.maxLevel}"
            );
        }

        File.WriteAllText(filePath, sb.ToString());
    }
}
