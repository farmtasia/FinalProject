using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractObjectBase : MonoBehaviour
{
    protected Player player;
    protected ItemSO giveItemSO;

    protected virtual void Start()
    {
        player = GameManager.Instance.Player;
    }

    // 데이터매니저에서 리스트 반환하는 메서드를 활용하여 구현하면 됨
    public virtual ItemSO GiveItem(List<ItemSO> itemList) // 실제 사용할 곳으로 이동해야함
    {
        giveItemSO = itemList[Random.Range(0, itemList.Count)];
        return giveItemSO;
    }
}
