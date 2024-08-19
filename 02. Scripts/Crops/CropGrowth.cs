using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CropGrowth : MonoBehaviour, IInteractable
{
    public Sprite[] cropStages; // 작물 단계별 이미지 배열
    private int currentStage = 0; // 현재 단계
    private float growthTime = 300f; // 비트의 성장시간. =각 단계로 성장하는데 걸리는 시간
    private float wateringTime; // 물뿌리개로 물 준 시간
    private int wateringDay; // 물을 준 날짜
    private SpriteRenderer growthBeet; // 작물(비트) 스프라이트 렌더러
    private bool canGrowth = false; // 성장 가능 여부

    public GameObject getBeet; // 수확물 오브젝트(비트)
    public GameObject soil; // 필드크롭 (땅파기 과정이 담긴 오브젝트)
    public GameObject cropTextPrefab; // Crop Text Prefab
    public Transform canvasTransform; // Canvas Transform

    public Equipment equipment; // 도구 관리
    public EquipTool curTool; // 현재 장착도구

    [SerializeField] private int needItemCode = 1101; // 필요한 아이템 코드. 1101 = 물뿌리개

    public ItemSO itemData;
    public int itemCount = 1;

    private void Awake()
    {
        growthBeet = transform.GetChild(0).GetComponent<SpriteRenderer>(); // 자식오브젝트 중 0번째를 가져온다
    }

    void Start()
    {
        equipment = GameManager.Instance.Player.equipment;
        UpdateCropStage(); // 초기 작물 단계 설정.이미지 업데이트.
        wateringTime = TimeManager.Instance.time;
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
                UpdateCropStage();
                Debug.Log(currentStage + "단계로 성장했습니다.");
                canGrowth = false;
            }
        }
    }

    public void WaterCrop() // 물뿌리개 장착한 상태로 작물에 물줬을때
    {
        if (canGrowth)
        {
            return;
        } // fasle일때만 밑으로 가도록

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
            growthBeet.sprite = cropStages[currentStage - 1];
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
                return "600초 안지남";
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
            AddCropToInventory();
            Debug.Log("획득한 작물은 인벤토리에서 확인할 수 있습니다");
            ShowCropText("작물+1");
            Soil();
            Destroy(gameObject);
        }
    }

    public void Soil()
    {
        Instantiate(soil, transform.position, transform.rotation);
    }

    private void AddCropToInventory()
    {
        for (int i = 1; i <= itemCount; i++)
        {
            GameManager.Instance.Player.itemdata = itemData;
            GameManager.Instance.Player.addItem?.Invoke();
        }
    }

    private void ShowCropText(string message) // 텍스트 표시
    {
        GameObject cropTextInstance = Instantiate(cropTextPrefab, canvasTransform);
        GetCropTxt cropText = cropTextInstance.GetComponent<GetCropTxt>();
        cropText.ShowText(message); // GetCropTxt 스크립트
    }
}
