using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIStore : PopupUI
{
    // 각 슬롯에 데이터를 전달하고 초기화.

    // 1. 상점 아이템 데이터가 필요함.
    public List<ItemSO> storeItems; // 일단 인스펙터창에서 넣어주기??

    // 상점에 물건이 추가될 수도 있으니까 리스트로.
    public Transform storeSlotParent; // 상점 슬롯이 배치될 부모 객체
    public GameObject storeSlotPrefab; // 상점 슬롯 프리팹

    private List<StoreItemSlot> storeSlots = new List<StoreItemSlot>();

    //------------------------------------------------------------------------

    // 2. 플레이어 인벤토리 아이템 데이터가 필요함.
    //public List<ItemSO> inventoryItems;

    public Transform playerInventorySlotParent; // 플레이어 인벤토리 슬롯이 배치될 부모 객체
    public GameObject playerInventorySlotPrefab; // 플레이어 인벤토리 슬롯 프리팹

    private List<StoreInventorySlot> playerInventorySlots = new List<StoreInventorySlot>();

    public GameObject storeWindow;

    public GameObject buyPopup; // 구매 팝업 창
    public GameObject sellPopup; // 판매 팝업 창
    //public GameObject BGPanel; // 팝업창 떴을 때 상점창을 막을 이미지

    private Inventory inventory;
    private int gold;
    public TextMeshProUGUI playerGoldText;

    public GameObject completeTextPrefab; // CompleteText 프리팹
    public Transform completeTextPos; //  CompleteText 생성 위치

    //---------------------------
    // 레벨에 따른 아이템 해금을 위한 플레이어
    //public Player player;

    public Button storeCloseBtn;

    private void Start()
    {
        storeCloseBtn.onClick.AddListener(Canvas_Main.OnCloseBtn);
        storeWindow.SetActive(false);

        //inventory = GameManager.Instance.Player.inventory; // 게임 매니저를 통해 인벤토리 객체 초기화

        //gold = GameManager.Instance.Player.gold.currentGold; // 현재 골드 가져오기
        //UpdateGoldUI(gold); // 골드 UI 업데이트

        //inventory.store = this;

        // 인벤토리 객체가 null인지 확인
        //if (inventory == null)
        //{
        //    Debug.Log("인벤토리가 null 임.");
        //    return;
        //}

    }



    private void OnEnable()
    {
        inventory = GameManager.Instance.Player.inventory; // 게임 매니저를 통해 인벤토리 객체 초기화
        inventory.store = this;

        gold = GameManager.Instance.Player.gold.currentGold; // 현재 골드 가져오기
        UpdateGoldUI(gold); // 골드 UI 업데이트
        ResetInventorySlots();
        ResetStoreSlots();

        // BuyPopup의 이벤트를 구독하여 구매 후 골드 UI, 상점/인벤토리 아이템 슬롯 업데이트
        buyPopup.GetComponent<BuyPopup>().OnGoldUpdated += UpdateGoldUI;
        buyPopup.GetComponent<BuyPopup>().OnItemBuy += ResetInventorySlots;
        buyPopup.GetComponent<BuyPopup>().OnItemBuy += ResetStoreSlots;

        // SellPopup의 이벤트를 구독하여 구매 후 골드 UI, 상점/인벤토리 아이템 슬롯 업데이트
        sellPopup.GetComponent<SellPopup>().OnGoldUpdated += UpdateGoldUI;
        sellPopup.GetComponent<SellPopup>().OnItemSell += ResetInventorySlots;
        sellPopup.GetComponent<SellPopup>().OnItemSell += ResetStoreSlots;

        buyPopup.GetComponent<BuyPopup>().OnItemBuy += () => ShowCompleteText("구매가 완료되었습니다");
        sellPopup.GetComponent<SellPopup>().OnItemSell += () => ShowCompleteText("판매가 완료되었습니다");
    }

    private void OnDisable()
    {
        // 이벤트 구독 취소
        buyPopup.GetComponent<BuyPopup>().OnGoldUpdated -= UpdateGoldUI;
        buyPopup.GetComponent<BuyPopup>().OnItemBuy -= ResetInventorySlots;
        buyPopup.GetComponent<BuyPopup>().OnItemBuy -= ResetStoreSlots;

        sellPopup.GetComponent<SellPopup>().OnGoldUpdated -= UpdateGoldUI;
        sellPopup.GetComponent<SellPopup>().OnItemSell -= ResetInventorySlots;
        sellPopup.GetComponent<SellPopup>().OnItemSell -= ResetStoreSlots;

        buyPopup.GetComponent<BuyPopup>().OnItemBuy -= () => ShowCompleteText("구매가 완료되었습니다");
        sellPopup.GetComponent<SellPopup>().OnItemSell -= () => ShowCompleteText("판매가 완료되었습니다");

        // 판매창이 열려있다면 닫기
        sellPopup.SetActive(false);
        // 구매창이 열려있다면 닫기
        buyPopup.SetActive(false);
    }

    public void ResetStoreSlots()
    {
        ClearStoreSlots();

        foreach (ItemSO item in storeItems) // 상점 아이템 슬롯 초기화
        {
            int playerLevel = DataManager.Instance.curData.playerData.level;
            int playerGold = GameManager.Instance.Player.gold.currentGold;
            GameObject slotObj = Instantiate(storeSlotPrefab, storeSlotParent);
            StoreItemSlot slot = slotObj.GetComponent<StoreItemSlot>();
            slot.SetSlot(item, playerLevel, playerGold, this);
            storeSlots.Add(slot);
        }
    }

    private void ClearStoreSlots()
    {
        foreach (Transform child in storeSlotParent)
        {
            Destroy(child.gameObject);
        }
        storeSlots.Clear();
    }

    public void ResetInventorySlots()
    {
        ClearInventorySlots();

        // 인벤토리 아이템 슬롯 초기화
        var items = inventory.GetItems();
        Debug.Log("현재 인벤 아이템 개수: " + items.Count);
        if (items == null || items.Count == 0)
        {
            Debug.Log("플레이어 인벤토리가 비어있음");
        }
        else
        {
            Debug.Log("상점인벤에 아이템 들어옴");
            foreach (ItemSO item in items)
            {
                GameObject slotObj = Instantiate(playerInventorySlotPrefab, playerInventorySlotParent);
                StoreInventorySlot slot = slotObj.GetComponent<StoreInventorySlot>();
                // 아이템 수량 가져오기
                int itemQuantity = GetItemQuantityInInventory(item);
                slot.item = item;
                slot.SetSlot(item, itemQuantity);
                playerInventorySlots.Add(slot);
            }
        }
    }

    private int GetItemQuantityInInventory(ItemSO item)
    {
        // 인벤토리에서 해당 아이템의 수량을 찾아 반환
        int quantity = 0;

        foreach (ItemSlot slot in inventory.itemSlots)
        {
            if (slot.item == item)
            {
                quantity += slot.quantity;
            }
        }

        return quantity;
    }

    private void ClearInventorySlots()
    {
        foreach (Transform child in playerInventorySlotParent)
        {
            Destroy(child.gameObject);
        }
        playerInventorySlots.Clear();
    }


    public void OpenBuyPopup(ItemSO item)
    {
        // 판매창이 열려있다면 닫기
        sellPopup.SetActive(false);

        // 아이템 정보를 팝업에 전달
        buyPopup.GetComponent<BuyPopup>().Setup(item);

        buyPopup.SetActive(true);

        //BGPanel.SetActive(true); // 상점창 잠그기
    }

    public void OpenSellPopup(ItemSO item, Inventory inventory)
    {
        // 구매창이 열려있다면 닫기
        buyPopup.SetActive(false);

        // 아이템 정보를 팝업에 전달
        sellPopup.GetComponent<SellPopup>().Setup(item, inventory);

        sellPopup.SetActive(true);

        //BGPanel.SetActive(true); // 상점창 잠그기
    }

    public void UpdateGoldUI(int updatedGoldAmount)
    {
        // PlayerGold 클래스의 현재 골드 값으로 UI 업데이트
        gold = updatedGoldAmount;
        playerGoldText.text = gold.ToString() + " G";
    }

    private void ShowCompleteText(string message)
    {
        // 프리팹 인스턴스화
        GameObject newTextObject = Instantiate(completeTextPrefab, completeTextPos);
        CompleteText completeText = newTextObject.GetComponent<CompleteText>();
        completeText.SetText(message);
        newTextObject.SetActive(true);
    }

}
