using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialBase : MonoBehaviour
{
    protected Canvas_Tutorial tutorialUI;

    public virtual void Initialize(Canvas_Tutorial tutorialUI)
    {
        this.tutorialUI = tutorialUI;
    }

    public abstract void Enter();

    public abstract void Execute(TutorialController controller);

    public abstract void Exit();
}
