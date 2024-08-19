using System.Collections.Generic;
using UnityEngine;

public class LoadDataUI : MonoBehaviour
{
    [SerializeField] private Transform loadDataSlotParent;
    [SerializeField] private GameObject loadDataSlot;
    [SerializeField] private GameObject deletePopUp;

    private List<CharacterData> data => DataManager.Instance.userDataList.user;

    private void Start()
    {
        if (data.Count > 0)
        {
            InstantiateLoadDataSlot();
        }
    }

    private void InstantiateLoadDataSlot()
    {
        // 가지고 있는 데이터들 각 슬롯에 넣어줌
        foreach (CharacterData data in data)
        {
            GameObject slotObj = Instantiate(loadDataSlot, loadDataSlotParent);
            LoadDataSlot slot = slotObj.GetComponent<LoadDataSlot>();
            slot.SetSlot(data, deletePopUp);
        }
    }
}
