using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private List<int> ids = new List<int>();

    private void Start()
    {
        foreach (int id in ids)
        {
            StartCoroutine(UseSkillCoroutine(id));
        }

        Invoke("LearnBaseAttack", 3f);
    }

    IEnumerator UseSkillCoroutine(int id) // 나중에 최적화, 오브젝트풀링 적용 
    {
        GameObject skillPrefab = SkillManager.Instance.GetSkillPrefab(id);
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

    public void LearnSkill(int id)
    {
        Debug.Log("Get Skill" + id);
        ids.Add(id);
        StartCoroutine(UseSkillCoroutine(id));
    }

    public void LearnBaseAttack()
    {
        int id = 1000001;
        ids.Add(id);
        StartCoroutine(UseSkillCoroutine(id));
    }
}