using UnityEngine;

public class Work : MonoBehaviour, IInteractable
{
    public CropSO cropSO;

    public string GetInteractPrompt()
    {
        string str = cropSO.name;
        return str;
    }

    public void OnInteract()
    {
        Working();
    }

    public void Working()
    {
        //Debug.Log("Working 실행");
        //GameManager.Instance.Player.GetComponent<PlayerExpLevel>().GetExp(cropSO.expValue);
        //Debug.Log($"수확물 : {cropSO.name} 획득 경험치 : {cropSO.expValue}");

        Debug.Log("Working 호출");
        if (GameManager.Instance != null && GameManager.Instance.Player != null)
        {
            

            PlayerExpLevel playerExpLevel = GameManager.Instance.Player.GetComponent<PlayerExpLevel>();
            if (playerExpLevel != null)
            {
                playerExpLevel.GetExp(cropSO.expValue);
                Debug.Log($"수확물 : {cropSO.name} 획득 경험치 : {cropSO.expValue}");
            }
            else
            {
                Debug.LogError("PlayerExpLevel component is null!");
            }
        }
        else
        {
            Debug.LogError("GameManager.Instance or GameManager.Instance.Player is null!");
        }
    }
}
