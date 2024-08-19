using System.Collections;
using TMPro;
using UnityEngine;

public class Sleep : MonoBehaviour, IInteractable
{
    [SerializeField] private Color blackOut = Color.black;
    [SerializeField] private AnimationCurve lightCurve;    // 서서히 암전

    [SerializeField] private GameObject mainUI;    // Main_UI 인스펙터 창에서 할당
    [SerializeField] private TextMeshProUGUI nextDay;

    private void Initialize()
    {
        mainUI = GameObject.Find("Canvas_Main(Clone)/SafeArea/Main_UI");
        GameObject nextDayObj = GameObject.Find("Canvas_Main(Clone)/SafeArea/NextDay");
        if (nextDayObj != null)
        {
            nextDay = nextDayObj.GetComponent<TextMeshProUGUI>();
        }
    }

    public string GetInteractPrompt()
    {
        string str = "잠들기";
        return str;
    }

    public void OnInteract()
    {
        Initialize();
        StartCoroutine(SleepMode());
    }

    public void ActiveBtn()
    {
        gameObject.SetActive(true);
    }

    private IEnumerator SleepMode()
    {
        Time.timeScale = 0f;    // 시간 멈춤
        float duration = 1.5f;    // 암전 시간 1.5초
        float elapsedTime = 0f;    // 경과 시간
        Color BGColor = TimeManager.Instance.GlobalLight.color;    // 빛
        mainUI.SetActive(false);    // UI 비활성화

        while (elapsedTime < duration)    // 1.5초 동안
        {
            elapsedTime += Time.unscaledDeltaTime;    // 현실 시간 흐름
            float curve = lightCurve.Evaluate(elapsedTime / duration);    // 커브 값 계산
            Color lightColor = Color.Lerp(BGColor, blackOut, curve);    // 점점 까맣게 변함
            TimeManager.Instance.GlobalLight.color = lightColor;    // 실제 빛에 적용
            yield return null; // 프레임마다 업데이트
        }

        TimeManager.Instance.GlobalLight.color = blackOut;    // 암전 상태 유지
        nextDay.gameObject.SetActive(true);    // 다음 날 텍스트 표시
        yield return new WaitForSecondsRealtime(duration);    // 1.5초 대기

        nextDay.gameObject.SetActive(false);    // 다음 날 텍스트 미표시
        TimeManager.Instance.Tomorrow();    // 다음 날
        GameManager.Instance.Player.transform.position = new Vector2(-16.9f, 14.3f);
        mainUI.SetActive(true);    // UI 활성화
        Time.timeScale = 1f;    // 시간 재개
    }
}