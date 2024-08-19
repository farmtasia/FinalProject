using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryTutorialBase : TutorialBase
{
    public virtual void Initialize(Canvas_Tutorial tutorialUI)
    {
        base.Initialize(tutorialUI);
    }

    public abstract override void Enter();
    public abstract override void Execute(TutorialController controller);
    public abstract override void Exit();
}
