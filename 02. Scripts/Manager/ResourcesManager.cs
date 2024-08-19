using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourcesManager : Singleton<ResourcesManager>
{
    // 특정 경로에서 데이터를 가져오는 것은 모두 리소스 매니저에서 실행
    // TODO: 암호화
    // Application.persistentDataPath: 사용하고 있는 기기의 운영 체제에 따른 적정 저장 경로를 자동으로 찾아줌
    private string savePath;

    protected override void Awake()
    {
        base.Awake();
        savePath = Application.persistentDataPath;
    }

    private void Start()
    {
        LoadNPCData("npcData"); //  NPC 데이터 로드
    }

    public void SaveData()     // 모든 캐릭터 데이터 저장
    {
        List<CharacterData> characterList = DataManager.Instance.CharacterDicToList();
        UserData data = new UserData(characterList);
        File.WriteAllText(savePath + $"/UserData.txt", JsonUtility.ToJson(data));     // JSON으로 직렬화하여 타입 명으로 파일 저장
        Debug.Log("저장 완료: " + savePath + "/UserData.txt");
    }

    public UserData LoadData()    // 모든 캐릭터 데이터 불러오기
    {
        if (!File.Exists(savePath + "/UserData.txt"))
            return JsonUtility.FromJson<UserData>(null);

        string jsonData = File.ReadAllText(savePath + "/UserData.txt");
        Debug.Log("불러오기 완료: " + savePath + "/UserData.txt");
        return JsonUtility.FromJson<UserData>(jsonData);
    }

    // JSON 파일에서 NPC 데이터 로드
    public void LoadNPCData(string fileName)
    {
        // Resources 폴더에서 파일 로드
        TextAsset jsonData = Resources.Load<TextAsset>($"NPCData/{fileName}");

        if (jsonData != null) // JSON 데이터가 로드 됐으면
        {
            // JSON 문자열을 NPCDataList 객체로 변환
            NPCDefaultList npcDataList = JsonUtility.FromJson<NPCDefaultList>(jsonData.text);

            foreach (NPCDefault npcData in npcDataList.npcList)
            {
                // NPC ID를 키로 사용하여 npcDataDictionary에 NPC 데이터 저장
                DataManager.Instance.npcDefaultDic[npcData.npcID] = npcData;
                Debug.Log($"NPC 데이터 로드됨: {npcData.npcID}, {npcData.name}");
            }
        }
        else
        {
            Debug.LogError($"NPC 데이터 파일 로드 실패: {fileName}");
        }
    }

    public BaseUI LoadUIObject(UIs type, string uiName)
    {
        BaseUI obj = Instantiate(Resources.Load<BaseUI>($"UI/{type}/{uiName}"));
        return obj;
    }
}