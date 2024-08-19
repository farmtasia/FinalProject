using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBox : MonoBehaviour, IInteractable
{
    // 작물오브젝트 CropGrowth

    public GameObject whiteBox; // WhiteBox
    public Equipment equipment;
    public EquipTool curTool;

    void Start()
    {
        equipment = GameManager.Instance.Player.equipment;
        gameObject.SetActive(false); // 초기에는 비활성화
    }

    public string GetInteractPrompt() // 오브젝트가 감지됐을때 뜨는 프롬프트 문구
    {
        return "블랙박스 감지";
    }

    public void OnInteract() // 인터랙트 됐을 때 일어나는 동작
    {
        curTool = equipment.curEquipTool;

        if (curTool != null && curTool is Watering)
        {
            gameObject.SetActive(false); // 나자신 비활성화
            whiteBox.SetActive(true); // 화이트박스 활성화
        }

        else
        {
            gameObject.SetActive(true);
            whiteBox.SetActive(false);
        }
            
    }

}
