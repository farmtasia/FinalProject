using UnityEngine.SceneManagement;

public class ItemObject : ItemObjectBase
{
    // private Collection collection; // 채린 테스트 중 _ 콜렉션

    public override void OnInteract()
    {

        for (int i = 1; i <= itemCount; i++)
        {
            GameManager.Instance.Player.itemdata = itemData;
            //GameManager.Instance.Player.showItem.ShowGetItemInfoBox(itemData.itemIcon, $"+ {itemCount}");
            GameManager.Instance.Player.itemQuantity = itemCount;
            GameManager.Instance.Player.addItem?.Invoke();
        }
        GameManager.Instance.Player.itemQuantity = 1;

        Destroy(gameObject);
    }
}
