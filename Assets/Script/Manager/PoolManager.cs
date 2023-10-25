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

    private void Start()
    {
        GameObject projectilePrefab = SkillManager.Instance.projectilePrefab;
        List<GameObject> skillPrefabs = SkillManager.Instance.allSkillPrefabs;

        foreach (GameObject skillPrefab in skillPrefabs)
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

    public void CreatePool(GameObject key, GameObject prefab, int initialSize = 256)
    {
        Vector3 playerPosition = GameManager.player.transform.position;

        ObjectPool<GameObject> newPool = new ObjectPool<GameObject>
        (
            createFunc: () => Instantiate(prefab, playerPosition, Quaternion.identity),
            actionOnGet: instance =>
            {
                instance.transform.position = playerPosition;
                instance.SetActive(true);
            },
            actionOnRelease: instance =>
            {
                instance.SetActive(false);
                instance.transform.position = playerPosition;
                instance.transform.SetParent(poolObjectParent.transform, false);
            },
            defaultCapacity: initialSize
        );
        pools.Add(key, newPool);
    }
}