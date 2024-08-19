using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageScene : BaseScene
{
    protected override void Start()
    {
        base.Start();
        UIManager.Instance.ShowUI<UIStore>(UIs.Popup);
        UIManager.Instance.ShowUI<UIDialogue>(UIs.Popup);
    }

}
