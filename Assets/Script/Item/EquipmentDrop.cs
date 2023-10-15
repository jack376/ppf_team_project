using UnityEngine;

public class EquipmentDrop : MonoBehaviour
{
    public EquipmentData equipmentData;

    public void Use(GameObject target)
    {
        InventoryManager.Instance.AddItem(equipmentData);
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