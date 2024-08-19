using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Canvas_Main : SceneUI
{
    [SerializeField] private TextMeshProUGUI date;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private GameObject _interactionUI;
    [SerializeField] private TextMeshProUGUI _promptText;
    [SerializeField] private GameObject _curEquipToolUI;
    [SerializeField] private Image _curEquipToolImage;
    [SerializeField] private TextMeshProUGUI _curEquipToolName;
    [SerializeField] private GameObject _blockBoard;
    [SerializeField] private VirtualJoystick joystick;
    [SerializeField] private GameObject guide;
    [SerializeField] private GameObject skipBtn;

    public static GameObject interactionUI;
    public static TextMeshProUGUI promptText;
    public static GameObject curEquipToolUI;
    public static Image curEquipToolImage;
    public static TextMeshProUGUI curEquipToolName;
    public static GameObject blockBoard; // 버튼클릭을 막을 때 사용

    private Button interactionBtn;

    private void Start()
    {
        MainUIFieldStartSet();
        CheckPlatform();
        interactionUI.SetActive(false); // 이게 없으면 씬 바뀌자마자 한번 바로 보임
        promptText.gameObject.SetActive(false);
        blockBoard.SetActive(false);

        interactionBtn = interactionUI.GetComponent<Button>();
        interactionBtn.onClick.AddListener(OnInteractionBtnClick);
        GameManager.Instance.Player.gold.OnGoldChanged -= UpdateGoldText;
        GameManager.Instance.Player.gold.OnGoldChanged += UpdateGoldText;

        SetJoystickForPlayer();

        OnCurEquipTool(); // 장착된 도구가 있다면 UI를 활성화
        //OpenGuide(); // 게임 첫 시작시 가이드화면 열림

        if (DataManager.Instance.curData.playerData.hasCompletedTutorial == false)
            skipBtn.SetActive(true);
    }

    private void Update()
    {
        UpdateInfo();

        #if UNITY_EDITOR
        // J 키로 조이스틱 활성화/비활성화 / 테스트용
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (joystick != null)
            {
                joystick.gameObject.SetActive(!joystick.gameObject.activeSelf);
            }
        }
        #endif
    }

    private void MainUIFieldStartSet()
    {
        interactionUI = _interactionUI;
        promptText = _promptText;
        curEquipToolUI = _curEquipToolUI;
        curEquipToolImage = _curEquipToolImage;
        curEquipToolName = _curEquipToolName;
        blockBoard = _blockBoard;
    }

    private void UpdateInfo()
    {
        int hour = (int)TimeManager.Instance.Hours;    // 현재 시간
        int minute = (int)TimeManager.Instance.Minutes;    // 현재 분
        if (time == null || date == null)
            return;
        time.text = hour.ToString("00") + ":" + minute.ToString("00");
        date.text = TimeManager.Instance.days.ToString() + "일째";
        UpdateGoldText(DataManager.Instance.curData.playerData.gold);
    }

    public void OnClickSettingBtn()
    {
        UIManager.Instance.ShowUI<SettingsPopup>(UIs.Popup);
        blockBoard.SetActive(true);
        GameManager.Instance.Player.topDownMovement.HoldOnMoveSpeed(0f);
    }

    public void OnClickInventoryBtn()
    {      
        UIManager.Instance.ShowUI<Inventory>(UIs.Popup);
        GameManager.Instance.Player.topDownMovement.HoldOnMoveSpeed(0f);
    }

    public void OnInteractionBtnClick()
    {
        GameManager.Instance.Player.interaction.OnItemInteract();
    }

    public void OnClickGuideBtn()
    {
        GameManager.Instance.Player.topDownMovement.HoldOnMoveSpeed(0f);
        blockBoard.SetActive(true);
        guide.SetActive(true);
    }

    public static void OnCloseBtn()
    {
        // close 버튼이 연결되어 있는 다른 스크립트에서 코드로 연결
        if (SceneManager.GetActiveScene().name != SceneName.StartScene.ToString())
        {
            GameManager.Instance.Player.topDownMovement.ResetMoveSpeed();
            if (blockBoard.activeSelf) blockBoard.SetActive(false);
        }
    }

    private void UpdateGoldText(int gold)
    {
        this.gold.text = gold.ToString() + " G";
    }

    private void CheckPlatform()
    {
        #if UNITY_EDITOR
        // 에디터에서는 기본적으로 조이스틱 비활성화
        if (joystick != null)
        {
            joystick.gameObject.SetActive(false);
        }
        return;
        #endif

        if (Application.isMobilePlatform)
        {
            if (joystick != null)
            {
                joystick.gameObject.SetActive(true);
                SetJoystickForPlayer(); // 조이스틱 활성화 시 플레이어 컨트롤러에 조이스틱 설정
            }
        }
        else
        {
            if (joystick != null)
            {
                joystick.gameObject.SetActive(false);
                ClearJoystickForPlayer(); // 조이스틱 비활성화 시 플레이어 컨트롤러에서 조이스틱 제거
            }
        }
    }

    private void SetJoystickForPlayer()
    {
        var playerInputController = GameManager.Instance.Player.GetComponent<PlayerInputController>();
        if (playerInputController != null && joystick != null)
        {
            playerInputController.SetJoystick(joystick); // SetJoystick 메서드를 통해 조이스틱 설정
        }
    }

    private void ClearJoystickForPlayer()
    {
        var playerInputController = GameManager.Instance.Player.GetComponent<PlayerInputController>();
        if (playerInputController != null)
        {
            playerInputController.SetJoystick(null); // 조이스틱을 null로 설정하여 제거
        }
    }

    private void OnCurEquipTool()
    {
        if (GameManager.Instance.Player.equipment.curEquipTool != null)
        {
            SetcurEquipToolUI(GameManager.Instance.Player.equipment.curEquipTool.itemData, true);
        }
    }

    public static void SetcurEquipToolUI(ItemSO itemData, bool setActiveValue)
    {
        if (setActiveValue)
        {
            curEquipToolUI.SetActive(setActiveValue);
            curEquipToolImage.sprite = itemData.itemIcon;
            curEquipToolName.text = itemData.itemName;
        }
        else
        {
            curEquipToolUI.SetActive(setActiveValue);
            curEquipToolImage.sprite = null;
            curEquipToolName.text = null;
        }
    }

    //private void OpenGuide()
    //{
    //    if (SceneManager.GetActiveScene().name == SceneName.FarmScene.ToString() && !GameManager.Instance.Player.firstOpenGuide)
    //    {
    //        OnClickGuideBtn();
    //        GameManager.Instance.Player.firstOpenGuide = true;
    //    }
    //}
}
