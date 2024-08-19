using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FishingRod : EquipTool
{
    [SerializeField] private float minTime; // 1f;
    [SerializeField] private float maxTime; // 5f;
    [SerializeField] private float waitTime; // 0.2f;

    private float rodWiatingTime; // 낚싯대를 던지고 기다리는 시간
    private float rodReelingTime; // 낚싯대를 감는 시간
    private bool isFishing = false; // 낚시중일 때 OnTriggerStay 비활성화하기 위함

    private FishingZone fishingZone;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (fishingZone != null && !isFishing && player.inventory.CheckEmptySlot())
        {
            interactionBtn.gameObject.SetActive(true);
        }
        else if (fishingZone != null && isFishing)
        {
            interactionBtn.gameObject.SetActive(false);
        }
        else
        {
            FishingInteractionSet(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(ELayerName.FishingZone.ToString()))
        {
            fishingZone = null;
            ResetInteractButtonFunction();

            if (Canvas_Main.promptText.gameObject.activeSelf)
            {
                Canvas_Main.promptText.gameObject.SetActive(false);
            }
        }
    }

    private void FishingInteractionSet(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(ELayerName.FishingZone.ToString()))
        {
            interactionBtn = Canvas_Main.interactionUI.GetComponent<Button>();
            fishingZone = collision.gameObject.GetComponent<FishingZone>();
            ChangeInteractButtonFunction();
            isFishing = false;
            interactionBtn.gameObject.SetActive(true);
        }
    }

    public override void UseTool()
    {
        if (player.inventory.CheckEmptySlot())
        {
            isFishing = true;
            player.animController.OnStopMoveEvent?.Invoke(); // 걷는 애니메이션 바로 종료
            Canvas_Main.interactionUI.SetActive(false);
            Canvas_Main.blockBoard.SetActive(true);
            GameManager.Instance.Player.topDownMovement.HoldOnMoveSpeed(0f);
            StartCoroutine(WaitFishing());
        }
        else
        {
            Canvas_Main.promptText.gameObject.SetActive(true);
            Canvas_Main.promptText.text = "가방이 가득차서 낚시를 할 수 없습니다";
        }
    }

    private IEnumerator WaitFishing()
    {
        rodWiatingTime = Random.Range(minTime, maxTime);
        player.animController.OnCastingEvent?.Invoke();
        yield return new WaitForSeconds(rodWiatingTime);

        rodReelingTime = Random.Range(minTime, maxTime);
        player.animController.OnReelingEvent?.Invoke();
        yield return new WaitForSeconds(rodReelingTime);

        player.animController.OnCaughtingEvent?.Invoke();
        yield return new WaitForSeconds(waitTime);

        CatchFish();
        GameManager.Instance.Player.topDownMovement.ResetMoveSpeed();
        isFishing = false;
        Canvas_Main.blockBoard.SetActive(false);
    }

    private void CatchFish()
    {
        if (fishingZone != null)
        {
            GameManager.Instance.Player.itemdata = fishingZone.GiveItem(DataManager.Instance.ItemList(EItemDetailType.FISHES));
            //GameManager.Instance.Player.showItem.ShowGetItemInfoBox(GameManager.Instance.Player.itemdata.itemIcon, $"+ 1");
            GameManager.Instance.Player.itemQuantity = 1;
            GameManager.Instance.Player.expLevel.GetExp(GameManager.Instance.Player.itemdata.expValue);
            GameManager.Instance.Player.addItem?.Invoke();
            Canvas_Main.interactionUI.SetActive(true);
        }
    }
}