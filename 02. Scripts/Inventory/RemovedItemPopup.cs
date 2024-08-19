using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RemovedItemPopup : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI amountText;
    public Slider amountSlider;
    public Button decreaseButton;
    public Button increaseButton;
    public Button deleteButton;
    public TextMeshProUGUI totalAmountText;

    private ItemSlot removedSlot;
    private Inventory inventory;
    private int currentAmount; // 현재 개수
    private int maxAmount; // 해당 아이템 슬롯의 quantity;

    private void Start()
    {
        inventory = GameManager.Instance.Player.inventory;
        gameObject.SetActive(false);
    }

    public void RemovedItemPopupSet(ItemSlot itemSlot)
    {
        removedSlot = itemSlot;
        itemNameText.text = removedSlot.item.itemName;
        maxAmount = removedSlot.quantity;
        amountSlider.maxValue = maxAmount;
        amountSlider.value = 1;
        UpdateRemovedItemAmount(1);
    }

    public void OnDecreaseAmount()
    {
        if (currentAmount > 1)
        {
            currentAmount--;
            amountSlider.value = currentAmount;
            UpdateRemovedItemAmount(currentAmount);
        }
    }

    public void OnIncreaseAmount()
    {
        if (currentAmount < maxAmount)
        {
            currentAmount++;
            amountSlider.value = currentAmount;
            UpdateRemovedItemAmount(currentAmount);
        }
    }

    public void OnAmountSlider(float value)
    {
        int newAmount = Mathf.RoundToInt(value);
        UpdateRemovedItemAmount(newAmount);
    }

    public void OnDeleteItem()
    {        
        if(currentAmount == 1)
        {
            inventory.ThrowItem(removedSlot.item);
            removedSlot.quantity--;
        }
        else
        {
            inventory.ThrowItem(removedSlot.item, currentAmount);
            removedSlot.quantity -= currentAmount;
        }

        if(removedSlot.quantity == 0)
        {
            removedSlot.ClearSlot();
        }

        inventory.UpdateSlots();
        gameObject.SetActive(false);
        inventory.ReturnData();
    }

    public void UpdateRemovedItemAmount(int amount)
    {
        currentAmount = amount;
        amountText.text = $"{currentAmount} / {maxAmount}";
        totalAmountText.text = $"{currentAmount} 개";
        deleteButton.interactable = currentAmount > 0; // 버튼은 최소 1개 이상의 아이템을 선택해야 활성화
    }
}
