using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public GameObject normalIconPrefab;
    public GameObject rareIconPrefab;
    public GameObject epicIconPrefab;
    public GameObject emptyIconPrefab;
    public RectTransform inventoryContent;

    private List<GameObject> itemSlots = new List<GameObject>();
    private int nextEmptySlot = 0;
    private int MaxSlot = 64;
    private float iconImageSize = 128f;

    void Start()
    {
        InventoryManager.Instance.OnItemAdd += AddItemIcon;

        List<EquipmentData> currentInventory = InventoryManager.Instance.inventory;
        for (int i = 0; i < MaxSlot; i++)
        {
            GameObject slot = null;
            if (i < currentInventory.Count && currentInventory[i] != null)
            {
                Sprite icon = Resources.Load<Sprite>(currentInventory[i].Icon);
                slot = Instantiate(normalIconPrefab, inventoryContent);

                GameObject iconImageGO = new GameObject("IconImage");
                iconImageGO.transform.SetParent(slot.transform, false);

                Image image = iconImageGO.AddComponent<Image>();
                image.sprite = icon;

                RectTransform rect = image.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(iconImageSize, iconImageSize);
                rect.anchoredPosition = Vector2.zero;

                nextEmptySlot++;
            }
            else
            {
                slot = Instantiate(emptyIconPrefab, inventoryContent);
            }
            slot.SetActive(true);
            itemSlots.Add(slot);
        }
    }

    private void OnDestroy()
    {
        InventoryManager.Instance.OnItemAdd -= AddItemIcon;
    }

    private void AddItemIcon(EquipmentData itemData)
    {
        if (nextEmptySlot < itemSlots.Count)
        {
            GameObject slot = itemSlots[nextEmptySlot];
            Sprite itemSprite = Resources.Load<Sprite>(itemData.Icon);

            slot.GetComponent<Image>().sprite = itemSprite;
            slot.SetActive(true);
            nextEmptySlot++;
        }
        else
        {
            Debug.Log("Slot Full");
        }
    }
}
