using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class EquipmentManager : MonoBehaviour
{
    public List<EquipmentData> equipmentList = new List<EquipmentData>();

    void Start()
    {
        LoadEquipmentDataFromCSV("Assets/Resources/equipment.csv");
    }

    void LoadEquipmentDataFromCSV(string filePath)
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

            newData.ID        = int.Parse(rowData[0]);
            newData.Name      = rowData[1];
            newData.Rank      = int.Parse(rowData[2]);
            newData.Character = int.Parse(rowData[3]);

            newData.BasicAttack  = int.Parse(rowData[4]);
            newData.BasicHP      = int.Parse(rowData[5]);
            newData.BasicDefence = int.Parse(rowData[6]);

            newData.EditionalAttack      = float.Parse(rowData[7]);
            newData.EditionalMaxHP       = float.Parse(rowData[8]);
            newData.EditionalAttackSpeed = float.Parse(rowData[9]);
            newData.EditionalSpeed       = float.Parse(rowData[10]);
            newData.EditionalDefence     = float.Parse(rowData[11]);

            newData.Enchant              = int.Parse(rowData[12]);
            newData.EnchantAble          = int.Parse(rowData[13]);
            newData.EnchantProbability   = int.Parse(rowData[14]);
            newData.EnchantResult        = int.Parse(rowData[15]);
            newData.GoldNeeded           = int.Parse(rowData[16]);

            newData.ComposeAble          = int.Parse(rowData[17]);
            newData.ComposeProbabilty    = int.Parse(rowData[18]);
            newData.ComposeResult        = int.Parse(rowData[19]);

            newData.SellPrice = int.Parse(rowData[20]);

            equipmentList.Add(newData);
        }

        streamReader.Close();
    }

    void SaveEquipmentDataToCSV(string filePath)
    {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("ID,Name,Rank,Character,BasicAttack,BasicHP,BasicDefence,EditionalAttack,EditionalMaxHP,EditionalAttackSpeed,EditionalSpeed,EditionalDefence,Enchant,EnchantAble,EnchantProbability,EnchantResult,GoldNeeded,ComposeAble,ComposeProbabilty,ComposeResult,SellPrice");

        foreach (var equipment in equipmentList)
        {
            stringBuilder.AppendLine
            (
                $"{equipment.ID}," +
                $"{equipment.Name}," +
                $"{equipment.Rank}," +
                $"{equipment.Character}," +
                $"{equipment.BasicAttack}," +
                $"{equipment.BasicHP}," +
                $"{equipment.BasicDefence}," +
                $"{equipment.EditionalAttack}," +
                $"{equipment.EditionalMaxHP}," +
                $"{equipment.EditionalAttackSpeed}," +
                $"{equipment.EditionalSpeed}," +
                $"{equipment.EditionalDefence}," +
                $"{equipment.Enchant}," +
                $"{equipment.EnchantAble}," +
                $"{equipment.EnchantProbability}," +
                $"{equipment.EnchantResult}," +
                $"{equipment.GoldNeeded}," +
                $"{equipment.ComposeAble}," +
                $"{equipment.ComposeProbabilty}," +
                $"{equipment.ComposeResult}," +
                $"{equipment.SellPrice}"
            );
        }

        File.WriteAllText(filePath, stringBuilder.ToString());
    }
}
