using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    private Dictionary<int, Coroutine> skillCoroutines = new Dictionary<int, Coroutine>();

    private void LearnSkill(int id)
    {
        if (skillCoroutines.ContainsKey(id))
        {
            return;
        }

        skillCoroutines[id] = StartCoroutine(UseSkillCoroutine(id));
    }

    public void UnlearnSkill(int id)
    {
        if (skillCoroutines.TryGetValue(id, out Coroutine skillCoroutine))
        {
            StopCoroutine(skillCoroutine);
            skillCoroutines.Remove(id);
        }
    }

    public void UpgradeSkill(int oldSkillId, int newSkillId)
    {
        UnlearnSkill(oldSkillId);
        LearnSkill(newSkillId);
    }

    private IEnumerator UseSkillCoroutine(int id)
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

    public void LearnBaseAttack() // 테스트용
    {
        int id = 10000001;
        LearnSkill(id);
    }
}