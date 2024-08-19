using TMPro;
using UnityEngine;


public class StartNewUI : MonoBehaviour
{
    // '새로하기'에서 입력한 정보(유저이름, 농장이름)를 데이터 매니저로 전달
    [SerializeField] private TMP_InputField inputUserName;
    [SerializeField] private TMP_InputField inputFarmName;
    [SerializeField] private GameObject maxCharacterCountPopUp;
    [SerializeField] private GameObject nullNamePopUp;
    [SerializeField] private GameObject isSameName;

    private int maxNameCount = 12;

    private void Start()
    {
        // 익명 메서드로 델리게이트에 연결해서 입력 값에 변화가 있을 때마다 InputLimit 메서드 호출
        inputUserName.onValueChanged.AddListener(delegate { InputLimit(inputUserName); });
        inputFarmName.onValueChanged.AddListener(delegate { InputLimit(inputFarmName); });
    }

    private void InputLimit(TMP_InputField input)    // 입력 값 제한
    {
        int curNameCount = CalCulateByteCount(input.text);
        if (curNameCount > maxNameCount)
        {
            // 커서 위치 한 칸 앞으로 이동
            int caretPos = input.caretPosition -1;
            // 크기 12까지의 문자열만 추출
            input.text = input.text.Substring(0, input.text.Length - 1);
            // 커서 음수 되지 않게 보정
            input.caretPosition = Mathf.Max(0, caretPos);
        }
    }

    private int CalCulateByteCount(string input)
    {
        // 한글: 2byte, 영어, 공백, 특수문자: 1byte
        int byteCount = 0;
        foreach (char c in input)
        {
            if (IsKorean(c))
            {
                byteCount += 2;
            }
            else
            {
                byteCount ++;
            }
        }
        return byteCount;
    }

    private bool IsKorean(char c)
    {
        // 한글 유니코드 범위: AC00 ~ D7A3
        return c >= '\uAC00' && c <= '\uD7A3';
    }

    public void CheckCreate()
    {
        if (CheckCanNewCharacter())
        {
            if (CheckIsNameNull())
            {
                CheckIsSameName();
            }
        }
    }

    private bool CheckCanNewCharacter()    // 새로운 캐릭터 생성 가능 여부 체크
    {
        if (DataManager.Instance.userDataList.user.Count >= 3)    // 최대 생성 가능 개수 3개
        {
            maxCharacterCountPopUp.SetActive(true);    // 이미 계정이 3개면 생성 불가하다는 팝업
            return false;
        }
        return true;
    }

    private bool CheckIsNameNull()    // 공란 확인
    {
        if (string.IsNullOrEmpty(inputUserName.text) || string.IsNullOrEmpty(inputFarmName.text))
        {
            // 둘 중 하나라도 이름이 입력되어 있지 않으면 입력하라는 팝업
            nullNamePopUp.SetActive(true);
            return false;
        }
        return true;
    }

    private void CheckIsSameName()    // 중복 이름 확인
    {
        if (DataManager.Instance.userDataDic.ContainsKey(inputUserName.text))
        {
            isSameName.SetActive(true);
            return;
        }
        else
        {
            // 알맞게 작성했으면 데이터 매니저로 이름 데이터 전달
            DataManager.Instance.GetNameData(new NameData(inputUserName.text, inputFarmName.text));
            DataManager.Instance.StartSetting();
            GameManager.Instance.Player.transform.position = new Vector2(0, 0);
            SceneLoadManager.Instance.LoadScene(SceneName.StartScene.ToString(), SceneName.FarmScene.ToString());
            //GameManager.Instance.Player.ApplyData();
            GameManager.Instance.Player.interaction.otherCase = false;
        }
    }
}
