using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : TutorialBase
{
    [SerializeField] private TopDownMovement playerCtrl;
    [SerializeField] private Transform triggerObject;

    public bool isTrigger { get; set; } = false;

    public override void Enter()
    {
        // 플레이어 이동
        playerCtrl.isMoving = true;

        triggerObject.gameObject.SetActive(true);
    }

    public override void Execute(TutorialController controller)
    {
        // TutorialTrigger오브젝트의 위치를 플레이어와 동일하게 설정(트리거 오브젝트와 충돌할 수 있도록)
        transform.position = playerCtrl.transform.position;

        if (isTrigger == true)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        // 플레이어 이동 불가능
        playerCtrl.isMoving = false;

        triggerObject.gameObject.SetActive(false);
    }
}
