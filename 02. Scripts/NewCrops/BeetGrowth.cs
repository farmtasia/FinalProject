using System.Collections;
using UnityEngine;

public class BeetGrowth : MonoBehaviour, IInteractable
{
    public Sprite[] cropStages; // 작물 단계별 이미지 배열
    private float growthTime = 3600f; // 비트의 성장시간. (36000f)
    private SpriteRenderer growthBeet; // 작물(비트) 스프라이트 렌더러
    // currentStage: 현재 단계
    // wateringTime: 물뿌리개로 물 준 시간
    // wateringDay: 물을 준 날짜
    // canGrowth: 성장 가능 여부

    public GameObject getBeet; // 수확물 오브젝트(비트)
    public GameObject soil; // 필드크롭 (땅파기 과정이 담긴 오브젝트)
    public GameObject cropTextPrefab; // Crop Text Prefab
    public Transform canvasTransform; // Canvas Transform

    public Equipment equipment; // 도구 관리
    public EquipTool curTool; // 현재 장착도구

    [SerializeField] private int needItemCode = 1101; // 필요한 아이템 코드. 1101 = 물뿌리개

    public ItemSO cropSO;
    public int itemCount = 1;

    private Transform parentObj;
    public GrowthData growthDataDic;
    private string keyName;

    public SEType wateringSoundEffect = SEType.WATERING;

    private void Awake()
    {
        growthBeet = transform.GetChild(0).GetComponent<SpriteRenderer>(); // 자식오브젝트 중 0번째를 가져온다
    }

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null.");
            return;
        }

        if (GameManager.Instance.Player == null)
        {
            Debug.LogError("GameManager.Instance.Player is null.");
            return;
        }

        equipment = GameManager.Instance.Player.equipment;
        parentObj = transform.parent;
        keyName = $"{cropSO.itemCode}/{gameObject.transform.position.ToString("F2")}";
        if (!DataManager.Instance.growthDataDic.ContainsKey(keyName))
            Init();
        growthDataDic = DataManager.Instance.growthDataDic[keyName];

        if (cropSO == null)
        {
            Debug.LogError("itemData is not set in the inspector.");
        }

        UpdateCropStage(); // 초기 작물 단계 설정.이미지 업데이트.
    }

    private void Init()
    {
        GrowthData growthData = new GrowthData(cropSO.itemCode, gameObject.transform.position, keyName, 0, 0, 0, false);
        DataManager.Instance.growthDataDic[keyName] = growthData;
        DataManager.Instance.curData.growthData.Add(growthData);
    }

    void Update()
    {
        if (growthDataDic.canGrowth) // 성장조건 체크
        {
            if (growthDataDic.currentStage >= 4) // 4단계까지만 반복
            {
                return;
            }

            if (TimeManager.Instance.time + (86400 * (TimeManager.Instance.days - growthDataDic.wateringDay)) - growthDataDic.wateringTime >= growthTime) // 현재시간 - 심은시간 >= 성장시간
            {
                growthDataDic.currentStage++; // 현재 단계에서 한단계 올려주기 
                UpdateCropStage();
                Debug.Log(growthDataDic.currentStage + "비트가 성장했습니다.");
                growthDataDic.canGrowth = false;
            }
        }
    }

    public void WaterCrop() // 물뿌리개 장착한 상태로 작물에 물줬을때
    {
        if (growthDataDic.canGrowth)
        {
            return;
        } // fasle일때만 밑으로 가도록

        //GameManager.Instance.Player.animController.OnWateringEvent?.Invoke();
        StartCoroutine(WateringAnim());
        Debug.Log("물주는 애니메이션");
        growthDataDic.wateringDay = TimeManager.Instance.days;
        growthDataDic.wateringTime = TimeManager.Instance.time; // 물 준 시간 기록
        growthDataDic.canGrowth = true;
    }

    void UpdateCropStage() // 현재 작물 이미지 업데이트
    {
        if (growthDataDic.currentStage > 0 && growthDataDic.currentStage <= cropStages.Length)
        {
            growthBeet.sprite = cropStages[growthDataDic.currentStage - 1];
        }
    }

    public string GetInteractPrompt()
    {
        if (equipment == null)
        {
            Debug.LogError("");
            return "";
        }

        curTool = equipment.curEquipTool;

        if (curTool != null && needItemCode == equipment.PlayerNowEquipToolCode())
        {
            if (!growthDataDic.canGrowth)
            {
                GameManager.Instance.Player.interaction.OnEquipUI(true); // 인터랙션 하는 곳에 다 붙여줘야됨.(상호작용 버튼을 켜줌. 접근이 편하도록)
                return ""; // 물뿌리개가 장착되었을 때!
            }
            else
            {
                GameManager.Instance.Player.interaction.OnEquipUI(false); // 인터랙션 하는 곳에 다 붙여줘야됨.(상호작용 버튼을 켜줌. 접근이 편하도록)
                return "성장중에는 물을 줄 수 없습니다";
            }
        }
        else
        {
            return "물뿌리개를 장착하세요"; // 물뿌리개가 장착되지 않았을 때!
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
            ShowCropText("+1");
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
            GameManager.Instance.Player.addItem?.Invoke();
        }
    }

    private void ShowCropText(string message)
    {
        if (cropTextPrefab != null && canvasTransform != null)
        {
            Debug.Log("확인!!!");
            // 인스턴스화할 때 부모를 설정하지 않음
            GameObject cropTextInstance = Instantiate(cropTextPrefab);

            // 인스턴스화한 후 부모를 설정함
            cropTextInstance.transform.SetParent(canvasTransform, false);

            GetCropTxt cropText = cropTextInstance.GetComponent<GetCropTxt>();
            cropText.ShowText(message); // GetCropTxt 스크립트
        }
        else
        {
            Debug.LogError("텍스트가 없습니다");
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