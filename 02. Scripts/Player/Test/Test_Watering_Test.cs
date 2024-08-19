using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WateringItem", menuName = "Inventory/Watering")]
public class Test_Watering_Test : ItemSO
{
    // Watering 아이템에 필요한 기능
    [Header("Watering Info")]
    public string wateringFlavor; // 무슨맛 추가? 물뿌리개의 속성...?

    private void OnEnable()
    {
        itemType = EItemType.EQUIPABLE; // 물뿌리개는 장착형
    }
}
