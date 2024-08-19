using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : BaseUI
{
    [Header("Connected Scripts")]
    public UIStore store;
    public Collection collection;
    public InfoBox infoBox;
    public DragSlot dragSlot;
    public ToolTip toolTip;
    public RemovedItemPopup removedItemPopup;
    public QuickSlotInven quickSlotInven;

    [Header("Connected Object")]
    public Transform slotPanel;
    public Transform dropPosition;
    public Transform equipPosition;
    public GameObject inventoryWindow;

    [Header("Selected Item & ItemSlot")]
    public ItemSlot selectedSlot;
    public int selectedSlotIndex = 0;
    public int curEquipIndex = 0;

    [Header("ItemSlot List")]
    public ItemSlot[] itemSlots;

    [Header("Inventory Button")]
    public GameObject equipBtn;
    public GameObject unEquipBtn;
    public GameObject moveQuickBtn;
    public GameObject cancelQuickBtn;
    public GameObject useBtn;
    public GameObject deleteBtn;

    private void Awake()
    {
        if(GameManager.Instance.Player.inventory == null)
        {
            GameManager.Instance.Player.inventory = this;
        }
    }

    private void Start()
    {
        StartSet();
        quickSlotInven = GameManager.Instance.Player.quickSlotInven;

        ApplyData();
        dropPosition = GameManager.Instance.Player.dropPosition;
        equipPosition = GameManager.Instance.Player.equipToolPosition;

        GameManager.Instance.Player.addItem -= AddItem;
        GameManager.Instance.Player.addItem += AddItem;

        InventoryButtonClear();
        inventoryWindow.SetActive(false);
        if (infoBox == null)
            infoBox = GetComponentInChildren<InfoBox>();
        infoBox.Init();

        if (store != null) // 상점의 데이터들은 인벤토리 데이터 로드 후에 실행이 되어야 함
        {
            // 1초 후에 상점과 상점 내 인벤토리 슬롯 초기화(UIStore의 Start가 먼저 실행될 수도 있어서 1초 후 실행되도록)
            Invoke("InitializeStoreAndInventorySlots", 1.0f);
        }
    }

    public void OnClickExitBtn()
    {
        gameObject.SetActive(false);
        ReturnData();
        GameManager.Instance.Player.topDownMovement.ResetMoveSpeed();
    }

    private void OnDestroy()
    {
        ReturnData();
        GameManager.Instance.Player.addItem -= AddItem;
    }

    private void OnEnable()
    {
        if (selectedSlot != null)
        {
            selectedSlot.ResetSlot();
            selectedSlot = null;
        }

        InventoryButtonClear();
        selectedSlotIndex = 0;
    }

    public void StartSet()
    {
        itemSlots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            itemSlots[i].index = i;
        }

        UpdateSlots();
    }

    private void AddItem()
    {
        ItemSO itemData = GameManager.Instance.Player.itemdata;
        int itemQuantity = GameManager.Instance.Player.itemQuantity;

        //중복가능한 아이템 타입 확인
        if (CheckItemType(itemData))
        {
            ThrowItem(itemData);
            ReturnData();
            return;
        }

        // 동일한 아이템이 있는지 확인 후 있다면 개수를 늘림
        if (itemData.canStack)
        {
            ItemSlot slot = GetItemStack(itemData);
            if (slot != null)
            {
                slot.quantity++;
                GameManager.Instance.Player.showItem.ShowGetItemInfoBox(itemData.itemIcon, itemQuantity);
                UpdateSlots();
                collection.CollectItem(itemData);
                GameManager.Instance.Player.itemdata = null;
                ReturnData();
                return;
            }
        }

        // 비어있는 슬롯을 우선 가져오고, 빈슬롯이 있다면 실행
        ItemSlot emptySlot = GetEmptySlot();
        if (emptySlot != null)
        {
            emptySlot.item = itemData;
            emptySlot.quantity = 1;
            GameManager.Instance.Player.showItem.ShowGetItemInfoBox(itemData.itemIcon, itemQuantity);
            UpdateSlots();
            collection.CollectItem(itemData);
            GameManager.Instance.Player.itemdata = null;
            ReturnData();
            return;
        }

        // 비어있는 슬롯이 없으면 아이템 반환함
        ThrowItem(itemData);
        GameManager.Instance.Player.itemdata = null;
        ReturnData();
    }

    public bool CheckItemType(ItemSO data)
    {
        if (data.itemType == EItemType.EQUIPABLE)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].item != null && itemSlots[i].item == data)
                {
                    return true; // 동일한 Equipable 타입의 아이템이 존재함
                }
            }
        }
        return false; // 동일한 Equipable 타입의 아이템 없음
    }

    ItemSlot GetItemStack(ItemSO data)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item == data && itemSlots[i].quantity < data.maxStackAmount)
            {
                return itemSlots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < itemSlots.Length; ++i)
        {
            if (itemSlots[i].item == null)
            {
                return itemSlots[i];
            }
        }
        return null;
    }

    public void UpdateSlots()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item != null)
            {
                itemSlots[i].SetSlot();
            }
            else
            {
                itemSlots[i].ClearSlot();
            }
        }
    }

    public bool CheckEmptySlot()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item == null)
            {
                return true;
            }
        }
        return false;
    }

    public void ExchangeSlot(int a, int b)
    {
        ItemSO temp = itemSlots[a].item;
        itemSlots[a].item = itemSlots[b].item;
        itemSlots[b].item = temp;

        UpdateSlots();
    }

    public void ThrowItem(ItemSO data, int amount = 1)
    {
        GameObject item = Instantiate(data.itemPrefab, dropPosition.position, Quaternion.identity);
        if (data.itemType != EItemType.EQUIPABLE)
        {
            item.GetComponent<ItemObject>().itemCount = amount; // 아이템 오브젝트의 스크립트를 가져오고 접근하여 할당
        }
    }

    // 상점 관련 메서드-------------------------------------------------------------------------------------------------
    private void InitializeStoreAndInventorySlots()
    {
        store.ResetStoreSlots();
        store.ResetInventorySlots();
    }

    // 최유정 추가 : RemovedOneItem()은 매개변수를 받지 않고 구현되어 있어서, 외부에서 특정 아이템을 지울 때 쓸 수 없음.
    // 매개변수를 받아 특정 아이템을 제거할 수 있는 메서드 추가.
    public void RemovedItem(ItemSO item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item == item)
            {
                itemSlots[i].quantity--;

                if (itemSlots[i].isQuick)
                    quickSlotInven.CancelQuickSlot(itemSlots[i].index);

                if (itemSlots[i].quantity <= 0)
                    itemSlots[i].ClearSlot();

                UpdateSlots();
                ReturnData();
                return;
            }
        }
    }

    // 최유정 추가 : 인벤토리의 모든 아이템을 반환하는 메서드(현재 인벤토리에 있는 모든 아이템을 List<ItemSO> 형태로 반환)
    public List<ItemSO> GetItems()
    {
        List<ItemSO> items = new List<ItemSO>();
        foreach (ItemSlot slot in itemSlots)
        {
            if (slot.item != null && !slot.isEquip)
            {
                items.Add(slot.item);
            }
        }
        return items;
    }

    // 도감 관련 메서드-----------------------------------------------------------------------------------------------
    // 강채린 추가 : 인벤토리에 씨앗을 1개이상 소지하고 있는지 확인하기
    public int GetItemCount(ItemSO item)
    {
        int count = 0;
        foreach (ItemSlot slot in itemSlots)
        {
            if (slot.item == item)
            {
                count += slot.quantity;
            }
        }
        return count;
    }

    // 저장관련 메서드------------------------------------------------------------------------------------------------
    public void ApplyData()    // 데이터 매니저에서 가져온 데이터 적용
    {
        List<SlotData> loadData = DataManager.Instance.SetInventoryData();
        if (loadData == null)   // 저장된 정보가 없다면 그대로 진행
            return;

        foreach (var slotData in loadData)    // 슬롯 하나씩 검사
        {
            if (slotData.item != null)    // 데이터가 있다면
            {
                itemSlots[slotData.index].item = slotData.item;    // 지정된 인덱스에 아이템 정보 불러오기
                itemSlots[slotData.index].quantity = slotData.itemCount;    // 지정된 인덱스에 아이템 개수 불러오기
                itemSlots[slotData.index].isEquip = slotData.isEquip;
                itemSlots[slotData.index].isQuick = slotData.isQuick;     // 지정된 인덱스에 퀵슬롯등록상태 불러오기
                itemSlots[slotData.index].quickIndex = slotData.quickIndex;
                StartSetIsEquip(itemSlots[slotData.index], itemSlots[slotData.index].index, itemSlots[slotData.index].quickIndex);
            }           
            UpdateSlots();    // 슬롯에 데이터 적용
        }
    }

    // 저장관련 ------------------------------------------------------------------------------------------------------
    public void ReturnData()    // 데이터 매니저로 현재 데이터 전달
    {
        List<SlotData> returnData = new List<SlotData>();
        for (int i = 0; i < itemSlots.Length; i++)    // 모든 슬롯 확인
        {
            if (itemSlots[i].item == null)    // 예외 처리
                continue;

            SlotData slotData = new SlotData(itemSlots[i].item, itemSlots[i].quantity, i, itemSlots[i].isEquip, itemSlots[i].isQuick, itemSlots[i].quickIndex);
            returnData.Add(slotData);    // 리스트에 데이터 더하기
        }
        DataManager.Instance.GetInventoryData(returnData);
    }

    public void StartSetIsEquip(ItemSlot itemSlot, int saveIndex, int saveQuickIndex)
    {
        if (itemSlot.isEquip)
        {
            curEquipIndex = itemSlot.index;

            if (GameManager.Instance.Player.equipment.curEquipTool == null)
                GameManager.Instance.Player.equipment.EquipNew(itemSlot.item);
        }

        if (itemSlot.isQuick)
        {
            Debug.Log("조건문 안에 들어옴");
            quickSlotInven.GetSavedQuickSlot(saveIndex, saveQuickIndex);
        }
    }

    // 인벤토리 버튼 관련----------------------------------------------------------------------------------------------
    public void InventoryButtonClear()
    {
        equipBtn.SetActive(false);
        unEquipBtn.SetActive(false);
        moveQuickBtn.SetActive(false);
        cancelQuickBtn.SetActive(false);
        useBtn.SetActive(false);
        deleteBtn.SetActive(false);
    }

    public void SelectedItem(int index)
    {
        if (itemSlots[index].item == null) return;

        selectedSlotIndex = index;

        if (!inventoryWindow.gameObject.activeSelf) return;

        equipBtn.SetActive(itemSlots[index].item.itemType == EItemType.EQUIPABLE && !itemSlots[index].isEquip);
        unEquipBtn.SetActive(itemSlots[index].item.itemType == EItemType.EQUIPABLE && itemSlots[index].isEquip);

        moveQuickBtn.SetActive
            ((itemSlots[index].item.itemType == EItemType.EQUIPABLE
            /*|| itemSlots[index].item.detailType == EItemDetailType.SEED*/)
            && quickSlotInven.CheckEmptyQuickSlot() && !itemSlots[index].isQuick);

        cancelQuickBtn.SetActive
            ((itemSlots[index].item.itemType == EItemType.EQUIPABLE
            /*|| itemSlots[index].item.detailType == EItemDetailType.SEED*/)
            && itemSlots[index].isQuick);

        //useBtn.SetActive(itemSlots[index].item.itemType == EItemType.CONSUMABLE);
        deleteBtn.SetActive(true);
    }

    public void CheckOutLineEnabled(ItemSlot currentSlot)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].outline.activeSelf == true && itemSlots[i] != currentSlot)
            {
                itemSlots[i].outline.SetActive(false); // 다른 슬롯의 outline을 끔
            }
        }
        currentSlot.outline.SetActive(true);
        selectedSlot = currentSlot;
    }

    public void OnEquipButton()
    {
        Equip(curEquipIndex, selectedSlotIndex);
        curEquipIndex = selectedSlotIndex;
        SelectedItem(selectedSlotIndex);
        UpdateSlots();

        if (itemSlots[curEquipIndex].isQuick)
        {
            quickSlotInven.CurrentEquipSlotSet(itemSlots[curEquipIndex].quickIndex);
        }
    }

    public void Equip(int curEquipIdx, int newEquipIdx) // curInvenEquipIdx = 현재장착중인 슬롯(있다면), newEquipIdx = 장착할 도구 슬롯
    {
        if (itemSlots[curEquipIdx].isEquip)
        {
            UnEquip(curEquipIdx);
        }

        GameManager.Instance.Player.equipment.EquipNew(itemSlots[newEquipIdx].item);
        itemSlots[newEquipIdx].isEquip = true;
    }

    void UnEquip(int curEquipIdx)
    {
        itemSlots[curEquipIdx].isEquip = false;
        GameManager.Instance.Player.equipment.UnEquip();

        if (selectedSlotIndex == curEquipIdx)
        {
            SelectedItem(selectedSlotIndex);
        }
    }

    public void OnUnEquipButton()
    {
        UnEquip(selectedSlotIndex);
        UpdateSlots();
    }

    public void OnEnterQuickButton()
    {
        itemSlots[selectedSlotIndex].isQuick = true;
        quickSlotInven.RegisterQuickSlot(selectedSlotIndex);
        SelectedItem(selectedSlotIndex);
        UpdateSlots();
    }

    public void OnCancelQuickButton()
    {
        Debug.Log("퀵슬롯 등록해제");

        itemSlots[selectedSlotIndex].isQuick = false;
        quickSlotInven.CancelQuickSlot(selectedSlotIndex);
        SelectedItem(selectedSlotIndex);
        UpdateSlots();
    }

    public void OnUseButton()
    {
        if (selectedSlot.quantity >= 2)
        {
            Debug.Log("언제, 몇개를 사용할건지?");
        }
        else
        {
            Debug.Log("사용");
            RemovedOneItem();
        }
    }

    public void OnDeleteButton()
    {
        // 도구가 아닌 아이템이 2개 이상일 때
        if (selectedSlot.quantity >= 2 && selectedSlot.item.itemType != EItemType.EQUIPABLE)
        {
            removedItemPopup.RemovedItemPopupSet(selectedSlot);
            removedItemPopup.gameObject.SetActive(true);
        }
        // 도구이며, 장착 중일 때
        else if (selectedSlot.isEquip && selectedSlot.item.itemType == EItemType.EQUIPABLE)
        {
            Debug.Log("장착 중인 아이템은 버릴수 없습니다.");
        }
        else
        {
            ThrowItem(selectedSlot.item);
            quickSlotInven.CancelQuickSlot(selectedSlotIndex);
            SelectedItem(selectedSlotIndex);
            RemovedOneItem();
        }
    }

    public void RemovedOneItem()
    {
        selectedSlot.quantity--;

        if (selectedSlot.quantity <= 0)
        {
            selectedSlot.ClearSlot();
            InventoryButtonClear();
        }
        UpdateSlots();
    }
}