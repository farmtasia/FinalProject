using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass2 : MonoBehaviour, IInteractable
{
    public GameObject soil;
    public Sprite grassSoil; // 잔디
    public Sprite normalSoil; // 기본 땅
    public Sprite diggedSoil; // 파인 땅

    public GameObject seedDropPrefab; // 씨앗 애니메이션

    public enum SoilState
    {
        Grass,
        Normal,
        Digged
    }

    private SoilState currentState;
    private SeedItemSO currentSeed;

    private void Start()
    {
        currentState = SoilState.Grass; // 잔디상태의 땅
        soil.GetComponent<SpriteRenderer>().sprite = grassSoil;
    }

    public string GetInteractPrompt() // 오브젝트가 감지됐을때 뜨는 프롬프트 문구
    {
        var curTool = GameManager.Instance.Player.equipment.curEquipTool;

        if (curTool != null)
        {
            GameManager.Instance.Player.interaction.OnEquipUI(true); // 인터랙션 하는 곳에 다 붙여줘야됨.
            return ""; // 어떤 도구든 장착되었을 때!
        }
        else
        {
            return "도구를 장착하세요"; // 도구가 장착되지 않았을 때!
        }
    }

    public void OnInteract() // 인터랙트 됐을 때 일어나는 동작
    {
        var curTool = GameManager.Instance.Player.equipment.curEquipTool;

        if (curTool != null && curTool is Shovel)
        {
            if (currentState == SoilState.Grass)
            {
                ChangeSoilState(SoilState.Normal);
            }
            else if (currentState == SoilState.Normal)
            {
                ChangeSoilState(SoilState.Digged);
                Debug.Log("이제 씨앗을 심어보자!");
            }
        }
    }

    public void ChangeSoilState(SoilState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case SoilState.Grass:
                soil.GetComponent<SpriteRenderer>().sprite = grassSoil;
                break;
            case SoilState.Normal:
                soil.GetComponent<SpriteRenderer>().sprite = normalSoil;
                break;
            case SoilState.Digged:
                soil.GetComponent<SpriteRenderer>().sprite = diggedSoil;
                break;
        }
    }
}
