using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    public Dictionary<GameObject, ObjectPool<GameObject>> pools;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            pools = new Dictionary<GameObject, ObjectPool<GameObject>>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        var projectilePrefab = SkillManager.Instance.projectilePrefab;
        CreatePool(projectilePrefab, projectilePrefab);

        InitPool(SkillManager.Instance.allParticlePrefabs);
        InitPool(EnemyManager.Instance.allEnemyPrefabs);
        InitPool(DropItemManager.Instance.allDropItemPrefabs);
        InitPool(DamageTextManager.Instance.allDamageTextPrefabs);
    }

    private void InitPool(List<GameObject> allPrefabs, int initialSize = 500)
    {
        foreach (var prefab in allPrefabs)
        {
            CreatePool(prefab, prefab, initialSize);
        }
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

    public void CreatePool(GameObject key, GameObject prefab, int initialSize = 500)
    {
        var createPool = new ObjectPool<GameObject>
        (
            createFunc: () => Instantiate(prefab, GameManager.player.transform.position, Quaternion.identity),
            actionOnGet: instance =>
            {
                instance.transform.position = GameManager.player.transform.position;
                instance.SetActive(true);
            },
            actionOnRelease: instance =>
            {
                instance.SetActive(false);
            },
            defaultCapacity: initialSize
        );
        pools.Add(key, createPool);
    }
}