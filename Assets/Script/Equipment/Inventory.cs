using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<EquipmentData> items = new List<EquipmentData>();
    public PlayerEquipment playerEquipment;

    public void AddItem(EquipmentData item)
    {
        items.Add(item);
    }

    public void RemoveItem(int index)
    {
        if (index >= 0 && index < items.Count)
        {
            items.RemoveAt(index);
        }
    }

    public void EquipFromInventory(int inventoryIndex, int equipmentSlot)
    {
        if (inventoryIndex >= 0 && inventoryIndex < items.Count)
        {
            EquipmentData itemToEquip = items[inventoryIndex];
            playerEquipment.EquipItem(equipmentSlot, itemToEquip);
        }
        else
        {
            Debug.LogError("해당 인덱스에 장비 없음");
        }
    }

    public List<EquipmentData> GetItemList()
    {
        return items;
    }
}
