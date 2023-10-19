using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    public Dictionary<string, ObjectPool<GameObject>> pools;
    public GameObject poolObjectParent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            pools = new Dictionary<string, ObjectPool<GameObject>>();
            poolObjectParent = new GameObject("PoolObjects");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (GameObject skillPrefab in SkillManager.Instance.allSkillPrefabs)
        {
            SkillBehavior skillBehavior = skillPrefab.GetComponent<SkillBehavior>();

            CreatePool(skillPrefab.name, skillPrefab);
            CreatePool(skillBehavior.projectile.name, skillBehavior.projectile);
            CreatePool(skillBehavior.hit.name, skillBehavior.hit);
            CreatePool(skillBehavior.flash.name, skillBehavior.flash);
        }
    }

    public ObjectPool<GameObject> GetPool(string id)
    {
        if (!pools.ContainsKey(id))
        {
            Debug.Log(id + "키가 없어");
        }
        return pools[id];
    }

    public void CreatePool(string id, GameObject prefab, int initialSize = 50)
    {
        ObjectPool<GameObject> newPool = new ObjectPool<GameObject>
        (
            createFunc: () => Instantiate(prefab, GameManager.weapon.transform.position, Quaternion.identity),
            actionOnGet: instance =>
            {
                instance.transform.position = GameManager.weapon.transform.position;
                instance.SetActive(true);
            },
            actionOnRelease: instance =>
            {
                instance.SetActive(false);
                instance.transform.position = GameManager.weapon.transform.position;
                instance.transform.SetParent(poolObjectParent.transform, false);
            },
            defaultCapacity: initialSize
        );
        pools.Add(id, newPool);
    }
}
