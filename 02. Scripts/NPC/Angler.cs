using UnityEngine;

public class Angler : BaseNPC
{
    protected override void Awake()
    {
        NpcID = 1003;
        base.Awake();
    }

    protected override void InitializeNPCData(int npcID)
    {
        base.InitializeNPCData(NpcID); // NPC 데이터 로드
        Debug.Log($"InitializeNPCData 호출됨. {npcName}의 npcID: {npcID}");

        // 낚시꾼에 상호작용 이벤트 추가
        InteractionEvent anglerInteraction = gameObject.GetComponent<InteractionEvent>();
        if (anglerInteraction == null)
        {
            anglerInteraction = gameObject.AddComponent<InteractionEvent>();
        }
        //anglerInteraction.SetEventInfo(npcID, CurrentEventID); // 낚시꾼에 대한 이벤트 ID 설정

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
