using UnityEngine;

public class VillageChief : BaseNPC
{
    protected override void Awake()
    {
        NpcID = 1002;
        base.Awake();
    }

    protected override void InitializeNPCData(int npcID)
    {
        base.InitializeNPCData(NpcID); // NPC 데이터 로드
        //SetNPCData(1002, "임정화", "마을 이장", Resources.Load<Sprite>("VillageChiefImage"), 0f); // npc에서 로드를 직접 잘 쓰진 않음. 퍼블릭으로 꽂는게 낫다. 
        Debug.Log($"InitializeNPCData 호출됨. {npcName}의 npcID: {npcID}"); // npcSO를 만들거나 관련 정보만 갖고있는 제이슨 만들어서 쓰거나... / 리소스매니저에안에서 NPC이미지 최적화해서 불어올 방법을 구현하거나(시도 추천!!)

        // 마을 이장에 상호작용 이벤트 추가
        InteractionEvent villageChiefInteraction = gameObject.GetComponent<InteractionEvent>();
        if (villageChiefInteraction == null)
        {
            villageChiefInteraction = gameObject.AddComponent<InteractionEvent>();
        }
        //villageChiefInteraction.SetEventInfo(npcID, CurrentEventID); // 마을이장에 대한 이벤트 ID 설정
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