using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CropHarvest : TutorialBase
{
    [SerializeField] private GameObject harvestArea;
    [SerializeField] private TutorialController controller;

    private Button interactionBtn;

    private int clickCount = 0; // 클릭 횟수를 카운트하는 변수

    public bool isTrigger { get; set; } = false;

    public override void Enter()
    {
        GameObject canvasMain = GameObject.Find("Canvas_Main(Clone)");
        if (canvasMain != null)
        {
            Button[] buttons = canvasMain.GetComponentsInChildren<Button>(true);
            foreach (Button button in buttons)
            {
                if (button.name == "InteractionBtn")
                {
                    interactionBtn = button.GetComponent<Button>();

                    interactionBtn.onClick.AddListener(OnInteractionButtonClick);
                    interactionBtn.interactable = false;
                    break;
                }
            }
        }

        tutorialUI.SetTutorialMessage("밭으로 이동해볼까요?\n지정된 자리로 가세요~\n안그러면 이곳에 영원히\n갇힌답니다.");
        GameManager.Instance.Player.topDownMovement.isMoving = true;
        harvestArea.SetActive(true);
    }

    public override void Execute(TutorialController controller)
    {
        if (isTrigger && clickCount == 0)
        {
            tutorialUI.SetTutorialMessage("오른쪽 아래에 보이는 \n상호작용 버튼을 누르면 \n땅을 팔 수 있습니다.");
            interactionBtn.interactable = true;
        }
    }

    public void OnPlayerEnterHarvestArea()
    {
        isTrigger = true;
    }

    private void OnInteractionButtonClick()
    {
        //interactionBtn.interactable = false;
        //tutorialUI.SetTutorialMessage("땅을 판 뒤 버튼을 한 번 더 누르면 인벤토리에 들어있는 씨앗이 심기게 됩니다.");
        //StartCoroutine(TimerCoroutine());

        clickCount++;

        if (clickCount == 1)
        {
            // 첫 번째 클릭
            GameManager.Instance.Player.topDownMovement.isMoving = false;
            Debug.Log("첫번째 클릭: 땅을 파는 중");
            interactionBtn.interactable = false;
            harvestArea.SetActive(false);
            tutorialUI.SetTutorialMessage("이렇게 땅이 파진답니다. \n씨앗을 심으려면 골고루 파줘야하니 \n한번 더 파주세요.");
            StartCoroutine(TimerCoroutine());
        }
        else if (clickCount == 2)
        {
            // 두 번째 클릭
            Debug.Log("두번째 클릭: 땅을 더 파는 중");
            interactionBtn.interactable = false;
            tutorialUI.SetTutorialMessage("이제 인벤토리에 있는 씨앗을 \n심어봅시다. 버튼을 한 번 더 \n눌러 씨앗을 심어주세요.");
            StartCoroutine(TimerCoroutine());
        }
        else if (clickCount == 3)
        {
            // 세 번째 클릭
            Debug.Log("세번째 클릭: 씨앗 심기");
            tutorialUI.SetTutorialMessage("씨앗이 심어졌습니다. \n이제 물뿌리개를 장착하고 \n물을 줘볼까요?");
            interactionBtn.interactable = false;
            controller.SetNextTutorial();
        }
    }

    private IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(1f);
        //tutorialUI.SetTutorialMessage("상호작용 버튼을 눌러보세요.");
        interactionBtn.interactable = true;
    }

    public override void Exit()
    {
        // 플레이어 이동 불가능
        GameManager.Instance.Player.topDownMovement.isMoving = false;

        //if (interactionBtn != null)
        //{
        //    interactionBtn.onClick.RemoveListener(OnInteractionButtonClick);
        //}
    }
}
