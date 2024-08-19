using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupStore : MonoBehaviour, IInteractable
{
    //public GameObject storeUI; // 테스트용

    public string GetInteractPrompt()
    {
        string str = "Store";
        return str;
    }

    public void OnInteract()
    {
        //storeUI.SetActive(true);
        //storeUI.GetComponent<UIStore>().ResetInventorySlots();

        if (DataManager.Instance.CheckUIDictionary("UIStore"))// / ui딕셔너리에 UIStore 있는지 확인 
        {
            UIStore storeUI = DataManager.Instance.GetUIInDictionary("UIStore") as UIStore; // UIStore의 메서드에 접근하기 위해 변환
            if (storeUI != null)
            {
                storeUI.gameObject.SetActive(true); // 상점ui 열고
                storeUI.ResetInventorySlots(); // 상점인벤토리슬롯 초기화
            }
            else
            {
                Debug.LogError("UIStore 인스턴스를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("UIStore가 UI 딕셔너리에 없습니다.");
        }
    }
}
