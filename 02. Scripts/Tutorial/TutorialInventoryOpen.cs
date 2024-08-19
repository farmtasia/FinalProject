using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialInventoryOpen : InventoryTutorialBase
{
    [SerializeField] private GameObject highlightImage; // 인벤토리 버튼을 강조할 이미지
    [SerializeField] private TutorialController controller;

    private Button inventoryButton;
    private int callCount = 0;  // 호출 횟수

    public override void Enter()
    {
        callCount++;

        highlightImage = FindObjectOfType<Canvas_Tutorial>().transform.GetChild(1).gameObject;
        highlightImage.SetActive(false);
        GameObject canvasMain = GameObject.Find("Canvas_Main(Clone)");


        // 호출 횟수에 따라 다른 메시지를 표시
        if (callCount == 1)
        {
            tutorialUI.SetTutorialMessage("이번에는 인벤토리를 \n열어볼까요?");
        }
        else if (callCount == 2)
        {
            tutorialUI.SetTutorialMessage("이제 물뿌리개를 장착해봅시다.");
        }

        if (canvasMain != null)
        {
            Button[] buttons = canvasMain.GetComponentsInChildren<Button>(true);
            foreach (Button button in buttons)
            {
                if (button.name == "InventoryBtn")
                {
                    inventoryButton = button.GetComponent<Button>();
                    highlightImage.transform.position = inventoryButton.transform.position; // 강조 이미지 위치를 인벤토리 버튼 위치로 이동
                    highlightImage.SetActive(true);
                    inventoryButton.onClick.AddListener(OnInventoryButtonClick);
                    break;
                }
            }
        }
    }

    private void OnInventoryButtonClick()
    {
        highlightImage.SetActive(false);

        // `TutorialController` 인스턴스를 사용하여 다음 튜토리얼로 진행
        if (tutorialUI != null)
        {
            tutorialUI.SetOrder(20);
            controller.SetNextTutorial();
        }
    }

    public override void Execute(TutorialController controller)
    {
        // 현재 상태에서는 버튼 클릭 이벤트가 처리되면 다음 튜토리얼로 이동하기 때문에 빈 메서드
    }

    public override void Exit()
    {
        if (inventoryButton != null)
        {
            inventoryButton.onClick.RemoveListener(OnInventoryButtonClick);
        }
    }
}