using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIMovement : TutorialBase
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Vector3 endPosition;
    private bool isCompleted = false;

    public override void Enter()
    {
        StartCoroutine(nameof(Movement));
    }

    public override void Execute(TutorialController controller)
    {
        if (isCompleted == true)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        
    }

    private IEnumerator Movement()
    {
        float current = 0f;
        float percent = 0f;
        float moveTime = 0.5f;
        Vector3 start = rectTransform.anchoredPosition;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTime;

            rectTransform.anchoredPosition = Vector3.Lerp(start, endPosition, percent);

            yield return null;
        }

        isCompleted = true;
    }
}
