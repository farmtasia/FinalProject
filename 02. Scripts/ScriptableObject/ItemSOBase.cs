using UnityEngine;

public class ItemSOBase : ScriptableObject
{
    [Header("Info")]
    public int itemCode;
    public string itemName; // 채린 사용

    [TextArea]
    public string itemDescription;

    public Sprite itemIcon; // 채린 사용
    public int itemPrice;
    public GameObject itemPrefab; // 아이템 버릴 때, 도구라면 장착할 때 사용
    public int requiredLevel; // 해금에 필요한 플레이어 레벨
    public float expValue; // 경험치

    [Header("ItemType")]
    public EItemType itemType;
    public EItemDetailType detailType;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;
}
