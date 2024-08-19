using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Watering : EquipTool
{
    [SerializeField] private float waitTime; // 2.4f;
    [SerializeField] private float wateringExp; // 물뿌리개 사용에 따른 경험치
    private FruitTree curFruitTree;

    public SEType wateringSoundEffect = SEType.WATERING;

    private void OnTriggerStay2D(Collider2D collision)
    {
        TriggerEnterFruitTree(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(ELayerName.FruitTree.ToString()))
        {
            curFruitTree = null;
            ResetInteractButtonFunction();
        }
    }

    public override void UseTool()
    {
        if (curFruitTree != null && curFruitTree.treeDataDic.isDrink) return;

        interactionBtn.gameObject.SetActive(false);
        GameManager.Instance.Player.expLevel.GetExp(wateringExp);
        GameManager.Instance.Player.topDownMovement.HoldOnMoveSpeed(0f);
        player.animController.OnStopMoveEvent?.Invoke();
        player.animController.OnWateringEvent?.Invoke();
        SoundManager.Instance.PlayEffect(wateringSoundEffect);

        if (curFruitTree != null)
        {
            ChooseInteractType(ELayerName.FruitTree);
        }
    }

    private void TriggerEnterFruitTree(Collider2D collision)
    {
        if (curFruitTree != null && !curFruitTree.treeDataDic.isDrink)
        {
            interactionBtn.gameObject.SetActive(true);
        }
        else if (curFruitTree != null && curFruitTree.treeDataDic.isDrink)
        {
            player.interaction.otherCase = false;
            //interactionBtn.gameObject.SetActive(false);
        }
        else if (curFruitTree == null && collision.gameObject.CompareTag(ELayerName.FruitTree.ToString()))
        {
            curFruitTree = collision.gameObject.GetComponent<FruitTree>();
            interactionBtn = Canvas_Main.interactionUI.GetComponent<Button>();
            ReadyForWater();
        }
    }

    private void ReadyForWater()
    {       
        player.interaction.curInteractable = null;
        ChangeInteractButtonFunction();
        //interactionBtn.gameObject.SetActive(true);
    }

    private void ChooseInteractType(ELayerName layerName)
    {
        // 농작물과 상호작용을 구현하려면
        // 1) LayerMask 추가 및 구현하고자 하는 곳에 레이어마스크 설정
        // 2) ELayerName 에 선언 및 초기화
        // 3) 해당 메서드 구현

        switch (layerName)
        {
            case ELayerName.FruitTree:
                InteractFruitTree();
                break;
            default:
                Debug.Log("아무것도 실행되지 않음");
                break;
        }
    }

    private void InteractFruitTree()
    {
        if (curFruitTree.treeDataDic.isDrink) return;

        StartCoroutine(InteractionButtonOff(waitTime));
        curFruitTree.treeDataDic.isDrink = true;
        curFruitTree.treeDataDic.drinkDay = TimeManager.Instance.days;
        curFruitTree.treeDataDic.drinkTime = TimeManager.Instance.time;
        // TODO : 추후 물약여부와 함께 물뿌리개와 조합하여 사용할 수 있도록 하기
    }

    private IEnumerator InteractionButtonOff(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.Instance.Player.topDownMovement.ResetMoveSpeed();
        Canvas_Main.blockBoard.SetActive(false);
        interactionBtn.gameObject.SetActive(true);
    }
}