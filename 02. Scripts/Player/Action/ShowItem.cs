using UnityEngine;

public class ShowItem : MonoBehaviour
{
    public GameObject getItemInfoBox; // 획득한 아이템 정보를 표기할 프리팹
    public Transform getItemInfoPos; // 획득한 아이템 정보를 띄울 위치

    public void ShowGetItemInfoBox(Sprite getItemImg, int getItemCount)
    {
        GameObject newGetItemInfoObj = Instantiate(getItemInfoBox, getItemInfoPos);
        GetItemInfoBox newGetItemInfoBox = newGetItemInfoObj.GetComponent<GetItemInfoBox>();
        newGetItemInfoBox.GetItemInfoSet(getItemImg, getItemCount);
        newGetItemInfoObj.SetActive(true);
    }
}
