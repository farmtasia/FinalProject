using UnityEngine;

public class BaseNPC : MonoBehaviour
{
    private int npcID;
    public string npcName;
    public string npcJob;
    public Sprite npcImage;
    public float likeability = 0f; // 호감도

    public int CurrentEventID = 1; // 현재 이벤트 ID
    public bool HasMetPlayer = false;

    protected DialogueManager dialogueManager; // 상속받은 개별 NPC들이 dialogueManager에 접근할 수 있도록.

    public int NpcID
    {
        get { return npcID; }
        protected set { npcID = value; }
    }

    protected virtual void Awake()
    {
        dialogueManager = DialogueManager.Instance;
        // NPC 데이터 초기화
        InitializeNPCData(NpcID);
        Debug.Log($"Awake 호출됨. NPC 이름: {npcName}, NPC ID: {npcID}");
    }

    protected virtual void Start()
    {
        // NPC의 대화 데이터 로드
        LoadDialogue();
    }

    // NPC 데이터 초기화
    protected virtual void InitializeNPCData(int npcID)
    {
        NPCDefault npcData = DataManager.Instance.GetNPCData(npcID);
        Debug.Log($"InitializeNPCData 호출됨. npcID: {npcID}");
        if (npcData != null)
        {
            npcName = npcData.name;
            npcJob = npcData.job;
            npcImage = Resources.Load<Sprite>(npcData.imageResourcePath);
            likeability = npcData.initialLikeability;
            Debug.Log($"NPC 데이터 초기화 성공: {npcName}, {npcJob}");

            // 캐릭터를 새로 생성했을 때는 NPC 상호작용 기록이 없으므로 각 NPC 객체 생성 후 리스트, 딕셔너리에 추가
            if (!DataManager.Instance.npcDataDic.ContainsKey(npcID))
            {
                NPCData newNPCData = new NPCData(npcID, likeability, CurrentEventID, HasMetPlayer);
                DataManager.Instance.npcDataDic[newNPCData.npcID] = newNPCData;
                DataManager.Instance.curData.npcData.Add(newNPCData);
            }
        }
        else
        {
            Debug.LogError($"NPC 데이터 초기화 실패: {npcID}");
        }
    }

    // DialogueManager에서 대화 데이터를 가져오는 메서드
    protected virtual void LoadDialogue()
    {
        // DialogueManager에서 NPC의 대화 데이터 가져오기
        var dialogues = dialogueManager.GetDialogue(npcID, CurrentEventID);
        Debug.Log($"LoadDialogue 호출됨. npcID: {npcID}, CurrentEventID: {CurrentEventID}");
        if (dialogues != null)
        {
            // 대화 데이터 로드 되었을 때
            Debug.Log($"{npcName}의 대화 데이터가 로드되었습니다.");
        }
        else
        {
            Debug.LogWarning($"{npcName}의 대화 데이터를 찾을 수 없습니다.");
        }
    }

    // 대화 데이터를 다시 로드하는 메서드
    public void RefreshDialogue()
    {
        Debug.Log("다시 로드하는 대화의 현재 이벤트ID : " + CurrentEventID);
        LoadDialogue(); // 현재 이벤트 ID에 맞는 대화 데이터 다시 로드
    }

    // 호감도 증가 메서드
    public virtual void IncreaseLikeability(float amount)
    {
        DataManager.Instance.npcDataDic[npcID].likeability += amount;
        Debug.Log($"{npcName}의 호감도가 {amount}만큼 증가. 현재 호감도: {likeability}");
    }

    // 플레이어를 만났음을 설정하는 메서드
    public virtual void MeetPlayer()
    {
        DataManager.Instance.npcDataDic[npcID].hasMetPlayer = true;
        Debug.Log($"{npcName}를 만났습니다: {DataManager.Instance.npcDataDic[npcID].hasMetPlayer}");
    }
}
