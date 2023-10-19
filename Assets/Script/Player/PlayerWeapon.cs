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

        Invoke("LearnBaseAttack", 4f);
    }

    IEnumerator UseSkillCoroutine(int id)
    {
        GameObject skillPrefab = SkillManager.Instance.GetSkillPrefab(id);
        SkillBehavior skillBehavior = skillPrefab.GetComponent<SkillBehavior>();

        if (skillBehavior == null || skillPrefab == null)
        {
            yield break;
        }

        while (true)
        {
            GameObject useSkill = PoolManager.Instance.GetPool(skillPrefab.name).Get();
            useSkill.transform.position = GameManager.weapon.transform.position;
            yield return new WaitForSeconds(skillBehavior.skillData.cooldown);
        }
    }

    public void LearnSkill(int id)
    {
        ids.Add(id);
        StartCoroutine(UseSkillCoroutine(id));
    }

    public void LearnBaseAttack()
    {
        int id = 10000001;
        ids.Add(id);
        StartCoroutine(UseSkillCoroutine(id));
    }
}