using TMPro;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    public GameObject itemPopupPrefab;
    private GameObject currentPopup;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ShowItemPopup(EquipmentData itemData)
    {
        if (currentPopup != null)
        {
            Destroy(currentPopup);
        }

        currentPopup = Instantiate(itemPopupPrefab, transform);

        currentPopup.GetComponentInChildren<TextMeshProUGUI>().text = itemData.Name;
        currentPopup.GetComponentInChildren<TextMeshProUGUI>().text = itemData.Icon;
        currentPopup.GetComponentInChildren<TextMeshProUGUI>().text = itemData.Rank.ToString();
        currentPopup.GetComponentInChildren<TextMeshProUGUI>().text = itemData.Enchant.ToString();

        currentPopup.GetComponentInChildren<TextMeshProUGUI>().text = itemData.BasicAttack.ToString();
        currentPopup.GetComponentInChildren<TextMeshProUGUI>().text = itemData.BasicHP.ToString();
        currentPopup.GetComponentInChildren<TextMeshProUGUI>().text = itemData.BasicDefence.ToString();

        currentPopup.GetComponentInChildren<TextMeshProUGUI>().text = itemData.GoldNeeded.ToString();
        currentPopup.GetComponentInChildren<TextMeshProUGUI>().text = itemData.SellPrice.ToString();

        currentPopup.SetActive(true);
    }
}