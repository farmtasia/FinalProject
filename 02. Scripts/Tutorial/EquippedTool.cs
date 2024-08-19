using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class EquippedTool : InventoryTutorialBase
{
    [SerializeField] private GameObject highlightImage; // 강조 이미지
    [SerializeField] private TutorialController controller;
    private Button equipBtn;
    private Button invenCloseBtn;
    private int callCount = 0;  // 호출 횟수

    public override void Enter()
    {
        callCount++;

        highlightImage = FindObjectOfType<Canvas_Tutorial>().transform.GetChild(1).gameObject;
        highlightImage.SetActive(false);

        equipBtn = GameManager.Instance.Player.inventory.equipBtn.GetComponent<Button>();
        equipBtn.onClick.AddListener(OnEquipButtonClick);

        // 호출 횟수에 따라 메시지 설정
        if (callCount == 1)
        {
            tutorialUI.SetTutorialMessage("인벤토리에는 농사에 필요한 \n도구가 들어있답니다.");
            // 인벤토리 열리고 2초 후 첫 번째 슬롯 강조 시작
            StartCoroutine(HighlightFirstSlotCoroutine());
        }
        else if (callCount == 2)
        {
            tutorialUI.SetTutorialMessage("물뿌리개를 장착해봅시다.");
            HighlightSecondSlot();
        }

        GameObject inventory = GameObject.Find("Inventory(Clone)");

        if (inventory != null)
        {
            Button[] buttons = inventory.GetComponentsInChildren<Button>(true);
            foreach (Button button in buttons)
            {
                if (button.name == "Inven_CloseButton")
                {
                    invenCloseBtn = button.GetComponent<Button>();
                    invenCloseBtn.onClick.AddListener(OnCloseButtonClick);
                    invenCloseBtn.interactable = false; // 닫기 버튼 비활성화
                    break;
                }
            }
        }
    }

    private IEnumerator HighlightFirstSlotCoroutine()
    {
        yield return new WaitForSeconds(2f); // 2초 대기
        tutorialUI.SetTutorialMessage("삽 도구를 장착해볼까요?");

        HighlightFirstSlot();
    }

    private void HighlightFirstSlot()
    {
        // 첫 번째 슬롯을 강조
        var firstSlot = GameManager.Instance.Player.inventory.itemSlots[0]; // 첫 번째 슬롯

        if (firstSlot.item != null && firstSlot.item.name == "Shovel")
        {
            highlightImage.transform.position = firstSlot.transform.position;
            highlightImage.SetActive(true);
        }
    }

    private void HighlightSecondSlot()
    {
        // 두 번째 슬롯을 강조
        var secondSlot = GameManager.Instance.Player.inventory.itemSlots[1]; // 두 번째 슬롯

        if (secondSlot.item != null && secondSlot.item.name == "Watering")
        {
            highlightImage.transform.position = secondSlot.transform.position;
            highlightImage.SetActive(true);
        }
    }


    private void OnEquipButtonClick()
    {
        // 삽 또는 물뿌리개 장착 여부 확인
        if (GameManager.Instance.Player.equipment.curEquipTool != null)
        {
            // 첫 번째 단계 (삽 장착)
            if (callCount == 1)
            {
                // 삽 아이템 코드 확인
                if (GameManager.Instance.Player.equipment.curEquipTool.itemData.itemCode == 1102)
                {
                    tutorialUI.SetTutorialMessage("인벤토리를 닫고 \n밭으로 이동하세요.");
                    invenCloseBtn.interactable = true; // 닫기 버튼 활성화
                }
                else
                {
                    // 장착된 도구가 삽이 아닐 때
                    tutorialUI.SetTutorialMessage("삽을 장착해주세요!");
                    invenCloseBtn.interactable = false;
                }
            }
            // 두 번째 단계 (물뿌리개 장착)
            else if (callCount == 2)
            {
                // 물뿌리개 아이템 코드 확인
                if (GameManager.Instance.Player.equipment.curEquipTool.itemData.itemCode == 1101)
                {
                    tutorialUI.SetTutorialMessage("인벤토리를 닫고 \n밭에 물을 줘보세요.");
                    invenCloseBtn.interactable = true; // 닫기 버튼 활성화
                }
                else
                {
                    // 장착된 도구가 물뿌리개가 아닐 때
                    tutorialUI.SetTutorialMessage("물뿌리개를 장착해주세요!");
                    invenCloseBtn.interactable = false;
                }
            }
        }
        else
        {
            // 도구가 장착되어 있지 않을 때
            invenCloseBtn.interactable = false;
            tutorialUI.SetTutorialMessage("도구를 장착해주세요!");
        }
    }


    private void OnCloseButtonClick()
    {
        highlightImage.SetActive(false);

        // 다음 튜토리얼로 진행
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
        if (invenCloseBtn != null)
        {
            invenCloseBtn.onClick.RemoveListener(OnCloseButtonClick);
        }

        if (equipBtn != null)
        {
            equipBtn.onClick.RemoveListener(OnEquipButtonClick);
        }
    }
}
