using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class TimeManager : Singleton<TimeManager>
{
    private readonly float secondsOfDay = 86400f;    // 초 단위 하루 시간
    private readonly float timeScale = 60f;    // 실제 1초 = 게임 세계 1분(실제 작동: 60, 테스트용: 3600)

    [SerializeField] private Color dayLightColor;    // 낮 빛 색상
    [SerializeField] private Color nightLightColor;    // 밤 빛 색상
    [SerializeField] private AnimationCurve lightCurve;    // 낮 -> 밤 빛 세기 조절해서 자연스럽게 변화
    [SerializeField] private Light2D globalLight;    // Light2D 인스펙터 창에서 할당

    public Light2D GlobalLight
    {
        get => globalLight;
        private set => globalLight = value;
    }
    
    public float time => DataManager.Instance.curData.timeData.time;    // 현재 시간(시작 시간: 오전 6시)
    public int days => DataManager.Instance.curData.timeData.days;    // 날짜
    public float Hours { get { return time / 3600f; } }    // 시 계산
    public float Minutes { get { return Mathf.Floor((time % 3600f / 60f) / 10f) * 10f; } }    // 분 계산(10분 단위로 표현)

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == SceneName.StartScene.ToString())
        {
            Time.timeScale = 0;
        }
        globalLight = FindObjectOfType<Light2D>();
    }

    private void Update()
    {
        if (Time.timeScale < 1) return;    //  시간 멈췄을 때 작동하지 않도록 예외 처리

        DataManager.Instance.curData.timeData.time += Time.deltaTime * timeScale;    // 시간 흐름
        float curve = lightCurve.Evaluate(Hours);    // 시간의 커브 값
        Color lightColor = Color.Lerp(dayLightColor, nightLightColor, curve);    // 낮 밤 사이의 비율에 위치한 색
        globalLight.color = lightColor;    // 색 적용

        if (time >= 0f && time < 1f)    // 자정 되면 자동 저장
            GameManager.Instance.SaveData();

        if (time > secondsOfDay)    // 하루 지났는지 확인
            StartCoroutine(NextDay());
    }

    IEnumerator NextDay()
    {
        DataManager.Instance.curData.timeData.days++;    // 다음 날
        DataManager.Instance.curData.timeData.time = 0;    // 시간 초기화
        yield break;
    }

    public void Tomorrow()
    {
        DataManager.Instance.curData.timeData.days++;    // 다음 날
        DataManager.Instance.curData.timeData.time = 32400f;    // 오전 6시
        // TODO: 위치 확인하고 특정 오브젝트 위치로 이동
        GameManager.Instance.Player.transform.position = new Vector3(0, 0, 0);    // 기본 시작 위치
    }
}