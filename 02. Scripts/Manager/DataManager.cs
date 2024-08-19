using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : Singleton<DataManager>
{
    // TODO: 메서드를 활용해서 해당 스크립트 내에서만 데이터 변동이 있도록 리팩토링
    public UserData userDataList { get; private set; }
    public Dictionary<string, CharacterData> userDataDic { get; private set; }
    public CharacterData curData { get; private set; }    // 플레이 중인 데이터

    public Dictionary<string, BaseUI> uiDic = new Dictionary<string, BaseUI>();    // UI객체
    public Dictionary<int, NPCDefault> npcDefaultDic = new Dictionary<int, NPCDefault>();    // 기본 NPC
    public Dictionary<int, NPCData> npcDataDic = new Dictionary<int, NPCData>();    // 변화 NPC
    public Dictionary<string, TreeData> treeDataDic = new Dictionary<string, TreeData>();    // 과일 나무
    public Dictionary<string, FieldData> fieldDataDic = new Dictionary<string, FieldData>();    // 작물 땅
    public Dictionary<string, GrowthData> growthDataDic = new Dictionary<string, GrowthData>();    // 작물 성장

    [SerializeField] private List<ItemSO> FruitsList;
    [SerializeField] private List<ItemSO> FishList;
    [SerializeField] private ItemSO shovel;
    [SerializeField] private ItemSO watering;
    [SerializeField] private ItemSO beetSeed;
    [SerializeField] private ItemSO carrotSeed;
    [SerializeField] private ItemSO potatoSeed;
    [SerializeField] private ItemSO pumpkinSeed;

    protected override void Awake()
    {
        base.Awake();
        userDataList = ResourcesManager.Instance.LoadData();
        if (userDataList != null && userDataList.user.Count > 0)
        {
            // 불러올 데이터가 있다면 캐릭터 리스트를 딕셔너리로 변환
            CharacterListToDic();
        }
        else
        {
            // 없다면 리스트, 딕셔너리 초기화
            userDataList = new UserData(new List<CharacterData>());
            userDataDic = new Dictionary<string, CharacterData>();
        }

        // 기본 값 세팅
        curData = new CharacterData(
            new NameData("UserName", "FarmName"),
            new PlayerData(100, 0, new Vector2(-16.9f, 14.3f), 1, false),
            new TimeData(32400, 0),
            new List<SlotData> { new SlotData(shovel, 1, 0, false, false, 0), new SlotData(watering, 1, 1, false, false, 0), new SlotData(beetSeed, 3, 2, false, false, 0), new SlotData(carrotSeed, 3, 3, false, false, 0), new SlotData(potatoSeed, 3, 4, false, false, 0), new SlotData(pumpkinSeed, 3, 5, false, false, 0) },
            SceneName.FarmScene.ToString(),
            new List<NPCData>(),
            new List<TreeData>(),
            new List<FieldData>(),
            new List<GrowthData>(),
            new CollectionData(new List<ItemSO>()));

        // 유저 테스트 이후 삭제
        //if (userDataList.user.Count == 0)
        //{
        //    DefaultData();
        //    CharacterListToDic();
        //}
    }

    private void DefaultData()    // ichi.io 모바일용 기본 데이터
    {
        CharacterData defaultData = new CharacterData(
            new NameData("기본 캐릭터", "모바일"),
            new PlayerData(100, 0, new Vector2(-16.9f, 14.3f), 1, false),
            new TimeData(32400, 0),
            new List<SlotData> { new SlotData(shovel, 1, 0, false, false, 0), new SlotData(watering, 1, 1, false, false, 0), new SlotData(pumpkinSeed, 1, 2, false, false, 0) },
            SceneName.FarmScene.ToString(),
            new List<NPCData>(),
            new List<TreeData>(),
            new List<FieldData>(),
            new List<GrowthData>(),
            new CollectionData(new List<ItemSO>()));
        userDataList.user.Add(defaultData);
    }

    // TODO: 제네릭 메서드로 변경 시도
    // 유저의 캐릭터
    private void CharacterListToDic()
    {
        Dictionary<string, CharacterData> dataDictionary = new Dictionary<string, CharacterData>();
        foreach (var cha in userDataList.user)
        {
            dataDictionary[cha.nameData.userName] = cha;
        }
        userDataDic = dataDictionary;
    }

    public List<CharacterData> CharacterDicToList()
    {
        // 데이터를 이전 리스트와 동일한 순서로 정렬
        List<CharacterData> chaList = new List<CharacterData>();
        foreach (var cha in userDataList.user)
        {
            if (userDataDic.TryGetValue(cha.nameData.userName, out CharacterData chaData))
            {
                chaList.Add(chaData);
            }
        }
        return chaList;
    }

    // NPC
    public void NPCListToDic()
    {
        foreach (var npc in curData.npcData)
        {
            npcDataDic[npc.npcID] = npc;
        }
    }

    public void NPCDicToList()
    {
        List<NPCData> npcList = new List<NPCData>();
        foreach (var npc in curData.npcData)
        {
            if (npcDataDic.TryGetValue(npc.npcID, out NPCData npcData))
            {
                npcList.Add(npcData);
            }
        }
        curData.npcData = npcList;
    }

    // FruitTree
    public void TreeListToDic()
    {
        foreach (var tree in curData.treeData)
        {
            treeDataDic[tree.treeName] = tree;
        }
    }

    public void TreeDicToList()
    {
        List<TreeData> treeList = new List<TreeData>();
        foreach (var tree in curData.treeData)
        {
            if (treeDataDic.TryGetValue(tree.treeName, out TreeData treeData))
            {
                treeList.Add(treeData);
            }
        }
        curData.treeData = treeList;
    }

    // Field
    public void FieldListToDic()
    {
        foreach (var field in curData.fieldData)
        {
            fieldDataDic[field.keyName] = field;
        }
    }

    public void FieldDicToList()
    {
        List<FieldData> fieldList = new List<FieldData>();
        foreach (var field in curData.fieldData)
        {
            if (fieldDataDic.TryGetValue(field.keyName, out FieldData fieldData))
            {
                fieldList.Add(fieldData);
            }
        }
        curData.fieldData = fieldList;
    }

    // Growh
    public void GrowhListToDic()
    {
        foreach (var growth in curData.growthData)
        {
            growthDataDic[growth.keyName] = growth;
        }
    }

    public void GrowhDicToList()
    {
        List<GrowthData> growthList = new List<GrowthData>();
        foreach (var growth in curData.growthData)
        {
            if (growthDataDic.TryGetValue(growth.keyName, out GrowthData growthData))
            {
                growthList.Add(growthData);
            }
        }
        curData.growthData = growthList;
    }

    // 새로하기에서 호출
    public void StartSetting()
    {
        // 현재 캐릭터 값을 리스트, 딕셔너리에 추가
        userDataList.user.Add(curData);
        userDataDic[curData.nameData.userName] = curData;
    }

    // 불러오기 슬롯에서 호출
    public void CurDataSetting(string name)
    {
        curData = userDataDic[name];
        NPCListToDic();
        TreeListToDic();
        FieldListToDic();
        GrowhListToDic();
    }

    // 딕셔너리에 현재 데이터 업데이트
    public void CurDataUpdate()
    {
        if (SceneManager.GetActiveScene().name != SceneName.StartScene.ToString())
        {
            GameManager.Instance.Player.ReturnData();
            GameManager.Instance.Player.inventory.ReturnData();
            curData.sceneData = SceneManager.GetActiveScene().name;
            NPCDicToList();
            TreeDicToList();
            FieldDicToList();
            GrowhDicToList();
            userDataDic[curData.nameData.userName] = curData;
        }
    }

    // 다른 클래스에서 데이터를 사용할 때 호출하는 메서드
    public List<SlotData> SetInventoryData() { return curData.inventoryData; }

    // 다른 클래스에서 데이터를 받아올 때 호출하는 메서드
    public void GetNameData(NameData getData) { curData.nameData = getData; }
    public void GetInventoryData(List<SlotData> getData) { curData.inventoryData = getData; }

    public List<ItemSO> ItemList(EItemDetailType detailType)
    {
        switch (detailType)
        {
            case EItemDetailType.FRUITS:
                return FruitsList;
            case EItemDetailType.FISHES:
                return FishList;
            default:
                return null;
        }
    }

    public BaseUI GetUIInDictionary(string uiName) // uiDictionary에서 uiName 키에 해당하는 BaseUI 객체 반환
    {
        return uiDic[uiName];
    }

    public bool CheckUIDictionary(string uiName) // uiDictionary에서 uiName 키가 있는지 확인 후 결과 반환
    {
        return uiDic.ContainsKey(uiName);
    }

    public void AddUIInDictionary(string uiName, BaseUI obj) // uiDictionary에 uiName 키와 BaseUI 객체 추가
    {
        uiDic.Add(uiName, obj);
    }

    // 씬 전환 시 호출하여 딕셔너리 초기화 시켜줌
    public void Clear() // 씬로드매니저에서 로드씬하기 전에 호출해주면 될 것 같음 / 아니면 베이스씬에서 해주거나...
    {
        uiDic.Clear();
    }

    public NPCDefault GetNPCData(int npcID) // NPC ID를 통해 NPC 데이터 가져오기
    {
        // npcDataDictionary에서 npcID 키를 사용하여 NPC 데이터 검색
        if (npcDefaultDic.TryGetValue(npcID, out NPCDefault npcData))
        {
            // 있으면 반환
            return npcData;
        }
        else
        {
            Debug.LogError($"NPC 데이터 없음: {npcID}");
            return null;
        }
    }
}