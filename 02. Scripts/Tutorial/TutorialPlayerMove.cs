using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayerMove : TutorialBase
{
    [SerializeField] private GameObject RestrictedArea;
    [SerializeField] private float timeLimit = 4f;

    private float movementTimeRemaining;

    public override void Enter()
    {
        movementTimeRemaining = timeLimit;
        RestrictedArea.SetActive(true);
        tutorialUI.SetTutorialMessage("먼저 움직여봅시다. \nWASD키나 모바일 패드를 \n사용해서 움직여보세요.");
        tutorialUI.SetSliderValue(1f); // 슬라이더 초기화
        GameManager.Instance.Player.topDownMovement.isMoving = true;
        StartCoroutine(MovementTimer());
    }

    public override void Execute(TutorialController controller)
    {
        // 슬라이더 업데이트
        tutorialUI.SetSliderValue(movementTimeRemaining / timeLimit);

        if (movementTimeRemaining <= 0)
        {
            GameManager.Instance.Player.topDownMovement.isMoving = false;
            controller.SetNextTutorial(); // 튜토리얼 완료 시 다음 튜토리얼로 이동
        }
    }

    public override void Exit()
    {
        // 튜토리얼 종료 시 플레이어 이동 멈추기
        GameManager.Instance.Player.topDownMovement.isMoving = false;
        tutorialUI.SetSliderValue(0f);
        tutorialUI.tutorialSlider.gameObject.SetActive(false);
    }

    private IEnumerator MovementTimer()
    {
        while (movementTimeRemaining > 0)
        {
            movementTimeRemaining -= Time.deltaTime;
            yield return null;
        }
    }
}
