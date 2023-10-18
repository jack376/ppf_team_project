using UnityEngine;
using UnityEngine.UI;

public class EquipUpdate : MonoBehaviour
{
    public Image itemIcon;

    public void UpdateSlot(ItemData itemData)
    {
        if (itemData != null)
        {
            itemIcon.sprite = Resources.Load<Sprite>("Icons/Items/" + itemData.Icon);
            itemIcon.enabled = true;
        }
        else
        {
            itemIcon.sprite = null;
            itemIcon.enabled = false;
        }
    }
}
