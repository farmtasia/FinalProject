using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellPopup : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI amountText;
    public Slider amountSlider;
    public Button minusButton;
    public Button plusButton;
    public Button sellButton;
    public TextMeshProUGUI totalPriceText;
    public TextMeshProUGUI marketPriceText;

    private ItemSO currentItem;
    private int currentAmount;
    private int maxAmount;
    private int totalPrice;

    private Inventory inventory;

    private PlayerGold playerGold; // PlayerGold 클래스 참조 추가
    public event Action<int> OnGoldUpdated; // 골드 업데이트 이벤트
    public event Action OnItemSell; // 아이템 판매 이벤트

    public SEType saleSoundEffect = SEType.GOLD; // 판매 시 효과음


    private void Awake()
    {
        minusButton.onClick.AddListener(DecreaseAmount);
        plusButton.onClick.AddListener(IncreaseAmount);
        sellButton.onClick.AddListener(SellItem);
        amountSlider.onValueChanged.AddListener(OnSliderValueChanged);

        playerGold = GameManager.Instance.Player.GetComponent<PlayerGold>();

        gameObject.SetActive(false);
    }


     // SellPopup 활성화 시 초기화하는 메서드
    public void Setup(ItemSO item, Inventory _inventory)
    {
        currentItem = item;
        inventory = _inventory;
        itemName.text = item.itemName;
        itemDescription.text = item.itemDescription;

        // 최대 판매 가능 수량 계산
        int itemQuantity = GetItemQuantity(item.itemName);
        maxAmount = itemQuantity;
        amountSlider.maxValue = maxAmount;
        amountSlider.value = 1;
        UpdateAmount(1); // 초기화 시 UI 업데이트

    }

    // 수량 변경 시 호출되는 함수
    private void UpdateAmount(int amount)
    {
        currentAmount = amount;
        amountText.text = $"{currentAmount} / {maxAmount}";

        // 원래 가격의 60%를 계산하여 판매 가격 설정
        int originalPrice = currentItem.itemPrice;
        int marketPrice = Mathf.FloorToInt(originalPrice * 0.6f);
        marketPriceText.text = $"{marketPrice} G";

        totalPrice = marketPrice * currentAmount; // 아이템의 시장 가격을 사용하여 총 판매 가격 계산
        totalPriceText.text = $"{totalPrice} G";
        sellButton.interactable = currentAmount > 0; // 판매 버튼은 최소 1개 이상의 아이템을 선택해야 활성화
    }

    // Slider 값 변경 시 호출되는 함수
    private void OnSliderValueChanged(float value)
    {
        int newAmount = Mathf.RoundToInt(value); // Slider 값을 반올림하여 정수로 변환
        UpdateAmount(newAmount);
    }

    private void DecreaseAmount()
    {
        if (currentAmount > 1)
        {
            currentAmount--;
            amountSlider.value = currentAmount;
            UpdateAmount(currentAmount);
        }
    }

    private void IncreaseAmount()
    {
        if (currentAmount < maxAmount)
        {
            currentAmount++;
            amountSlider.value = currentAmount;
            UpdateAmount(currentAmount);
        }
    }

    private void SellItem()
    {
        if (currentAmount > 0)
        {
            int itemQuantity = GetItemQuantity(currentItem.itemName); // 인벤토리에서 아이템 개수 가져오기
            if (itemQuantity >= currentAmount)
            {
                // 원래 가격의 60%를 계산하여 판매 가격 설정
                int originalPrice = currentItem.itemPrice;
                int marketPrice = Mathf.FloorToInt(originalPrice * 0.6f);
                int sellPrice = marketPrice * currentAmount; // 판매 가격 계산

                playerGold.AddGold(sellPrice); // 플레이어에게 골드 추가

                for (int i = 0; i < currentAmount; i++)
                {
                    //inventory.RemovedItem(currentItem); // 인벤토리에서 아이템 제거
                    GameManager.Instance.Player.inventory.RemovedItem(currentItem);
                }


                // 판매 완료 후 골드 업데이트 이벤트 발생
                OnGoldUpdated?.Invoke(playerGold.currentGold);
                OnItemSell?.Invoke();

                // 효과음 재생
                SoundManager.Instance.PlayEffect(saleSoundEffect);

                gameObject.SetActive(false); // 팝업창 비활성화
                
            }
            else
            {
                Debug.LogWarning("인벤토리에 충분한 아이템이 없습니다.");
            }
        }
    }

    // 인벤토리에서 특정 아이템의 개수 가져오기
    private int GetItemQuantity(string itemName)
    {
        int quantity = 0;
        foreach (var slot in inventory.itemSlots)
        {
            if (slot.item != null && slot.item.itemName == itemName)
            {
                quantity += slot.quantity;
            }
        }
        return quantity;
    }
}
