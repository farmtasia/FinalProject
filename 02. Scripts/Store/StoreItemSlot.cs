using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemSlot : MonoBehaviour
{
    public ItemSO item;
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public Button button;
    public Image overlay; // 잠겼을 때 슬롯을 어둡게 만들기 위한 오버레이 이미지
    public TextMeshProUGUI requiredLevelText; // 해금할 수 있는 레벨

    private UIStore store; // UIStore에 대한 참조

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    public void SetSlot(ItemSO newItem, int playerLevel, int playerGold, UIStore storeRef)
    {
        store = storeRef;
        item = newItem;
        icon.sprite = item.itemIcon;
        nameText.text = item.itemName;
        priceText.text = item.itemPrice.ToString() + " G";
        // 아이콘 이미지의 크기를 원본 크기로 설정
        icon.SetNativeSize();

         //해당 아이템이 Equipable이고, 이미 인벤토리에 있다면
        if (item.itemType == EItemType.EQUIPABLE && GameManager.Instance.Player.inventory.CheckItemType(item))
        {
            button.interactable = false; // 이미 보유 중인 경우 버튼 비활성화
            overlay.gameObject.SetActive(true);  // 오버레이 활성화
            requiredLevelText.text = "이미 보유 중";  // 텍스트 설정
        }
        else
        {
            if (item.itemType == EItemType.EQUIPABLE)// Equipable 아이템인 경우
            {
                if (playerLevel >= item.requiredLevel) // 플레이어 레벨이 해금이 될 레벨이라면,
                {
                    if (playerGold >= item.itemPrice) // 플레이어 골드가 아이템 가격보다 많거나 같다면
                    {
                        button.interactable = true;
                        overlay.gameObject.SetActive(false); // 오버레이 비활성화
                        requiredLevelText.text = ""; // 레벨 텍스트 비움
                    }
                    else // 플레이어 골드가 부족하다면
                    {
                        button.interactable = false;
                        overlay.gameObject.SetActive(true); // 오버레이 활성화
                        requiredLevelText.text = "골드 부족"; // 골드 부족 텍스트 표시
                    }
                }
                else // 플레이어 레벨이 부족하다면,
                {
                    button.interactable = false;
                    overlay.gameObject.SetActive(true);  // 오버레이 활성화
                    requiredLevelText.text = item.requiredLevel + " 레벨 시 구매 가능";  // 레벨 텍스트 표시
                }
            }
            else
            {
                if (playerGold >= item.itemPrice) // 플레이어 골드가 아이템 가격보다 많거나 같다면
                {
                    button.interactable = true;
                    overlay.gameObject.SetActive(false); // 오버레이 비활성화
                    requiredLevelText.text = ""; // 레벨 텍스트 비움
                }
                else // 플레이어 골드가 부족하다면
                {
                    button.interactable = false;
                    overlay.gameObject.SetActive(true); // 오버레이 활성화
                    requiredLevelText.text = "골드 부족"; // 골드 부족 텍스트 표시
                }
            }
        }

    }

    public void OnClick()
    {
        if (button.interactable)
        {
            // 상점 슬롯 클릭 시 BuyPopup을 띄움
            Debug.Log("상점 아이템 클릭");
            store.OpenBuyPopup(item);
        }
    }
}
