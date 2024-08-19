using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetSeed : EquipTool
{
    //public GameObject seedDropPrefab; // 씨앗 심기 애니메이션 프리팹
    //public SeedItemSO seedData; // 씨앗의 정보

    //public override void UseTool()
    //{
    //    // 사용된 도구가 씨앗일 때, 상호작용된 오브젝트가 Grass2인지 확인하고 씨앗 심기
    //    var curInteractable = GameManager.Instance.Player.interaction.curInteractable;
    //    if (curInteractable is Grass2 grass && grass.CanPlantSeed())
    //    {
    //        // 씨앗 심기 애니메이션 프리팹 생성
    //        GameObject seedDrop = Instantiate(seedDropPrefab, grass.transform.position, Quaternion.identity);
    //        seedDrop.GetComponent<Seed>().seedData = seedData;

    //        // 씨앗 심기 동작
    //        grass.PlantSeed(seedData);

    //        // 인벤토리에서 씨앗 감소
    //        GameManager.Instance.Inventory.RemoveSeed(seedData);
    //    }
    //}
}
