using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Watering_t : TutorialBase
{
    [SerializeField] private TutorialController controller;
    private Button interactionBtn;

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

        tutorialUI.SetTutorialMessage("밭에 물을 줘 봅시다.");
        interactionBtn.interactable = true;
    }

    public override void Execute(TutorialController controller)
    {
        
    }

    private void OnInteractionButtonClick()
    {
        interactionBtn.interactable = false;
        tutorialUI.SetTutorialMessage("이제 시간이 지나면 \n작물이 자라있을거랍니다.");
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(3f);
        tutorialUI.SetTutorialMessage("지금부터 본격적으로 \n팜타지아 생활을 시작해보세요!\n응원합니다~");

        // 문구 보여준 후 튜토리얼 완료
        yield return new WaitForSeconds(2f); // 추가로 2초 대기
        interactionBtn.interactable = true;
        interactionBtn.onClick.RemoveListener(OnInteractionButtonClick);
        GameManager.Instance.Player.topDownMovement.isMoving = true;
        controller.CompletedAllTutorials();
    }

    public override void Exit()
    {
        //if (interactionBtn != null)
        //{
        //    interactionBtn.onClick.RemoveListener(OnInteractionButtonClick);
        //}
    }
}
