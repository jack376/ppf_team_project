public class PlayerEquipment
{
    public EquipmentData[] equippedItems = new EquipmentData[5];

    public void EquipItem(int slot, EquipmentData item)
    {
        if (slot >= 0 && slot < 5)
        {
            equippedItems[slot] = item;
        }
    }

    public EquipmentData GetEquippedItem(int slot)
    {
        if (slot >= 0 && slot < 5)
        {
            return equippedItems[slot];
        }
        else
        {
            return null;
        }
    }
}