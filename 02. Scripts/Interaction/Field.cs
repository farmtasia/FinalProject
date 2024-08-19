using System;
using UnityEngine;

public class Field : MonoBehaviour, IInteractable
{
    public Equipment equipment;
    public EquipTool curTool;
    [SerializeField] private int needItemCode = 1102;

    public static event Action OnDigEvent;

    private void Start()
    {
        equipment = GameManager.Instance.Player.equipment;
    }

    public string GetInteractPrompt()
    {
        curTool = equipment.curEquipTool;

        if (curTool != null)
        {
            return "밭 갈기";
        }
        else
        {
            return "삽을 장착하세요";
        }
    }

    public void OnInteract()
    {
        curTool = equipment.curEquipTool;

        if (curTool != null && needItemCode == equipment.PlayerNowEquipToolCode())
        {
            Debug.Log("밭을 갑니다");
            OnDigEvent?.Invoke();
        }
        else
        {
            Debug.Log("이곳에선 농사를 진행할 수 없습니다.");
        }
    }
}
