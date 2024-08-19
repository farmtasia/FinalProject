using UnityEngine;

public class QuickSlotInven : MonoBehaviour
{
    public Transform quickSlotsPanel;
    public QuickSlot[] quickSlots;
    public Inventory inventory;

    private void Awake()
    {
        if (GameManager.Instance.Player.quickSlotInven == null)
        {
            GameManager.Instance.Player.quickSlotInven = this;
        }
    }

    private void Start()
    {
        StartSetQuickSlot();
        inventory = GameManager.Instance.Player.inventory;
    }

    private void StartSetQuickSlot()
    {
        quickSlots = new QuickSlot[quickSlotsPanel.childCount];

        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i] = quickSlotsPanel.GetChild(i).GetComponent<QuickSlot>();
            quickSlots[i].index = i;
        }

        UpdateQuickSlot();
    }

    // TODO(다빈) : 로직 수정해야함(이렇게까지 할 게 아닌 듯함)
    public void RegisterQuickSlot(int seletedSlotIndex)
    {
        // 1) 빈슬롯이 있는지 확인(없다면 false를 반환)
        if (!CheckEmptyQuickSlot()) return;

        // 2) 빈슬롯이 있다면 가져옴
        QuickSlot quickSlot = GetEmptyQuilckSlot();

        // 3) 빈슬롯에 정보를 넣고
        if (quickSlot != null)
        {
            quickSlot.item = inventory.itemSlots[seletedSlotIndex].item;
            quickSlot.invenSlotIndex = seletedSlotIndex;
            inventory.itemSlots[seletedSlotIndex].quickIndex = quickSlot.index;

            // 4) 인벤토리슬롯의 장착여부를 확인
            if (inventory.itemSlots[seletedSlotIndex].isEquip)
            {
                // 5) 장착되어있다면
                quickSlot.isEquip = inventory.itemSlots[seletedSlotIndex].isEquip;
            }

            UpdateQuickSlot();
        }
    }

    public void GetSavedQuickSlot(int saveInvenIndex, int saveQuickIndex)
    {
        Debug.Log("퀵슬롯까지 들어옴");

        quickSlots[saveQuickIndex].item = inventory.itemSlots[saveInvenIndex].item;
        quickSlots[saveQuickIndex].invenSlotIndex = saveInvenIndex;

        if (inventory.itemSlots[saveInvenIndex].isEquip)
        {
            quickSlots[saveQuickIndex].isEquip = inventory.itemSlots[saveInvenIndex].isEquip;
        }

        UpdateQuickSlot();
    }

    public void CancelQuickSlot(int seletedSlotIndex)
    {
        for (int i = 0; i < quickSlots.Length; i++)
        {
            if (quickSlots[i].invenSlotIndex == seletedSlotIndex)
            {
                quickSlots[i].quickSlotInven = null;
                quickSlots[i].isEquip = false;
                quickSlots[i].item = null;
            }
        }
        UpdateQuickSlot();
    }

    public void UpdateQuickSlot()
    { 
        for (int i = 0; i < quickSlots.Length; i++)
        {
            if (quickSlots[i].item != null)
            {
                quickSlots[i].SetSlot();
            }
            else
            {
                quickSlots[i].ClearSlot();
            }
        }
    }

    public bool CheckEmptyQuickSlot()
    {
        for (int i = 0; i < quickSlots.Length; i++)
        {
            if (quickSlots[i].item == null)
            {
                return true;
            }
        }
        return false;
    }

    QuickSlot GetEmptyQuilckSlot()
    {
        for (int i = 0; i < quickSlots.Length; i++)
        {
            if (quickSlots[i].item == null)
            {
                return quickSlots[i];
            }
        }
        return null;
    }

    QuickSlot CheckQuickSlotCurEquip()
    {
        for (int i = 0; i < quickSlots.Length; i++)
        {
            if (quickSlots[i].isEquip)
            {
                return quickSlots[i];
            }
        }
        return null;
    }

    public void CurrentEquipSlotSet(int equipIndex)
    {
        QuickSlot quickSlot = CheckQuickSlotCurEquip();

        if (quickSlot != null && quickSlot.index != equipIndex)
        {
            quickSlot.IsEquipUI(false);
        }

        quickSlots[equipIndex].IsEquipUI(true);
        UpdateQuickSlot();
    }
}
