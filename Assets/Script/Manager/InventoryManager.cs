using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public event Action<ItemData> OnItemAdd;
    public event Action<ItemData> OnItemRemove;

    public event Action<ItemData> OnEquipAdd;
    public event Action<ItemData> OnEquipRemove;

    public List<ItemData> currentInventory = new List<ItemData>();
    public List<ItemData> currentEquipment = new List<ItemData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadInventoryCSV("Assets/Resources/InventorySaveData.csv");
        LoadEquipmentCSV("Assets/Resources/EquipmentSaveData.csv");
    }

    public void AddItem(ItemData itemData)
    {
        currentInventory.Add(itemData);
        OnItemAdd?.Invoke(itemData);
    }

    public void RemoveItem(ItemData itemData)
    {
        currentInventory.Remove(itemData);
        OnItemRemove?.Invoke(itemData);
    }

    public void EquipItem(ItemData itemData)
    {
        currentEquipment.Add(itemData);
        OnEquipAdd?.Invoke(itemData);
    }

    public void UnequipItem(ItemData itemData)
    {
        currentEquipment.Remove(itemData);
        OnEquipRemove?.Invoke(itemData);
    }

    public void LoadInventoryCSV(string filePath)
    {
        StreamReader streamReader = new StreamReader(filePath, Encoding.Default);

        bool isFirstRow = true;
        while (!streamReader.EndOfStream)
        {
            string line = streamReader.ReadLine();

            if (isFirstRow)
            {
                isFirstRow = false;
                continue;
            }

            string[] rowData = line.Split(',');

            ItemData newData = new ItemData();

            newData.ID            = int.Parse(rowData[0]);
            newData.Character     = int.Parse(rowData[1]);
            newData.Name          = rowData[2];
            newData.Rank          = rowData[3];
            newData.Attack        = float.Parse(rowData[4]);
            newData.HP            = float.Parse(rowData[5]);
            newData.Defense       = float.Parse(rowData[6]);
            newData.AttackSpeed   = float.Parse(rowData[7]);
            newData.ShotTypeValue = float.Parse(rowData[8]);
            newData.MoveSpeed     = float.Parse(rowData[9]);
            newData.Drop          = float.Parse(rowData[10]);
            newData.Icon          = rowData[11];
            newData.IsEquipped    = bool.Parse(rowData[12]);
            newData.Type          = int.Parse(rowData[13]);

            currentInventory.Add(newData);
        }
        Debug.Log("Inventory Load");
        streamReader.Close();
    }

    public void LoadEquipmentCSV(string filePath)
    {
        StreamReader streamReader = new StreamReader(filePath, Encoding.Default);

        bool isFirstRow = true;
        while (!streamReader.EndOfStream)
        {
            string line = streamReader.ReadLine();

            if (isFirstRow)
            {
                isFirstRow = false;
                continue;
            }

            string[] rowData = line.Split(',');

            ItemData newData = new ItemData();

            newData.ID            = int.Parse(rowData[0]);
            newData.Character     = int.Parse(rowData[1]);
            newData.Name          = rowData[2];
            newData.Rank          = rowData[3];
            newData.Attack        = float.Parse(rowData[4]);
            newData.HP            = float.Parse(rowData[5]);
            newData.Defense       = float.Parse(rowData[6]);
            newData.AttackSpeed   = float.Parse(rowData[7]);
            newData.ShotTypeValue = float.Parse(rowData[8]);
            newData.MoveSpeed     = float.Parse(rowData[9]);
            newData.Drop          = float.Parse(rowData[10]);
            newData.Icon          = rowData[11];
            newData.IsEquipped    = bool.Parse(rowData[12]);
            newData.Type          = int.Parse(rowData[13]);

            currentEquipment.Add(newData);
        }
        Debug.Log("Equipment Load");
        streamReader.Close();
    }

    public void SaveInventoryCSV(string filePath)
    {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("ID,Character,Name,Rank,Attack,HP,Defense,AttackSpeed,ShotTypeValue,MoveSpeed,Drop,Icon,IsEquipped");

        foreach (ItemData item in currentInventory)
        {
            stringBuilder.AppendLine
            (
                $"{item.ID}," +
                $"{item.Character}," +
                $"{item.Name}," +
                $"{item.Rank}," +
                $"{item.Attack}," +
                $"{item.HP}," +
                $"{item.Defense}," +
                $"{item.AttackSpeed}," +
                $"{item.ShotTypeValue}," +
                $"{item.MoveSpeed}," +
                $"{item.Drop}," +
                $"{item.Icon}," +
                $"{item.IsEquipped}," +
                $"{item.Type}"
            );
        }
        Debug.Log("Inventory Save");
        File.WriteAllText(filePath, stringBuilder.ToString());
    }

    public void SaveEquipmentCSV(string filePath)
    {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("ID,Character,Name,Rank,Attack,HP,Defense,AttackSpeed,ShotTypeValue,MoveSpeed,Drop,Icon,IsEquipped");

        foreach (ItemData item in currentEquipment)
        {
            stringBuilder.AppendLine
            (
                $"{item.ID}," +
                $"{item.Character}," +
                $"{item.Name}," +
                $"{item.Rank}," +
                $"{item.Attack}," +
                $"{item.HP}," +
                $"{item.Defense}," +
                $"{item.AttackSpeed}," +
                $"{item.ShotTypeValue}," +
                $"{item.MoveSpeed}," +
                $"{item.Drop}," +
                $"{item.Icon}," +
                $"{item.IsEquipped}," +
                $"{item.Type}"
            );
        }
        Debug.Log("Equipment Save");
        File.WriteAllText(filePath, stringBuilder.ToString());
    }
}