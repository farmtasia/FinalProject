using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadDataSlot : MonoBehaviour
{
    [SerializeField] private Image characterImage;    // TODO: 이미지 추가하기
    [SerializeField] private TextMeshProUGUI userName;
    [SerializeField] private TextMeshProUGUI farmName;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI daysAndTime;
    [SerializeField] private Button thisButton;
    [SerializeField] private Button deleteButton;
    private GameObject deletePopUpObj;
    public CharacterData characterData;

    private void Start()
    {
        thisButton.onClick.AddListener(ClickData);
        deleteButton.onClick.AddListener(ShowDeletePopup);
    }

    public void SetSlot(CharacterData data, GameObject deletePopUp)
    {
        deletePopUpObj = deletePopUp;
        characterData = data;
        userName.text = data.nameData.userName;
        farmName.text = data.nameData.farmName + " 농장";
        int hour = (int)(data.timeData.time / 3600f);
        int minute = (int)(Mathf.Floor((data.timeData.time % 3600f / 60f) / 10f) * 10f);
        daysAndTime.text = data.timeData.days.ToString() + "일째     " + hour.ToString("00") + ":" + minute.ToString("00");
        gold.text = data.playerData.gold.ToString() + " Gold";
    }

    private void ClickData()
    {
        GameManager.Instance.Player.transform.position = new Vector2(0, 0);
        DataManager.Instance.CurDataSetting(userName.text);
        SceneLoadManager.Instance.LoadScene(SceneName.StartScene.ToString(), DataManager.Instance.curData.sceneData);
        //GameManager.Instance.Player.ApplyData();
        // DataManager.Instance.curData.npcListData에 여기 데이터를 업데이트
        // 그리고 NPC들에게 적용시켜 줘야 함
        GameManager.Instance.Player.interaction.otherCase = false;
    }
    
    private void ShowDeletePopup()
    {
        deletePopUpObj.SetActive(true);
        DeletePopUpUI popUp = deletePopUpObj.GetComponent<DeletePopUpUI>();
        popUp.SetPopUp(this, characterData);
    }

    public void DeleteLoadDataSlot()
    {
        DataManager.Instance.userDataList.user.Remove(characterData);
        DataManager.Instance.userDataDic.Remove(characterData.nameData.userName);
        Destroy(gameObject);    // TODO: 오브젝트 풀로 관리
        ResourcesManager.Instance.SaveData();
    }
}
