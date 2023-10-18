using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public GameObject basicIconPrefab;
    public GameObject emptyIconPrefab;

    public RectTransform itemSlotContent;
    public RectTransform equipSlotContent;

    private List<GameObject> itemSlots = new List<GameObject>();
    private List<GameObject> equipSlots = new List<GameObject>();

    private int itemMaxSlot = 256;
    private int equipMaxSlot = 4;

    private int nextEmptyItemSlot = 0;
    private int nextEmptyEquipSlot = 0;

    private float iconImageSize = 128f;

    private void Start()
    {
        InventoryManager.Instance.OnItemAdd += AddItemIcon;
        InventoryManager.Instance.OnItemRemove += RemoveItemIcon;
        InitItemSlot();

        InventoryManager.Instance.OnEquipAdd += AddEquipIcon;
        InventoryManager.Instance.OnEquipRemove += RemoveEquipIcon;
        InitEquipSlot();
    }

    private void OnDestroy()
    {
        InventoryManager.Instance.OnItemAdd -= AddItemIcon;
        InventoryManager.Instance.OnItemRemove -= RemoveItemIcon;

        InventoryManager.Instance.OnEquipAdd -= AddEquipIcon;
        InventoryManager.Instance.OnEquipRemove -= RemoveEquipIcon;
    }

    private void AddItemIcon(ItemData itemData)
    {
        if (nextEmptyItemSlot < itemSlots.Count)
        {
            GameObject slot = itemSlots[nextEmptyItemSlot];
            Sprite itemSprite = Resources.Load<Sprite>(itemData.Icon);

            slot.GetComponent<Image>().sprite = itemSprite;
            slot.SetActive(true);
            nextEmptyItemSlot++;
        }
    }

    private void RemoveItemIcon(ItemData itemData)
    {
        if (nextEmptyItemSlot > 0)
        {
            nextEmptyItemSlot--;
            GameObject slot = itemSlots[nextEmptyItemSlot];
            slot.GetComponent<Image>().sprite = null;
            slot.SetActive(false);
        }
    }

    private void AddEquipIcon(ItemData itemData)
    {
        if (nextEmptyEquipSlot < equipSlots.Count)
        {
            GameObject slot = equipSlots[nextEmptyEquipSlot];
            Sprite itemSprite = Resources.Load<Sprite>(itemData.Icon);

            slot.GetComponent<Image>().sprite = itemSprite;
            slot.SetActive(true);
            nextEmptyEquipSlot++;
        }
    }

    private void RemoveEquipIcon(ItemData itemData)
    {
        if (nextEmptyEquipSlot > 0)
        {
            nextEmptyEquipSlot--;
            GameObject slot = equipSlots[nextEmptyEquipSlot];
            slot.GetComponent<Image>().sprite = null;
            slot.SetActive(false);
        }
    }

    private void InitItemSlot()
    {
        List<ItemData> currentInventory = InventoryManager.Instance.currentInventory;
        for (int i = 0; i < itemMaxSlot; i++)
        {
            GameObject slot = null;
            if (i < currentInventory.Count && currentInventory[i] != null)
            {
                Sprite icon = Resources.Load<Sprite>("Icons/Items/" + currentInventory[i].Icon);
                slot = Instantiate(basicIconPrefab, itemSlotContent);

                GameObject iconImageGo = new GameObject("IconImage");
                iconImageGo.transform.SetParent(slot.transform, false);
                iconImageGo.transform.SetAsLastSibling();

                Image image = iconImageGo.AddComponent<Image>();
                image.sprite = icon;

                RectTransform rect = image.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(iconImageSize, iconImageSize);
                rect.anchoredPosition = Vector2.zero;

                Button itemButton = slot.GetComponent<Button>();
                if (itemButton == null) { itemButton = slot.AddComponent<Button>(); }

                PopupController buttonController = slot.AddComponent<PopupController>();
                buttonController.button = itemButton;
                buttonController.itemData = currentInventory[i];

                nextEmptyItemSlot++;
            }
            else
            {
                slot = Instantiate(emptyIconPrefab, itemSlotContent);
            }
            slot.SetActive(true);
            itemSlots.Add(slot);
        }
    }

    private void InitEquipSlot()
    {
        List<ItemData> currentEquipment = InventoryManager.Instance.currentEquipment;
        for (int i = 0; i < equipMaxSlot; i++)
        {
            GameObject slot = null;
            if (i < currentEquipment.Count && currentEquipment[i] != null)
            {
                Sprite icon = Resources.Load<Sprite>("Icons/Items/" + currentEquipment[i].Icon);
                slot = Instantiate(basicIconPrefab, equipSlotContent);

                GameObject iconImageGo = new GameObject("IconImage");
                iconImageGo.transform.SetParent(slot.transform, false);
                iconImageGo.transform.SetAsLastSibling();

                Image image = iconImageGo.AddComponent<Image>();
                image.sprite = icon;

                RectTransform rect = image.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(iconImageSize, iconImageSize);
                rect.anchoredPosition = Vector2.zero;

                Button itemButton = slot.GetComponent<Button>();
                if (itemButton == null) { itemButton = slot.AddComponent<Button>(); }

                PopupController buttonController = slot.AddComponent<PopupController>();
                buttonController.button = itemButton;
                buttonController.itemData = currentEquipment[i];

                nextEmptyEquipSlot++;
            }
            else
            {
                slot = Instantiate(basicIconPrefab, equipSlotContent);
            }
            slot.SetActive(true);
            equipSlots.Add(slot);
        }
    }
}
