using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public event Action<ItemData> OnItemAdd;
    public List<ItemData> inventory = new List<ItemData>();

    public Transform buttonParent;
    public GameObject buttonPrefab;

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
    }

    public void AddItem(ItemData itemData)
    {
        inventory.Add(itemData);
        OnItemAdd?.Invoke(itemData);
    }

    public void RemoveItem(ItemData itemData)
    {
        inventory.Remove(itemData);
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
            newData.Rank          = (ItemRank)int.Parse(rowData[3]);
            newData.Attack        = float.Parse(rowData[4]);
            newData.HP            = float.Parse(rowData[5]);
            newData.Defense       = float.Parse(rowData[6]);
            newData.AttackSpeed   = float.Parse(rowData[7]);
            newData.ShotTypeValue = float.Parse(rowData[8]);
            newData.MoveSpeed     = float.Parse(rowData[9]);
            newData.Drop          = float.Parse(rowData[10]);
            newData.Icon          = rowData[11];

            inventory.Add(newData);
        }
        Debug.Log("Inventory Load");
        streamReader.Close();
    }

    public void SaveInventoryCSV(string filePath)
    {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("ID,Character,Name,Rank,Attack,HP,Defense,AttackSpeed,ShotTypeValue,MoveSpeed,Drop,Icon");

        foreach (ItemData item in inventory)
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
                $"{item.Icon}"
            );
        }
        Debug.Log("Inventory Save");
        File.WriteAllText(filePath, stringBuilder.ToString());
    }
}