using TMPro;
using UnityEngine;

public class QuickSlot : SlotBase
{
    public QuickSlotInven quickSlotInven;

    [Header("QuickSlot Info")]
    public int invenSlotIndex; // 퀵슬롯에 등록된 인벤토리슬롯 인덱스
    public bool isEquip = false;

    [Header("QuickSlot UI")]
    public GameObject quantityBG;
    public GameObject equipImg;
    public GameObject equipBG;
    public TextMeshProUGUI quantityTxt;

    private void Start()
    {
        quickSlotInven = GameManager.Instance.Player.quickSlotInven;
    }

    public void OnQuickSlotButtonClick()
    {
        IsEquipToggle(invenSlotIndex);
    }

    public override void SetSlot()
    {
        base.SetSlot();

        if (isEquip)
        {
            equipImg.SetActive(true);
            equipBG.SetActive(true);
        }
    }

    public override void ClearSlot()
    {
        base.ClearSlot();
        quantityBG.SetActive(false);
        equipImg.SetActive(false);
        equipBG.SetActive(false);
        quantityTxt.text = "";
    }

    private void IsEquipToggle(int invenIndex)
    {
        if (item == null) return;

        GameManager.Instance.Player.inventory.SelectedItem(invenIndex);

        if (isEquip)
        {
            GameManager.Instance.Player.inventory.OnUnEquipButton();
            IsEquipUI(false);
            isEquip = false;
        }
        else
        {
            GameManager.Instance.Player.inventory.OnEquipButton();
            IsEquipUI(true);
            isEquip = true;
        }
    }

    public void IsEquipUI(bool equip)
    {
        isEquip = equip;
        equipImg.SetActive(equip);
        equipBG.SetActive(equip);
    }
}
