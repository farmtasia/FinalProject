using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour, IInteractable
{
    private DialogueManager dialogueManager;
    private UIDialogue uiDialogue;
    private BaseNPC npc;

    public float autoDialogueInterval = 3f; // 자동 대사 출력 간격
    public float autoModeTimeout = 2f; // 자동 모드로 전환되는 시간

    private bool isAutoMode = true; // 자동 모드 여부
    private float lastClickTime; // 마지막 클릭 시간

    public Sprite playerImage; // 플레이어 이미지(임시)
    private string playerName;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        uiDialogue = FindObjectOfType<UIDialogue>();
        npc = GetComponent<BaseNPC>();
        playerName = DialogueManager.Instance.playerName;
    }

    public string GetInteractPrompt()
    {
        //return "NPC와 대화하기";
        return $"{npc.npcJob} {npc.npcName}와(과) 대화하기";
    }

    public void OnInteract()
    {
        StartDialogue();
    }

    private void StartDialogue()
    {
        List<Dialogue> dialogues = dialogueManager.GetDialogue(npc.NpcID, DataManager.Instance.npcDataDic[npc.NpcID].currentEventID);
        GameManager.Instance.Player.topDownMovement.HoldOnMoveSpeed(0f); // 플레이어 움직임 속도 0으로 설정(움직이지 못함)

        // 가져온 대화 데이터가 유효하고 대화가 하나 이상 있을 경우
        if (dialogues != null && dialogues.Count > 0)
        {
            StartCoroutine(ShowDialogueCoroutine(dialogues));
        }
    }

    private IEnumerator ShowDialogueCoroutine(List<Dialogue> dialogues)
    {
        Debug.Log("상호작용이벤트 진입 : " + DataManager.Instance.npcDataDic[npc.NpcID].currentEventID);

        // 기본 이벤트 ID 저장 (이벤트가 끝난 후 다시 2번으로 되돌리기 위함/2까지 있기 때문에...^^)
        int baseEventID = 2;

        // 대화 데이터 리스트에서 순서대로 대화를 가져와 처리
        for (int i = 0; i < dialogues.Count; i++)
        {
            var dialogue = dialogues[i]; // 현재 대화 데이터

            Sprite npcSprite = npc.npcImage; // NPC의 이미지 가져오기

            // 화자가 플레이어인지 확인
            Sprite displayImage = dialogue.Speaker == playerName ? playerImage : npcSprite;

            // 대화 타입이 선택지 발생이라면
            if (dialogue.Type == DialogueType.ChoiceOccur)
            {
                uiDialogue.ShowDialogue(dialogue.Speaker, dialogue.DialogueText, displayImage); // 대사 화면 표시
                yield return new WaitUntil(() => !uiDialogue.isTypingEffect); // 타이핑 효과 끝날때까지 대기

                // 다음 대화가 있고 타입이 선택지라면
                if (i + 1 < dialogues.Count && dialogues[i + 1].Type == DialogueType.ChoiceList) // -> 후속 대화가 선택지 대사일 때의 처리 담당
                {
                    var choiceDialogue = dialogues[i + 1]; // 선택지대사 데이터 가져오기
                    uiDialogue.ShowDialogue(choiceDialogue.Speaker, choiceDialogue.DialogueText, choiceDialogue.Choices, displayImage); // 선택지 대사 표시
                    yield return new WaitUntil(() => !uiDialogue.isTypingEffect); // 타이핑 효과 끝날때까지 대기

                    yield return new WaitUntil(() => uiDialogue.isWaitingForClick); // 사용자가 선택할 때까지 대기
                    uiDialogue.isWaitingForClick = false; // 클릭 대기 상태 해제

                    // 선택지 클릭 시 이벤트 ID를 업데이트하고 대화를 계속 진행
                    int choiceIndex = uiDialogue.selectedChoiceIndex; // 선택된 인덱스 가져오기
                    DataManager.Instance.npcDataDic[npc.NpcID].currentEventID += (choiceIndex + 1); // 선택지에 따라 이벤트 ID 증가
                    npc.RefreshDialogue(); // 대화 데이터 갱신
                    StartDialogue(); // 대화 재시작
                    yield break; // 선택지 선택 후 반복문 종료
                }
            }
            else
            {
                // 대화 타입이 선택지라면
                if (dialogue.Type == DialogueType.ChoiceList) // -> 현재 대화가 선택지일 때 처리 담당 
                {
                    uiDialogue.ShowDialogue(dialogue.Speaker, dialogue.DialogueText, dialogue.Choices, displayImage); // 화면에 대사 표시
                    yield return new WaitUntil(() => !uiDialogue.isTypingEffect); // 타이핑 효과 끝날때까지 대기

                    yield return new WaitUntil(() => uiDialogue.isWaitingForClick); // 사용자가 선택할 때까지 대기
                    uiDialogue.isWaitingForClick = false; // 클릭 대기 상태 해제
                }
                else // 대화 타입이 일반대화라면
                {
                    uiDialogue.ShowDialogue(dialogue.Speaker, dialogue.DialogueText, displayImage); // 화면에 대사 표시
                    yield return new WaitUntil(() => !uiDialogue.isTypingEffect); // 타이핑 효과 끝날때까지 대기
                }
            }

            // 자동 모드 여부에 따라 대기 시간 설정
            float waitTime = isAutoMode ? autoDialogueInterval : autoModeTimeout;
            float startTime = Time.time; // 대기 시작 시간 기록

            // 클릭 이벤트 대기
            while (Time.time - startTime < waitTime)
            {
                // 클릭 대기 상태인지 확인
                if (uiDialogue.isWaitingForClick)
                {
                    uiDialogue.isWaitingForClick = false; // 클릭 대기 상태 해제
                    lastClickTime = Time.time; // 클릭 시간 기록
                    isAutoMode = false; // 자동 모드 비활성화
                    break; // 클릭이 발생했으므로 대기 루프 탈출
                }
                yield return null;
            }

            // 클릭 대기 상태가 아니라면 자동 모드로 전환 여부 결정
            if (!uiDialogue.isWaitingForClick)
            {
                isAutoMode = Time.time - lastClickTime > autoModeTimeout;
            }

            // 대화가 끝났다면
            if (dialogue.Type == DialogueType.EndDialogue)
            {
                //npc.CurrentEventID++; // 이벤트 ID 증가
                DataManager.Instance.npcDataDic[npc.NpcID].currentEventID = baseEventID; // 이벤트 ID를 2로 리셋
                npc.RefreshDialogue(); // 대화 데이터 갱신
            }
        }

        // 모든 대화가 끝나면 UI 숨기기
        uiDialogue.HideDialogue();
        GameManager.Instance.Player.topDownMovement.ResetMoveSpeed(); // 움직임 재개
        npc.MeetPlayer();
        Debug.Log("상호작용이벤트 끝나고 : " + DataManager.Instance.npcDataDic[npc.NpcID].currentEventID);
    }
}
