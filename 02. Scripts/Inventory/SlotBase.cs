using UnityEngine;
using UnityEngine.UI;

public class SlotBase : MonoBehaviour
{
    [Header("SlotBase Properties")]
    public ItemSO item;
    public Button button;
    public Image icon;
    public int index;

    public virtual void SetSlot()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.itemIcon;
        button.enabled = true;
    }

    public virtual void ClearSlot()
    {
        button.enabled = false; // 슬롯이 null일때 버튼 비활성화
        icon.sprite = null;
        item = null;
        icon.gameObject.SetActive(false);
    }
}
