using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }
    
    public ItemData currentItemData;

    public GameObject popupPrefab;

    private Image iconImage;
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

    public void ShowItemPopup(ItemData itemData)
    {
        if (currentPopup != null)
        {
            Destroy(currentPopup);
        }

        currentItemData = itemData;
        currentPopup = Instantiate(popupPrefab, transform);

        TextMeshProUGUI[] texts = currentPopup.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var text in texts)
        {
            switch (text.gameObject.name)
            {
                case "NameText": text.text          = $"{itemData.Name}"; break;
                case "RankText": text.text          = $"{itemData.Rank}"; break;
                case "AttackText": text.text        = $"{itemData.Attack}"; break;
                case "HPText": text.text            = $"{itemData.HP}"; break;
                case "DefenseText": text.text       = $"{itemData.Defense}"; break;
                case "AttackSpeedText": text.text   = $"{itemData.AttackSpeed}"; break;
                case "ShotTypeValueText": text.text = $"{itemData.ShotTypeValue}"; break;
                case "MoveSpeedText": text.text     = $"{itemData.MoveSpeed}"; break;
                case "DropText": text.text          = $"{itemData.Drop}"; break;
            }
        }

        Image[] images = currentPopup.GetComponentsInChildren<Image>(true);
        foreach (Image image in images)
        {
            if (image.CompareTag("Icon"))
            {
                iconImage = image;
                break;
            }
        }

        if (iconImage != null)
        {
            Sprite newSprite = Resources.Load<Sprite>("Icons/Items/" + itemData.Icon);
            if (newSprite != null)
            {
                iconImage.sprite = newSprite;
            }
        }

        currentPopup.SetActive(true);
    }
}