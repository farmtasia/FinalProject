using UnityEngine;
//using UnityEditor;
using System.Collections.Generic;
//using System.IO;

public class ItemSOCreator : MonoBehaviour
{
    //[MenuItem("Tools/Create ItemSO from CSV")]
    //public static void CreateItemSOFromCSV()
    //{
    //    string path = EditorUtility.OpenFilePanel("Select CSV", "", "csv");
    //    if (path.Length != 0)
    //    {
    //        var itemDataList = LoadCSV(path);

    //        foreach (var itemData in itemDataList)
    //        {
    //            ItemSO item = ScriptableObject.CreateInstance<ItemSO>();
    //            item.itemCode = int.Parse(itemData["itemCode"]);
    //            item.itemName = itemData["itemName"];
    //            item.itemDescription = itemData["itemDescription"];
    //            //item.itemIcon = LoadSprite(itemData["itemIconPath"]);
    //            item.itemPrice = string.IsNullOrEmpty(itemData["itemPrice"]) ? 0 : int.Parse(itemData["itemPrice"]);
    //            item.requiredLevel = int.Parse(itemData["requiredLevel"]);

    //            // DropPrefab는 생략
    //            item.itemType = (EItemType)System.Enum.Parse(typeof(EItemType), itemData["itemType"]);
    //            item.detailType = (EItemDetailType)System.Enum.Parse(typeof(EItemDetailType), itemData["detailType"]);
    //            //item.canEquip = bool.Parse(itemData["canEquip"]);
    //            //item.canConsum = bool.Parse(itemData["canConsum"]);
    //            //item.healEnergy = int.Parse(itemData["healEnergy"]);
    //            item.canStack = bool.Parse(itemData["canStack"]);
    //            item.maxStackAmount = int.Parse(itemData["maxStackAmount"]);

    //            //item.harvestType = (EHarvestType)System.Enum.Parse(typeof(EHarvestType), itemData["harvestType"]);
    //            //item.harvestAmount = int.Parse(itemData["harvestAmount"]);
    //            //item.harvestSecond = float.Parse(itemData["harvestSecond"]);

    //            // CSV에서 파일명 읽어오기
    //            string assetName = string.IsNullOrEmpty(itemData["fileName"]) ? item.itemName.Replace(" ", "_") + ".asset" : itemData["fileName"] + ".asset";
    //            string assetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine("Assets/Resources/Items", assetName));

    //            AssetDatabase.CreateAsset(item, assetPath);
    //        }

    //        AssetDatabase.SaveAssets();
    //        AssetDatabase.Refresh();
    //        Debug.Log("ItemSO creation from CSV completed");
    //    }
    //}

    //private static List<Dictionary<string, string>> LoadCSV(string filePath)
    //{
    //    var itemList = new List<Dictionary<string, string>>();

    //    string[] lines = File.ReadAllLines(filePath);
    //    string[] headers = lines[0].Split(',');

    //    for (int i = 1; i < lines.Length; i++)
    //    {
    //        string[] values = lines[i].Split(',');
    //        var itemData = new Dictionary<string, string>();

    //        for (int j = 0; j < headers.Length; j++)
    //        {
    //            itemData[headers[j]] = values[j];
    //        }

    //        itemList.Add(itemData);
    //    }

    //    return itemList;
    //}

    //private static Sprite LoadSprite(string path)
    //{
    //    return Resources.Load<Sprite>(path);
    //}
}