using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class StoreInventorySlot : SlotBase
{
    public TextMeshProUGUI quantityText;
    private Inventory inventory;

    private void Awake()
    {
        inventory = GameManager.Instance.Player.inventory;
    }

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    public override void SetSlot()
    {
        base.SetSlot();

        // 아이콘 이미지의 크기를 원본 크기로 설정
        icon.SetNativeSize();
    }

    public void SetSlot(ItemSO newItem, int quantity)
    {
        item = newItem;
        icon.sprite = item.itemIcon;
        icon.gameObject.SetActive(true);
        button.enabled = true;
        quantityText.text = quantity.ToString();
        UpdateQuantity(quantity);

        // 아이콘 이미지의 크기를 원본 크기로 설정
        icon.SetNativeSize();
    }

    public void UpdateQuantity(int quantity)
    {
        quantityText.text = quantity.ToString(); 

        // 수량이 0 이하일 때
        if (quantity <= 0)
        {
            ClearSlot(); // 슬롯 초기화
            //gameObject.SetActive(false); // 슬롯을 비활성화 / 어차피 생성되니까 삭제??
        }
    }

    public override void ClearSlot()
    {
        //base.ClearSlot();
        //quantityText.text = "";
        Destroy(gameObject);
    }

    public void OnClick()
    {
        // 인벤 슬롯 클릭 시 SellPopup을 띄움
        Debug.Log("상점 인벤 아이템 클릭");
        UIStore store = FindObjectOfType<UIStore>();
        store.OpenSellPopup(item, inventory);
    }
}
