using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
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

    void ShowPopup()
    {
        if (PopupManager.Instance != null && itemData != null)
        {
            PopupManager.Instance.ShowItemPopup(itemData);
        }
    }
}