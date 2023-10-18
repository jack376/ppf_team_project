using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipController : MonoBehaviour
{
    public Button equipButton;
    public Image buttonImageCurrent;
    public Sprite equipFalse;
    public Sprite equipTrue;
    public TextMeshProUGUI buttonText;

    private bool isEquip = false;

    void Start()
    {
        equipButton.onClick.AddListener(ToggleEquip);
    }

    public void ToggleEquip()
    {
        if (isEquip)
        {
            isEquip = false;
            buttonImageCurrent.sprite = equipFalse;
            buttonText.text = "장착";
            InventoryManager.Instance.RemoveItem(PopupManager.Instance.currentItemData);
        }
        else
        {
            isEquip = true;
            buttonImageCurrent.sprite = equipTrue;
            buttonText.text = "해제";
            InventoryManager.Instance.EquipItem(PopupManager.Instance.currentItemData);
        }
    }   
}