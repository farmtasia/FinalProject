using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyPopup : BaseUI
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI amountText;
    public Slider amountSlider;
    public Button minusButton;
    public Button plusButton;
    public Button buyButton;
    public TextMeshProUGUI totalPriceText;

    private ItemSO currentItem;
    private int currentAmount;
    private int maxAmount;
    private int totalPrice;

    private PlayerGold playerGold; // PlayerGold 클래스 참조 추가
    public event Action<int> OnGoldUpdated; // 골드 업데이트 이벤트
    public event Action OnItemBuy; // 아이템 구매 이벤트

    public SEType buySoundEffect = SEType.GOLD; // 구매 시 효과음

    private void Awake()
    {
        minusButton.onClick.AddListener(DecreaseAmount);
        plusButton.onClick.AddListener(IncreaseAmount);
        buyButton.onClick.AddListener(BuyItem);
        amountSlider.onValueChanged.AddListener(OnSliderValueChanged);

        playerGold = GameManager.Instance.Player.GetComponent<PlayerGold>();

        gameObject.SetActive(false);
    }

    public void Setup(ItemSO item) // 팝업 초기화
    {
        currentItem = item;
        itemName.text = item.itemName;
        itemDescription.text = item.itemDescription;

        if (currentItem.itemType == EItemType.EQUIPABLE)
        {
            // Equipable 아이템의 경우 수량을 1로 고정
            currentAmount = 1;
            maxAmount = 1;
            amountSlider.gameObject.SetActive(false); // 슬라이더 비활성화
            minusButton.gameObject.SetActive(false); // 마이너스 버튼 비활성화
            plusButton.gameObject.SetActive(false); // 플러스 버튼 비활성화
        }
        else
        {
            int gold = playerGold.currentGold;
            maxAmount = Mathf.FloorToInt(gold / item.itemPrice); // 소수점 이하 값을 무시하고 정수로 변환 / 최대 구매 가능 수량 계산
            Debug.Log(gold);
            amountSlider.maxValue = maxAmount;
            amountSlider.value = 1;
            amountSlider.gameObject.SetActive(true); // 슬라이더 활성화
            minusButton.gameObject.SetActive(true); // 마이너스 버튼 활성화
            plusButton.gameObject.SetActive(true); // 플러스 버튼 활성화
        }

        UpdateAmount(currentAmount);
    }

    public void UpdateAmount(int amount)// 구매할 아이템의 수량 업데이트
    {
        currentAmount = amount;
        amountText.text = currentAmount.ToString() + " / " + maxAmount.ToString();
        totalPrice = currentItem.itemPrice * currentAmount;
        totalPriceText.text = totalPrice.ToString() + " G";
        buyButton.interactable = playerGold.currentGold >= totalPrice; // 골드가 충분한 경우에만 구매 버튼 활성화

        Debug.Log($"수량 업데이트 / 현재 수량: {currentAmount}, 총 가격: {totalPrice}");
    }

    public void DecreaseAmount() // 수량을 감소시키는 버튼 클릭
    {
        Debug.Log("수량 감소 클릭");
        if (currentAmount > 1)
        {
            currentAmount--;
            amountSlider.value = currentAmount;
            UpdateAmount(currentAmount);
        }
    }

    public void IncreaseAmount() // 수량을 증가시키는 버튼 클릭
    {
        Debug.Log("수량 증가 클릭");
        Debug.Log("현재 수량 : " + currentAmount);
        if (currentAmount < maxAmount)
        {
            currentAmount++;
            amountSlider.value = currentAmount;
            UpdateAmount(currentAmount);
        }
    }

    private void OnSliderValueChanged(float value)
    {
        int newAmount = Mathf.RoundToInt(value);
        UpdateAmount(newAmount);
    }

    public void BuyItem() // 구매 버튼 클릭 이벤트 핸들러. 골드가 충분하면 아이템을 구매하고, UI를 비활성화
    {
        Debug.Log("구매 버튼 클릭");

        if (amountSlider.value < 1 && currentItem.itemType != EItemType.EQUIPABLE)
        {
            return;
        }
        
        if (playerGold.currentGold >= totalPrice)
        {
            GameManager.Instance.Player.gold.SubtractGold(totalPrice);// 플레이어의 골드를 차감하고

            GameManager.Instance.Player.itemQuantity = currentAmount;
            for (int i = 0; i < currentAmount; i++)
            {
                GameManager.Instance.Player.itemdata = currentItem;// 아이템을 인벤토리에 추가
                GameManager.Instance.Player.addItem?.Invoke();
            }

            Debug.Log($"구매한 아이템 : {currentItem.itemName} x {currentAmount}");

            // 구매가 완료되었음을 이벤트로 알림
            OnGoldUpdated?.Invoke(playerGold.currentGold);
            OnItemBuy?.Invoke();

            // 효과음 재생
            SoundManager.Instance.PlayEffect(buySoundEffect);

            gameObject.SetActive(false);

        }
    }


}
