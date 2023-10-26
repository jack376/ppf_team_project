using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] dropItems;
    public float dropChanceRate = 0.05f;

    public void DropItem()
    {
        if (Random.value <= dropChanceRate)
        {
            int itemIndex = Random.Range(0, dropItems.Length);

            var spawnItemGo = PoolManager.Instance.GetPool(dropItems[itemIndex]).Get();
            spawnItemGo.transform.position = transform.position;
            spawnItemGo.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            var spawnItem = spawnItemGo.GetComponent<Gem>();
            spawnItem.onItemRelease += ReleaseItem;

            void ReleaseItem()
            {
                spawnItem.onItemRelease -= ReleaseItem;
                PoolManager.Instance.GetPool(dropItems[itemIndex]).Release(spawnItemGo);
            }
        }
    }
}
