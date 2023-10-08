using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private SkillManager skillManager;
    private List<int> ids = new List<int>();

    private void Start()
    {
        skillManager = SkillManager.Instance;
        foreach (int id in ids)
        {
            StartCoroutine(UseSkillCoroutine(id));
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            Debug.Log("Q");
            int id = 12340002;
            ids.Add(id);
            StartCoroutine(UseSkillCoroutine(id));
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            Debug.Log("W");
            int id = 12340001;
            ids.Add(id);
            StartCoroutine(UseSkillCoroutine(id));
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
                Instantiate(skillPrefab, transform);
            }
        }
    }
}