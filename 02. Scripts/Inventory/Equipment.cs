using UnityEngine;

public class Equipment : MonoBehaviour
{
    public EquipTool curEquipTool;
    public Transform equipPosition;

    // TODO : 도구 장착해제시 부분 UI처럼 최초 실행시 생성하고 이후에는 setactive로 관리해야함
    public void EquipNew(ItemSO data)
    {
        UnEquip();
        curEquipTool = Instantiate(data.itemPrefab, equipPosition).GetComponent<EquipTool>();
        curEquipTool.gameObject.layer = (int)ELayerName.Default; // 장착 시 레이어를 변경하여 레이캐스트에서 반응하지 않도록 함(프리팹은 영향 없음)
        Canvas_Main.SetcurEquipToolUI(curEquipTool.itemData, true);

        if (curEquipTool != null )
        {
            GameManager.Instance.Player.equipment.equipPosition.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
    }

    public void UnEquip()
    {
        if (curEquipTool != null)
        {
            DestroyImmediate(curEquipTool.gameObject);
            Canvas_Main.SetcurEquipToolUI(curEquipTool.itemData, false);
            curEquipTool = null;
        }
    }
 
    public int PlayerNowEquipToolCode()
    {
        // 현재 장착된 아이템이 있다면 해당 아이템의 코드를 반환
        if (curEquipTool != null)
        {
            return curEquipTool.itemData.itemCode;
        }
        return 0;
    }
}
