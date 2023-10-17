using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public ItemData itemData;

    public void Use(GameObject target)
    {
        InventoryManager.Instance.AddItem(itemData);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Use(other.gameObject);
            Destroy(gameObject);
        }
    }
}