using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public PlayerInputController controller;
    public TopDownMovement topDownMovement;
    public PlayerExpLevel expLevel;
    public PlayerGold gold;
    public Interaction interaction;
    public Inventory inventory;
    public QuickSlotInven quickSlotInven;
    public ShowItem showItem;

    public ItemSO itemdata;
    public int itemQuantity;
    public Action addItem;
    public Equipment equipment;
    public Transform dropPosition;
    public Transform equipToolPosition; // 추후 수정하기
    public Transform levelUpEffectPosition; // 레벨업이 표시될 위치

    //public bool firstOpenGuide = false;

    public CharacterAnimationController animController;

    public bool hasCompletedTutorial;


    private void Awake()
    {
        GameManager.Instance.Player = this; // 게임 매니저에 현재 플레이어 인스턴스를 등록
        controller = GetComponent<PlayerInputController>();
        topDownMovement = GetComponent<TopDownMovement>();
        expLevel = GetComponent<PlayerExpLevel>();
        gold = GetComponent<PlayerGold>();
        interaction = GetComponent<Interaction>();
        equipment = GetComponent<Equipment>();
        animController = GetComponent<CharacterAnimationController>();
        showItem = GetComponent<ShowItem>();
    }

    public void ApplyData()    // 데이터 매니저에서 가져온 데이터 적용
    {
        this.transform.position = DataManager.Instance.curData.playerData.pos;    // 마지막 위치
    }

    public void ReturnData()    // 데이터 매니저로 현재 데이터 전달
    {
        DataManager.Instance.curData.playerData.pos = this.transform.position;
    }
}