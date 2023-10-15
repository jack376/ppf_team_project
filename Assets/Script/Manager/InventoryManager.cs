using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<EquipmentData> inventory = new List<EquipmentData>();

    public event Action<EquipmentData> OnItemAdd;

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
        LoadInventoryDataFromCSV("Assets/Resources/InventorySaveData.csv");
    }

    public void AddItem(EquipmentData equipmentData)
    {
        inventory.Add(equipmentData);
        OnItemAdd?.Invoke(equipmentData);
    }

    public void RemoveItem(EquipmentData equipmentData)
    {
        inventory.Remove(equipmentData);
    }

    public void LoadInventoryDataFromCSV(string filePath)
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

            EquipmentData newData = new EquipmentData();

            newData.ID                   = int.Parse(rowData[0]);
            newData.Name                 = rowData[1];
            newData.Character            = int.Parse(rowData[2]);
            newData.Rank                 = int.Parse(rowData[3]);

            newData.BasicAttack          = int.Parse(rowData[4]);
            newData.BasicDefence         = int.Parse(rowData[5]);
            newData.BasicHP              = int.Parse(rowData[6]);

            newData.EditionalAttack      = float.Parse(rowData[7]);
            newData.EditionalMaxHP       = float.Parse(rowData[8]);
            newData.EditionalAttackSpeed = float.Parse(rowData[9]);
            newData.EditionalSpeed       = float.Parse(rowData[10]);
            newData.EditionalDefence     = float.Parse(rowData[11]);

            newData.EnchantAble          = int.Parse(rowData[12]);
            newData.Enchant              = int.Parse(rowData[13]);
            newData.GoldNeeded           = int.Parse(rowData[14]);
            newData.EnchantProbability   = float.Parse(rowData[15]);
            newData.EnchantResult        = int.Parse(rowData[16]);

            newData.ComposeAble          = int.Parse(rowData[17]);
            newData.ComposeProbabilty    = int.Parse(rowData[18]);
            newData.ComposeResult        = int.Parse(rowData[19]);

            newData.SellPrice            = int.Parse(rowData[20]);
            newData.Icon                 = rowData[21];

            inventory.Add(newData);
        }
        Debug.Log("Load");
        streamReader.Close();
    }

    public void SaveInventoryDataToCSV(string filePath)
    {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("ID,Name,Character,Rank,BasicAttack,BasicDefence,BasicHP,EditionalAttack,EditionalMaxHP,EditionalAttackSpeed,EditionalSpeed,EditionalDefence,EnchantAble,Enchant,GoldNeeded,EnchantProbability,EnchantResult,ComposeAble,ComposeProbabilty,ComposeResult,SellPrice");

        foreach (var item in inventory)
        {
            stringBuilder.AppendLine
            (
                $"{item.ID}," +
                $"{item.Name}," +
                $"{item.Character}," +
                $"{item.Rank}," +
                $"{item.BasicAttack}," +
                $"{item.BasicDefence}," +
                $"{item.BasicHP}," +
                $"{item.EditionalAttack}," +
                $"{item.EditionalMaxHP}," +
                $"{item.EditionalAttackSpeed}," +
                $"{item.EditionalSpeed}," +
                $"{item.EditionalDefence}," +
                $"{item.EnchantAble}," +
                $"{item.Enchant}," +
                $"{item.GoldNeeded}," +
                $"{item.EnchantProbability}," +
                $"{item.EnchantResult}," +
                $"{item.ComposeAble}," +
                $"{item.ComposeProbabilty}," +
                $"{item.ComposeResult}," +
                $"{item.SellPrice}," +
                $"{item.Icon}"
            );
        }
        Debug.Log("Save");
        File.WriteAllText(filePath, stringBuilder.ToString());
    }
}