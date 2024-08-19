using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// 다이얼로그타입 열거형 추가
public enum DialogueType
{
    CommonDialogue, // 일반 대화
    EndDialogue, // 대화 종료
    ChoiceOccur, // 선택지 발생
    ChoiceList // 선택지
}
// 새 CSV 파일 데이터 딕셔너리에 넣기

public class DialogueManager : Singleton<DialogueManager>
{
    // 대화 데이터를 저장할 딕셔너리
    //private Dictionary<int, Dictionary<string, Dialogue>> dialogueDictionary = new Dictionary<int, Dictionary<string, Dialogue>>();
    //private Dictionary<int, List<Dialogue>> dialogueDictionary = new Dictionary<int, List<Dialogue>>();

    // NPC별 딕셔너리
    private Dictionary<int, List<Dialogue>> storeKeeperDialogueDictionary = new Dictionary<int, List<Dialogue>>();
    private Dictionary<int, List<Dialogue>> villageChiefDialogueDictionary = new Dictionary<int, List<Dialogue>>();
    private Dictionary<int, List<Dialogue>> anglerDialogueDictionary = new Dictionary<int, List<Dialogue>>();
    private Dictionary<int, List<Dialogue>> flowerManDialogueDictionary = new Dictionary<int, List<Dialogue>>();
    private Dictionary<int, List<Dialogue>> coffeeManDialogueDictionary = new Dictionary<int, List<Dialogue>>();

    // 각 NPC의 상태를 관리할 딕셔너리
    private Dictionary<int, BaseNPC> npcDictionary = new Dictionary<int, BaseNPC>();

    // 플레이어 이름
    public string playerName;

    protected override void Awake()
    {
        base.Awake();
        playerName = DataManager.Instance.curData.nameData.userName;
        LoadDialogue("npc_dialogue");
    }

    //private void Start()
    //{
        // 게임 시작 시 대화 데이터 로드
    //    LoadDialogue("npc_dialogue");
    //}

    // Resources 폴더에서 대화 데이터를 로드하여 Dictionary에 저장하는 함수
    private void LoadDialogue(string fileName)
    {
        // Resources 폴더에서 파일을 로드
        TextAsset dialogueData = Resources.Load<TextAsset>(fileName);
        if (dialogueData == null)
        {
            // 파일 로드 실패 시 에러 메시지 출력 후 함수 종료
            Debug.LogError($"대화 데이터 파일 로드하는 데 실패 : {fileName}");
            return;
        }

        // 파일에서 읽어온 텍스트를 줄바꿈 문자를 기준으로 분리하여 배열로 저장
        string[] data = Regex.Split(dialogueData.text, "\n|\r\n|\r");

        //string[] data = dialogueData.text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        // 각 줄을 처리하여 대화 데이터 딕셔너리에 저장
        for (int i = 1; i < data.Length; i++)
        {
            // 쉼표를 기준으로 데이터 분리
            string[] row = data[i].Split(new char[] { ',' });


            // 각 필드에서 데이터를 추출하고 공백 제거
            int npcID = int.Parse(row[0].Trim()); // NPC ID
            int eventID = int.Parse(row[1].Trim()); // 이벤트 ID
            string speaker = row[2].Trim(); // 말하는 사람
            string dialogueText = row[3].Replace("^", ","); // 대화 내용에서 '^'를 ','로 변환
            string choice1 = row.Length > 4 ? row[4].Trim() : ""; // 선택지 1
            string choice2 = row.Length > 5 ? row[5].Trim() : ""; // 선택지 2
            DialogueType type = (DialogueType)int.Parse(row[6].Trim()); // 대화 유형

            // 말하는 사람이 "주인공"이면 플레이어의 이름으로 변경
            if (speaker == "주인공")
            {
                speaker = playerName;
            }

            // 대화 내용에서 '주인공' 단어를 플레이어의 이름으로 변경
            dialogueText = dialogueText.Replace("주인공", playerName);

            // 대화 객체 생성
            Dialogue dialogue = new Dialogue(speaker, dialogueText, type, new string[] { choice1, choice2 });

            // 현재 상호작용하는 NPC의 대화 데이터를 저장하는 딕셔너리
            // 나중에 NPCID에 따라 상점주인, 마을이장, 낚시꾼 등 각기 다른 대화 딕셔너리 중 하나를 가리킬 것
            Dictionary<int, List<Dialogue>> currentNPCDictionary = null;

            // NPC ID에 따라 적절한 대화 딕셔너리 선택
            switch (npcID)
            {
                case 1001:
                    currentNPCDictionary = storeKeeperDialogueDictionary;
                    break;
                case 1002:
                    currentNPCDictionary = villageChiefDialogueDictionary;
                    break;
                case 1003:
                    currentNPCDictionary = anglerDialogueDictionary;
                    break;
                case 1004:
                    currentNPCDictionary = flowerManDialogueDictionary;
                    break;
                case 1005:
                    currentNPCDictionary = coffeeManDialogueDictionary;
                    break;
                default:
                    break;
            }

            // 이벤트 ID에 대한 리스트가 없으면 새로 생성
            if (!currentNPCDictionary.ContainsKey(eventID)) // 현재 NPC 딕셔너리에 해당 이벤트 ID가 있는지 확인
            {
                // 해당 이벤트 ID를 사용하는 대화 데이터를 저장하기 위해 필요
                currentNPCDictionary[eventID] = new List<Dialogue>();
            }

            // 대화 데이터 추가
            currentNPCDictionary[eventID].Add(dialogue); // 해당 이벤트 ID에 대응하는 대화 데이터가 순서대로 저장
        }

        Debug.Log("대화 데이터 로드 완료");

    }


    public List<Dialogue> GetDialogue(int npcID, int eventID)
    {
        Debug.Log("대화매니저 : " + npcID + eventID);
        // 대화 데이터를 저장하는 딕셔너리 선택
        Dictionary<int, List<Dialogue>> currentNPCDictionary = null;

        switch (npcID)
        {
            case 1001:
                currentNPCDictionary = storeKeeperDialogueDictionary;
                break;
            case 1002:
                currentNPCDictionary = villageChiefDialogueDictionary;
                break;
            case 1003:
                currentNPCDictionary = anglerDialogueDictionary;
                break;
            case 1004:
                currentNPCDictionary = flowerManDialogueDictionary;
                break;
            case 1005:
                currentNPCDictionary = coffeeManDialogueDictionary;
                break;
            default:
                Debug.LogWarning($"NPC ID {npcID} 데이터 없음");
                return null;
        }

        // 선택된 딕셔너리에서 eventID에 해당하는 대화 리스트 가져오기
        if (currentNPCDictionary.TryGetValue(eventID, out List<Dialogue> dialogueList))
        {
            return dialogueList; // eventID에 해당하는 대화 리스트 반환
        }
        else
        {
            Debug.LogWarning($"NPC ID {npcID}, Event ID {eventID}에 대한 대화 데이터가 없음");
            return null; // eventID에 해당하는 대화 데이터가 없을 경우 null 반환
        }
    }

}
