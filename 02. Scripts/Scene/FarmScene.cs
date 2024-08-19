using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmScene : BaseScene
{
    private bool hasTutorial;

    protected override void Start()
    {
        base.Start();

        hasTutorial = DataManager.Instance.curData.playerData.hasCompletedTutorial;

        CheckTutorialStatus();
    }

    private void CheckTutorialStatus()
    {
        if (!hasTutorial)
        {
            StartTutorial();
        }
    }


    private void StartTutorial()
    {
        Debug.Log("튜토리얼 시작");
        var tutorialController = FindObjectOfType<TutorialController>();
        if (tutorialController != null)
        {
            tutorialController.StartTutorial();
        }
    }
}
