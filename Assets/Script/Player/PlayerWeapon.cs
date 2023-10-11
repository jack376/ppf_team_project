using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private SkillManager skillManager;
    private List<int> ids = new List<int>();
    private Dictionary<int, Coroutine> activeCoroutines = new Dictionary<int, Coroutine>();

    private void Start()
    {
        skillManager = SkillManager.Instance;
        foreach (int id in ids)
        {
            StartCoroutine(UseSkillCoroutine(id));
        }

        Invoke("GetBaseAttack", 3f);
    }

    private void Update()
    {
        if (GameManager.isGameover)
        {
            StopAllSkills();
        }
    }

    IEnumerator UseSkillCoroutine(int id) // 나중에 최적화, 오브젝트풀링 적용 
    {
        GameObject skillPrefab = skillManager.GetSkillPrefab(id);
        SkillBehavior skillBehavior = skillPrefab.GetComponent<SkillBehavior>();

        if (skillBehavior == null || skillPrefab == null)
        {
            yield break;
        }

        while (true)
        {
            yield return new WaitForSeconds(skillBehavior.skillData.cooldown);
            {
                Instantiate(skillPrefab, GameManager.weapon.transform.position, Quaternion.identity);
            }
        }
    }

    public void StopAllSkills()
    {
        foreach (Coroutine coroutine in activeCoroutines.Values)
        {
            StopCoroutine(coroutine);
        }
        activeCoroutines.Clear();
    }

    public void GetSkill()
    {
        Debug.Log("Get Skill");
        int id = 12340002;
        StartCoroutine(UseSkillCoroutine(id));

        UIManager.Instance.CloseSkillSelectWindow();
    }

    public void GetBaseAttack()
    {
        int id = 12340001;
        ids.Add(id);
        StartCoroutine(UseSkillCoroutine(id));
    }
}