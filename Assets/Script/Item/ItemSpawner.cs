using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] dropItems;
    public float dropChanceRate = 0.05f;

    public void DropItem()
    {
        if (Random.value <= dropChanceRate)
        {
            float randomRotation = Random.Range(0f, 360f);
            Quaternion rotation = Quaternion.Euler(0f, randomRotation, 0f);

            int itemIndex = Random.Range(0, dropItems.Length);
            Instantiate(dropItems[itemIndex], transform.position, rotation);
        }
    }
}
