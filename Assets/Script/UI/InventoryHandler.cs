using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    public Button button;
    public ItemData itemData;

    void Start()
    {
        button.onClick.AddListener(ShowPopup);
    }

    public void SetItemData(ItemData data)
    {
        itemData = data;
    }

    public void ShowPopup()
    {
        if (PopupManager.Instance != null && itemData != null)
        {
            PopupManager.Instance.ShowItemPopup(itemData);
        }
    }
}