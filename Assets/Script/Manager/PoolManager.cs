using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    private Dictionary<string, ObjectPool<GameObject>> pools;
    private GameObject poolObjectParent;

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

    public ObjectPool<GameObject> GetPool(string id, GameObject prefab, int initialSize = 100)
    {
        if (!pools.ContainsKey(id))
        {
            var newPool = new ObjectPool<GameObject>
            (
                createFunc:      ()       => Instantiate(prefab, GameManager.weapon.transform.position, Quaternion.identity),
                actionOnGet:     instance => instance.SetActive(true),
                actionOnRelease: instance =>
                {
                    instance.SetActive(false);
                    instance.transform.SetParent(poolObjectParent.transform, false);
                },
                actionOnDestroy: null,
                collectionCheck: true,
                defaultCapacity: initialSize,
                maxSize:         initialSize
            );

            pools.Add(id, newPool);
        }
        return pools[id];
    }
}
