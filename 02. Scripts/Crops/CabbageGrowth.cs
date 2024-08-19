using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CabbageGrowth : MonoBehaviour, IInteractable
{
    public Sprite[] cropStages; // 작물 단계별 이미지 배열
    private int currentStage = 0; // 현재 단계
    private float growthTime = 600f; // 양배추 성장시간
    private float wateringTime; // 물뿌리개로 물 준 시간
    private int wateringDay; // 물을 준 날짜
    private SpriteRenderer growthCabbage; // 작물(양배추) 스프라이트 렌더러
    private bool canGrowth = false; // 성장 가능 여부

    public GameObject getCabbage; // 수확물 오브젝트(양배추)
    public GameObject soil;

    public Equipment equipment; // 도구 관리
    public EquipTool curTool; // 현재 장착도구

    [SerializeField] private int needItemCode = 1101; // 필요한 아이템 코드. 1101 = 물뿌리개

    // public static event Action OnWateringEvent; // 물을 줄 때 발생하는 이벤트

    private void Awake()
    {
        growthCabbage = transform.GetChild(0).GetComponent<SpriteRenderer>(); // 자식오브젝트 중 0번째를 가져온다
    }

    void Start()
    {
        // getCabbage.SetActive(false); // 초기에는 수확물 오브젝트 비활성화
        equipment = GameManager.Instance.Player.equipment;
        UpdateCropStage(); // 초기 작물 단계 설정.이미지 업데이트.
        wateringTime = TimeManager.Instance.time;
        Debug.Log("1");
    }


    void Update()
    {
        if (canGrowth) // 성장조건 체크
        {
            if (currentStage >= 4) // 4단계까지만 반복
            {
                return;
            }

            if (TimeManager.Instance.time + (86400 * (TimeManager.Instance.days - wateringDay)) - wateringTime >= growthTime) // 현재시간 - 심은시간 >= 성장시간
            {
                currentStage++; // 현재 단계에서 한단계 올려주기 
                // growthTime += growthTime; // 한단계 올라갈때마다 성장시간에 성장시간만큼 더해준다
                UpdateCropStage();
                Debug.Log(currentStage + "단계로 성장했습니다.");
                canGrowth = false;
            }
        }

        //else if (!canGrowth)
        //{
        //    getCabbage.SetActive(true); // 수확물 오브젝트 활성화
        //}
    }

    public void WaterCrop() // 물뿌리개 장착한 상태로 작물에 물줬을때
    {
        if (canGrowth)
        {
            return;
        } // fasle일때만 밑으로 가도록

        // OnWateringEvent?.Invoke();
        GameManager.Instance.Player.animController.OnWateringEvent?.Invoke();
        Debug.Log("물주는 애니메이션");
        wateringDay = TimeManager.Instance.days;
        wateringTime = TimeManager.Instance.time; // 물 준 시간 기록
        canGrowth = true;
    }

    void UpdateCropStage() // 현재 작물 이미지 업데이트
    {
        if (currentStage > 0 && currentStage <= cropStages.Length)
        {
            growthCabbage.sprite = cropStages[currentStage - 1];
        }
    }

    public string GetInteractPrompt()
    {
        curTool = equipment.curEquipTool;

        if (curTool != null && needItemCode == equipment.PlayerNowEquipToolCode())
        {
            if (!canGrowth)
            {
                GameManager.Instance.Player.interaction.OnEquipUI(true); // 인터랙션 하는 곳에 다 붙여줘야됨.(상호작용 버튼을 켜줌. 접근이 편하도록)
                return "도구가 장착됨"; // 물뿌리개가 장착되었을 때!
            }
            else
            {
                GameManager.Instance.Player.interaction.OnEquipUI(false); // 인터랙션 하는 곳에 다 붙여줘야됨.(상호작용 버튼을 켜줌. 접근이 편하도록)
                return "양배추 성장 중";
            }
        }
        else
        {
            return "도구를 장착하세요"; // 물뿌리개가 장착되지 않았을 때!
        }
    }

    public void OnInteract()  // 인터랙트 됐을 때 어떤 효과를 발생시킬건지.
    {
        curTool = equipment.curEquipTool;

        if (currentStage < 4)
        {
            if (curTool != null && needItemCode == equipment.PlayerNowEquipToolCode())
            {
                WaterCrop();
                Debug.Log("작물에 물을 주었습니다. 현재 시각 : " + TimeManager.Instance.time);
            }
            else
            {
                Debug.Log("다른 도구를 장착하세요");
            }
        }
        else
        {
            // getBeet.SetActive(true); // 수확물 오브젝트 활성화
            transform.GetChild(0).gameObject.SetActive(false);
            Instantiate(getCabbage, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
