using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collection : MonoBehaviour
{
    public List<ItemSO> allItems;  // 모든 아이템 데이터를 담은 리스트
    public List<CollectionSlot> cropItemSlots;  // 농작물 아이템 슬롯의 Transform 리스트
    public List<CollectionSlot> fishItemSlots;  // 해산물 아이템 슬롯의 Transform 리스트
    public List<CollectionSlot> fruitItemSlots;  // 과일 아이템 슬롯의 Transform 리스트

    private void Start()
    {
        if (DataManager.Instance.curData.collectionData.collectedItems != null)
        {
            foreach (var item in DataManager.Instance.curData.collectionData.collectedItems)
            {
                CollectItem(item);
            }
        }
    }

    public void CollectItem(ItemSO item)
    {
        if (!DataManager.Instance.curData.collectionData.collectedItems.Contains(item))
        {
            DataManager.Instance.curData.collectionData.collectedItems.Add(item);
        }
        UpdateCollectionUI(item);
    }

    void UpdateCollectionUI(ItemSO item)
    {
        List<CollectionSlot> targetSlots = null;

        //아이템의 타입에 따라 해당 슬롯 리스트 선택
        switch (item.detailType)
        {
            case EItemDetailType.VEGETABLES:
                targetSlots = cropItemSlots;
                break;
            case EItemDetailType.FISHES:
                targetSlots = fishItemSlots;
                break;
            case EItemDetailType.FRUITS:
                targetSlots = fruitItemSlots;
                break;
        }

        if (targetSlots != null)
        {
            for (int i = 0; i < targetSlots.Count; i++)
            {
                if (targetSlots[i].data.itemCode == item.itemCode)
                {
                    Transform slot = targetSlots[i].transform;

                    // Black 오브젝트를 찾아 비활성화
                    Transform blackIcon = slot.GetChild(1);
                    if (blackIcon != null && blackIcon.gameObject.activeSelf)
                    {
                        blackIcon.gameObject.SetActive(false);
                    }

                    break;

                }
            }
        }
    }
}
