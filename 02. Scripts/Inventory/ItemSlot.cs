using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : SlotBase, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("ItemSlot Properties")]
    public int quantity;
    public Inventory inventory;
    public GameObject quantityBG;
    public TextMeshProUGUI quantityText;
    public GameObject equipQuickBG;
    public GameObject effectImg;
    public GameObject equipText;
    public GameObject quickText;
    public GameObject outline;
    public bool isEquip; // 장착여부
    public bool isQuick; // 퀵슬롯여부
    public int quickIndex; // 등록된 퀵슬롯의 인덱스

    public float minClickTime; // 최소 클릭 시간
    private float clickTime = 0f; // 클릭 중인 시간
    private bool isClick = false;
    private bool isDraging = false; // 드래그 중인지 확인하기 위한 변수

    void Start()
    {
        outline.SetActive(false);
        inventory = GameManager.Instance.Player.inventory;
    }

    void Update()
    {
        if (isClick)
        {
            clickTime += Time.deltaTime;
            if (clickTime >= minClickTime && !isDraging)
            {
                inventory.toolTip.ToolTipToggle(item);
                outline.SetActive(true);
                clickTime = 0f;
                isClick = false;
            }
        }
        else
        {
            clickTime = 0f;
        }
    }

    public override void SetSlot()
    {
        base.SetSlot();
        icon.SetNativeSize();
        SlotUIStat();
    }

    public override void ClearSlot()
    {
        base.ClearSlot();
        isEquip = isQuick = false;
        quantityBG.SetActive(false);
        quantityText.text = string.Empty;
        equipQuickBG.SetActive(false);
        effectImg.SetActive(false);
        equipText.SetActive(false);
        quickText.SetActive(false);
        ResetSlot();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null && !isEquip && !isQuick)
        {
            outline.SetActive(true);
            isDraging = true;
            inventory.toolTip.gameObject.SetActive(false);

            inventory.dragSlot.dragItemSlot = this;
            inventory.dragSlot.DragSetImage(icon);
            inventory.dragSlot.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            inventory.toolTip.gameObject.SetActive(false);
            inventory.dragSlot.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        outline.SetActive(false);
        isDraging = false;
        inventory.InventoryButtonClear();
        inventory.dragSlot.SetColor(0);
        inventory.dragSlot.dragItemSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (inventory.dragSlot.dragItemSlot != null && !isEquip && !isQuick)
        {
            int temp = quantity;
            quantity = inventory.dragSlot.dragItemSlot.quantity;
            inventory.dragSlot.dragItemSlot.quantity = temp;

            inventory.ExchangeSlot(index, inventory.dragSlot.dragItemSlot.index);
        }
    }

    public void ButtonDown()
    {
        isClick = true;

        if (item != null && button.enabled)
        {
            inventory.SelectedItem(index);

            if (outline.activeSelf)
            {
                outline.SetActive(false);
                inventory.InventoryButtonClear();
                inventory.selectedSlot = null;
            }
            else
            {
                inventory.CheckOutLineEnabled(this);
            }
        }
    }

    public void ButtonUp()
    {
        isClick = false;
        inventory.toolTip.gameObject.SetActive(false);
    }

    public void ResetSlot()
    {
        isClick = false;
        outline.SetActive(false);
    }

    public void SlotUIStat()
    {
        // TODO(다빈) : 우선 동작은 하지만 수정이 필요함(하드코딩)
        if (quantity > 1)
        {
            quantityBG.SetActive(true);
            quantityText.text = quantity.ToString();
        }
        else
        {
            quantityBG.SetActive(false);
            quantityText.text = string.Empty;
        }

        if (isEquip || isQuick)
        {
            equipQuickBG.SetActive(true);
            effectImg.SetActive(true);

            if (isEquip) equipText.SetActive(true);
            else equipText.SetActive(false);

            if (isQuick) quickText.SetActive(true);
            else quickText.SetActive(false);
        }
        else
        {
            equipQuickBG.SetActive(false);
            effectImg.SetActive(false);
            equipText.SetActive(false);
            quickText.SetActive(false);
        }
    }
}
