using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public int curSortingOrder = 10;
    public int activeUI = 0;

    public void ShowUI<T>(UIs type)
    {
        string uiName = typeof(T).ToString();
        if (DataManager.Instance.CheckUIDictionary(uiName))
        {
            BaseUI ui = DataManager.Instance.GetUIInDictionary(uiName);
            ui.gameObject.SetActive(true);
            activeUI++;
            return;
        }

        BaseUI obj = ResourcesManager.Instance.LoadUIObject(type, uiName);
        obj.SetOrder(curSortingOrder++);
        activeUI++;
        DataManager.Instance.AddUIInDictionary(uiName, obj);
    }

    public void HideUI<T>() // 이걸 통해서 자신 말고 다른 것도 끌 수 있음
    {
        string uiName = typeof(T).ToString();
        BaseUI ui = DataManager.Instance.GetUIInDictionary(uiName);
        ui.gameObject.SetActive(false);
        curSortingOrder--;
    }

    public void ClearSortingOrder()
    {
        curSortingOrder -= activeUI;
        activeUI = 0;
    }
}
