using System.Collections;
using UnityEngine;

public class BaseGrowth : MonoBehaviour, IInteractable
{
    public Sprite[] cropStages; // 작물 단계별 이미지 배열
    private SpriteRenderer growthCrop; // 작물 스프라이트 렌더러

    public GameObject getCrop; // 수확물 오브젝트
    public GameObject soil; // 필드크롭 (땅파기 과정이 담긴 오브젝트)
    public GameObject cropTextPrefab; // Crop Text Prefab
    public Transform canvasTransform; // Canvas Transform

    public Equipment equipment; // 도구 관리
    public EquipTool curTool; // 현재 장착 도구

    [SerializeField] private int needItemCode = 1101; // 필요한 아이템 코드. 1101 = 물뿌리개

    public ItemSO cropSO; // 작물에 대한 데이터(성장 시간 등 포함)
    public int itemCount = 1;

    private Transform parentObj;
    public GrowthData growthDataDic;
    private string keyName;

    public SEType wateringSoundEffect = SEType.WATERING;

    float timer;

    private void Awake()
    {
        growthCrop = transform.GetChild(0).GetComponent<SpriteRenderer>(); // 자식오브젝트 중 0번째를 가져온다
    }

    void Start()
    {
        if (GameManager.Instance == null || GameManager.Instance.Player == null)
        {
            return;
        }

        equipment = GameManager.Instance.Player.equipment;
        parentObj = transform.parent;
        keyName = $"{cropSO.itemCode}/{gameObject.transform.position.ToString("F2")}";

        if (!DataManager.Instance.growthDataDic.ContainsKey(keyName))
            Init();

        growthDataDic = DataManager.Instance.growthDataDic[keyName];
        UpdateCropStage(); // 초기 작물 단계 설정, 이미지 업데이트
    }

    private void Init()
    {
        GrowthData growthData = new GrowthData(cropSO.itemCode, gameObject.transform.position, keyName, 0, 0, 0, false);
        DataManager.Instance.growthDataDic[keyName] = growthData;
        DataManager.Instance.curData.growthData.Add(growthData);
    }

    void Update()
    {
        if (growthDataDic.canGrowth)
        {
            if (growthDataDic.currentStage >= 4) // 최종 단계에 도달했는지 확인
            {
                return;
            }

            // 물 준 후 경과한 시간 계산
            float elapsedTime = (TimeManager.Instance.time + (TimeManager.Instance.days - growthDataDic.wateringDay) * 86400f) - growthDataDic.wateringTime;

            // 현재 성장 단계에서 필요한 고정된 시간
            float targetGrowthTime = cropSO.harvestSecond;

            // 경과한 시간에 따라 성장 단계 조정
            while (elapsedTime >= targetGrowthTime && growthDataDic.currentStage < 4)
            {
                growthDataDic.currentStage++; // 한 단계 성장
                elapsedTime -= targetGrowthTime;
                UpdateCropStage(); // 작물 이미지 업데이트
                growthDataDic.wateringTime = TimeManager.Instance.time + (TimeManager.Instance.days - growthDataDic.wateringDay) * 86400f; // 다음 성장 시작 시점으로 설정
                Debug.Log(growthDataDic.currentStage + "단계로 성장했습니다.");
            }
        }

        if (timer != 0)
        {
            timer -= Time.deltaTime * 60f;
        }
    }

    public void WaterCrop() // 물뿌리개 장착한 상태로 작물에 물줬을때
    {
        if (growthDataDic.canGrowth)
        {
            return;
        } // 성장 중이 아닐 때만 물을 줄 수 있음

        StartCoroutine(WateringAnim());
        Debug.Log("물주는 애니메이션");
        growthDataDic.wateringDay = TimeManager.Instance.days;
        growthDataDic.wateringTime = TimeManager.Instance.time; // 물 준 시간 기록
        growthDataDic.canGrowth = true; // 성장 가능 여부를 true로 설정
    }

    void UpdateCropStage() // 현재 작물 이미지 업데이트
    {
        if (growthDataDic.currentStage > 0 && growthDataDic.currentStage <= cropStages.Length)
        {
            growthCrop.sprite = cropStages[growthDataDic.currentStage - 1];
        }
    }

    public string GetInteractPrompt()
    {
        if (equipment == null)
        {
            Debug.LogError("장비가 초기화되지 않았습니다.");
            return "";
        }

        curTool = equipment.curEquipTool;

        // 물이 이미 주어진 상태라면 장착된 도구에 관계없이 타이머를 표시
        if (growthDataDic.canGrowth)
        {
            GameManager.Instance.Player.interaction.OnEquipUI(false); // 인터랙션 버튼 비활성화

            // 현재 단계에서의 남은 성장시간 계산
            float elapsedTime = (TimeManager.Instance.time + (TimeManager.Instance.days - growthDataDic.wateringDay) * 86400f) - growthDataDic.wateringTime;
            float currentStageRemainingTime = cropSO.harvestSecond - elapsedTime;

            // 다음 단계 이후의 남은시간 계산
            float totalRemainingTime = currentStageRemainingTime + cropSO.harvestSecond * (3 - growthDataDic.currentStage);

            // 남은 시간을 시간과 분으로 변환
            int hoursRemaining = (int)(totalRemainingTime / 3600f);
            int minutesRemaining = (int)((totalRemainingTime % 3600f) / 60f);

            // 성장이 완료된 경우
            if (hoursRemaining <= 0 && minutesRemaining <= 0)
            {
                return "작물 성장 완료";
            }

            return $"성장 완료까지 {hoursRemaining}시간 {minutesRemaining}분 남음";
        }

        // 물이 아직 주어지지 않았을 때만 도구 확인
        if (curTool != null && needItemCode == equipment.PlayerNowEquipToolCode())
        {
            GameManager.Instance.Player.interaction.OnEquipUI(true); // 인터랙션 버튼 활성화
            return ""; // 물뿌리개가 장착되었을 때
        }
        else
        {
            return "물뿌리개를 장착하세요"; // 물뿌리개가 장착되지 않았을 때
        }
    }

    public void OnInteract()  // 인터랙트 됐을 때 어떤 효과를 발생시킬건지.
    {
        if (equipment == null)
        {
            Debug.LogError("장비가 초기화되지 않았습니다.");
            return;
        }

        curTool = equipment.curEquipTool;

        if (growthDataDic.currentStage < 4)
        {
            if (curTool != null && needItemCode == equipment.PlayerNowEquipToolCode())
            {
                WaterCrop();
                timer = 3600;
                Debug.Log("작물에 물을 주었습니다. 현재 시각 : " + TimeManager.Instance.time);
            }
            else
            {
                Debug.Log("다른 도구를 장착하세요");
            }
        }
        else
        {
            GameManager.Instance.Player.expLevel.GetExp(cropSO.expValue);
            AddCropToInventory();
            Debug.Log("획득한 작물은 인벤토리에서 확인할 수 있습니다");
            //ShowCropText("+1");
            Soil();
            Destroy(gameObject);
            // 오브젝트가 없어졌으므로 데이터도 딕셔너리, 리스트에서 제거
            DataManager.Instance.growthDataDic.Remove(keyName);
            for (int i = 0; i < DataManager.Instance.curData.growthData.Count; i++)
            {
                if (DataManager.Instance.curData.growthData[i].keyName == keyName)
                {
                    DataManager.Instance.curData.growthData.RemoveAt(i);
                }
            }
        }
    }

    public void Soil()
    {
        Instantiate(soil, transform.position, transform.rotation, parentObj);
    }

    private void AddCropToInventory()
    {
        if (GameManager.Instance.Player == null)
        {
            Debug.LogError("Player is null");
            return;
        }

        if (cropSO == null)
        {
            Debug.LogError("itemData is not set in the inspector.");
            return;
        }

        if (GameManager.Instance.Player.addItem == null)
        {
            Debug.LogError("Add item is null");
            return;
        }

        for (int i = 1; i <= itemCount; i++)
        {
            GameManager.Instance.Player.itemdata = cropSO;
            GameManager.Instance.Player.itemQuantity = i;
            GameManager.Instance.Player.addItem?.Invoke();
        }
    }

    private void ShowCropText(string message)
    {
        if (cropTextPrefab != null && canvasTransform != null)
        {
            // 인스턴스화할 때 부모를 설정하지 않음
            GameObject cropTextInstance = Instantiate(cropTextPrefab);
            cropTextInstance.transform.position = transform.position;
            // 인스턴스화한 후 부모를 설정함
            // cropTextInstance.transform.SetParent(canvasTransform, false);

            GetCropTxt cropText = cropTextInstance.GetComponentInChildren<GetCropTxt>();
            cropText.ShowText(message); // GetCropTxt 스크립트
        }
        else
        {
            Debug.LogError("cropTextPrefab or canvasTransform is null");
        }
    }

    private IEnumerator WateringAnim()
    {
        GameManager.Instance.Player.topDownMovement.HoldOnMoveSpeed(0f);
        GameManager.Instance.Player.animController.OnWateringEvent?.Invoke();
        SoundManager.Instance.PlayEffect(wateringSoundEffect);
        yield return new WaitForSeconds(1.3f); // 우선 임의 설정
        GameManager.Instance.Player.topDownMovement.ResetMoveSpeed();
    }
}