using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    public Button button;
    public ItemData itemData { get; set; }

    void Start()
    {
        button.onClick.AddListener(ShowPopup);
    }

    public void ShowPopup()
    {
        if (PopupManager.Instance != null)
        {
            PopupManager.Instance.ShowItemPopup(itemData);
            PopupManager.Instance.currentItemData = itemData;
        }
    }
}