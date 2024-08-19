using UnityEngine;

public class ItemObjectBase : MonoBehaviour, IInteractable
{
    public ItemSO itemData;
    public int itemCount = 1;

    public string GetInteractPrompt()
    {
        GameManager.Instance.Player.interaction.OnEquipUI(true); // 인터랙션 하는 곳에 다 붙여줘야됨.
        string str = $"{itemData.itemName}";
        return str;
    }

    public virtual void OnInteract()
    {
        // 상호작용과 동시에 아이템 데이터 대입 및 Invoke 실행
        GameManager.Instance.Player.itemdata = itemData;
        GameManager.Instance.Player.itemQuantity = itemCount;
        //GameManager.Instance.Player.showItem.ShowGetItemInfoBox(itemData.itemIcon, $"+ {itemCount}");
        GameManager.Instance.Player.addItem?.Invoke();

        GameManager.Instance.Player.itemQuantity = 1;
        Destroy(gameObject);
    }
}
