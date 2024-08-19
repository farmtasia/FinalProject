using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeScene : BaseScene
{
    protected override void Start()
    {
        base.Start();
        UIManager.Instance.ShowUI<UIDialogue>(UIs.Popup);
    }

}
