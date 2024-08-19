using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    protected virtual void Start()
    {
        DataManager.Instance.Clear();
        UIManager.Instance.ShowUI<Canvas_Main>(UIs.Scene);
        UIManager.Instance.ShowUI<Inventory>(UIs.Popup);
    }
}
