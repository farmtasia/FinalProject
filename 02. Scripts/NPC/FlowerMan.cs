using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerMan : BaseNPC
{
    protected override void Awake()
    {
        NpcID = 1004;
        base.Awake();
    }

    protected override void InitializeNPCData(int npcID)
    {
        base.InitializeNPCData(NpcID); // NPC 데이터 로드
        Debug.Log($"InitializeNPCData 호출됨. {npcName}의 npcID: {npcID}");

        InteractionEvent flowerManInteraction = gameObject.GetComponent<InteractionEvent>();
        if (flowerManInteraction == null)
        {
            flowerManInteraction = gameObject.AddComponent<InteractionEvent>();
        }

    }


    public override void IncreaseLikeability(float amount)
    {
        base.IncreaseLikeability(amount);  // BaseNPC의 IncreaseLikeability 메서드 호출
    }

    // 플레이어를 만났음을 설정하는 메서드
    public override void MeetPlayer()
    {
        HasMetPlayer = true; // 첫만남용

        // 이벤트 ID 증가
        //CurrentEventID++;
    }
}
