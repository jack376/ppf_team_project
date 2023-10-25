using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    public Dictionary<GameObject, ObjectPool<GameObject>> pools;
    private GameObject poolObjectParent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            pools = new Dictionary<GameObject, ObjectPool<GameObject>>();
            poolObjectParent = new GameObject("PoolObjectGroup");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        var projectilePrefab = SkillManager.Instance.projectilePrefab;
        var allSkillPrefabs  = SkillManager.Instance.allSkillPrefabs;

        foreach (var skillPrefab in allSkillPrefabs)
        {
            CreatePool(skillPrefab, skillPrefab);
        }

        CreatePool(projectilePrefab, projectilePrefab);
    }

    public ObjectPool<GameObject> GetPool(GameObject key)
    {
        if (!pools.ContainsKey(key))
        {
            Debug.LogWarning(key.name + "키에 맞는 프리팹이 없어");
            return null;
        }
        return pools[key];
    }

    public void ReleasePool(GameObject key, GameObject go)
    {
        if (pools.TryGetValue(key, out var pool))
        {
            pool.Release(go);
        }
        else
        {
            Debug.LogWarning(go.name + "얘는 풀이 없어");
        }
    }

    public void CreatePool(GameObject key, GameObject prefab, int initialSize = 5)
    {
        var playerPos = GameManager.player.transform.position;
        var newPool = new ObjectPool<GameObject>
        (
            createFunc: () => Instantiate(prefab, playerPos, Quaternion.identity),
            actionOnGet: instance =>
            {
                instance.transform.position = playerPos;
                instance.SetActive(true);
            },
            actionOnRelease: instance =>
            {
                instance.SetActive(false);
                instance.transform.position = playerPos;
                instance.transform.SetParent(poolObjectParent.transform, false);
            },
            defaultCapacity: initialSize
        );
        pools.Add(key, newPool);
    }
}