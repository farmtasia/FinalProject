using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
    protected override void Start()
    {
        UIManager.Instance.ShowUI<Canvas_Start>(UIs.Scene);
    }

}
