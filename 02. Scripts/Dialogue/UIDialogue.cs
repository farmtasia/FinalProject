using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogue : PopupUI
{
    public GameObject dialoguePanel; // 대화창 패널
    public GameObject BGPanel; // 대화창BG 패널
    public GameObject choicePanel; // 선택지 패널

    public TextMeshProUGUI textDialogue; // 대화 내용 텍스트
    public TextMeshProUGUI textDialogueName; // 말하는 사람 이름 텍스트
    public Image speakerImage; // 말하는 사람 이미지
    public GameObject nextArrow; // 넥스트 이미지

    public Button[] choiceButtons; // 선택지 항목 텍스트 배열

    private DialogueManager dialogueManager;
    private ResourcesManager resourcesManager;

    private float typingSpeed = 0.1f; // 텍스트 타이핑 속도
    public bool isTypingEffect = false; // 텍스트 타이핑 중인지 여부

    private string fullDialogueText; // 전체 대사 텍스트
    private Coroutine typingCoroutine; // 현재 실행 중인 타이핑 코루틴
    public bool isWaitingForClick = false; // 클릭 대기 여부

    public int selectedChoiceIndex { get; private set; } = -1; // 선택된 인덱스

    private void Start()
    {
        HideDialogue(); // 시작 시 대화창 숨기기

        dialogueManager = FindObjectOfType<DialogueManager>();
        resourcesManager = ResourcesManager.Instance;

        // BGPanel 클릭 시 호출되는 메서드 설정
        Button bgButton = BGPanel.GetComponent<Button>();
        if (bgButton != null)
        {
            bgButton.onClick.RemoveAllListeners(); // 이전 리스너 제거
            bgButton.onClick.AddListener(OnBGPanelClicked);
        }

        if (nextArrow != null)
        {
            nextArrow.SetActive(false); // 넥스트 화살표 비활성화
        }
    }

    // 일반 대화
    public void ShowDialogue(string speaker, string dialogue, Sprite speakerSprite = null)
    {
        dialoguePanel.SetActive(true); // 대화창 패널 활성화
        BGPanel.SetActive(true); // 배경 패널 활성화
        textDialogueName.text = speaker; // 말하는 사람 이름 설정
        fullDialogueText = dialogue; // 전체 대사 저장

        if (isTypingEffect) // 타이핑 효과가 진행중이라면
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine); // 타이핑 효과를 멈추고
                typingCoroutine = null; // 나오던 거 지우고(안지우면 중복 표시 됨) / 타이핑 코루틴 참조 제거
            }
            textDialogue.text = fullDialogueText; // 전체 대사 표시
            isTypingEffect = false; // 타이핑 효과 비활성화
        }
        else
        {
            typingCoroutine = StartCoroutine(TypeDialogue(fullDialogueText)); // 타이핑 효과 시작
        }

        if (speakerSprite != null) // 스피커 이미지 있다면
        {
            speakerImage.sprite = speakerSprite; // 설정하고
            speakerImage.gameObject.SetActive(true); // 활성화
        }
        else
        {
            speakerImage.gameObject.SetActive(false); // 없다면 비활성화
        }

        choicePanel.SetActive(false); // 선택지 패널 비활성화
        nextArrow.SetActive(false); // 넥스트 버튼 비활성화
    }

    private IEnumerator TypeDialogue(string dialogue)
    {
        isTypingEffect = true;
        textDialogue.text = "";

        foreach (char letter in dialogue)
        {
            textDialogue.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTypingEffect = false; // 타이핑 종료
        isWaitingForClick = false;
        nextArrow.SetActive(true); // 타이핑 완료 시 넥스트 버튼 활성화(그런데 마지막 대사일 땐 비활성화 하고 싶음/보류)
    }

    // 선택지 있는 대화
    internal void ShowDialogue(string speaker, string dialogueText, string[] choices, Sprite speakerSprite = null)
    {
        dialoguePanel.SetActive(true);
        BGPanel.SetActive(true);
        textDialogueName.text = speaker;
        fullDialogueText = dialogueText;

        if (isTypingEffect)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }
            textDialogue.text = fullDialogueText;
            isTypingEffect = false;
        }
        else
        {
            typingCoroutine = StartCoroutine(TypeDialogue(fullDialogueText));
        }

        if (speakerSprite != null)
        {
            speakerImage.sprite = speakerSprite;
            speakerImage.gameObject.SetActive(true);
        }
        else
        {
            speakerImage.gameObject.SetActive(false);
        }


        // 선택지 패널 설정 부분
        ActivateChoiceButtons(choices); // 선택지 버튼 설정
        nextArrow.SetActive(false); // 넥스트 버튼 비활성화
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        selectedChoiceIndex = choiceIndex; // 선택된 인덱스 저장
        choicePanel.SetActive(false); // 선택지 패널만 숨기기

        Debug.Log("선택된 선택지 인덱스: " + choiceIndex);
        
        isWaitingForClick = true; // 선택지 선택 후 다음 대사 진행 준비
    }

    private void ActivateChoiceButtons(string[] choices) // 선택지 버튼 활성화하고 설정
    {
        choicePanel.SetActive(true); // 선택지 패널 활성화

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Length) // 선택지 배열 크기 내에서만
            {
                choiceButtons[i].gameObject.SetActive(true); // 선택지 버튼 활성화
                choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = choices[i]; // 선택지 텍스트 설정
                int choiceIndex = i; // 버튼의 인덱스 i 저장. 버튼 클릭 이벤트에 전달될 값
                choiceButtons[i].onClick.RemoveAllListeners(); // 기존에 등록된 이벤트 리스너 제거
                choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(choiceIndex)); // 선택지 선택 시 호출될 메서드 설정(선택된 인덱스 전달)
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }



    public void HideDialogue()
    {
        dialoguePanel.SetActive(false); // 대화창 패널 숨기기
        BGPanel.SetActive(false);
        choicePanel.SetActive(false); // 선택지 패널도 숨기기
    }

    // BGPanel 클릭 시 호출되는 메서드
    private void OnBGPanelClicked()
    {
        // 선택지 패널이 활성화되어 있을 때 배경 클릭 무시
        if (choicePanel.activeSelf)
        {
            return;
        }

        if (isTypingEffect)
        {
            // 타이핑 중이면 대사를 즉시 전체 표시
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }
            textDialogue.text = fullDialogueText;
            isTypingEffect = false;
            nextArrow.SetActive(true); // 넥스트 버튼 활성화 / 마지막 대사일 땐 비활성화 하고 싶음
        }
        else
        {
            // 타이핑이 끝났을 경우 클릭 대기 상태로 전환
            isWaitingForClick = true;
        }
    }

}
