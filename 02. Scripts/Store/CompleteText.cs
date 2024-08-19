using System.Collections;
using TMPro;
using UnityEngine;

public class CompleteText : MonoBehaviour
{
    public float moveSpeed = 50f; // 텍스트 이동 속도 (픽셀/초)-> 1초에 50픽셀만큼 이동
    public float alphaSpeed = 2f; // 투명도 변환 속도
    public float displayTime = 2f; // 표시 시간
    private TextMeshProUGUI completeText;
    private Color originalColor;
    private Color transparentColor;
    private RectTransform rectTransform;
    private Coroutine hideCoroutine; // 코루틴 참조

    private void Awake()
    {
        completeText = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
        originalColor = completeText.color;
        transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }

    private void OnEnable()
    {
        // 텍스트 초기화(색깔, 위치)
        completeText.color = originalColor;
        rectTransform.anchoredPosition = Vector2.zero;

        // 기존 코루틴이 실행 중이면 중단
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        // 새로운 코루틴 시작
        hideCoroutine = StartCoroutine(MoveAndFadeText());
    }

    /*
    private void Update() // 코루틴으로 바뀌는게 좋다.이동하고 바뀐 후 사라지게. 시작할 때 기본위치, 기본색깔. 시작해서 이동,지워짐/ 
    {
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition += new Vector2(0, moveSpeed * Time.deltaTime); // 위로 이동
        }

        completeText.color = Color.Lerp(completeText.color, transparentColor, Time.deltaTime * alphaSpeed);
    }

    private IEnumerator HideTextAfterDelay()
    {
        yield return new WaitForSeconds(displayTime); // displayTime만큼 대기
        gameObject.SetActive(false); // 텍스트 비활성화
    }
    */

    private IEnumerator MoveAndFadeText()
    {
        float elapsedTime = 0f;

        while (elapsedTime < displayTime)
        {
            rectTransform.anchoredPosition += new Vector2(0, moveSpeed * Time.deltaTime); // 위로 이동
            completeText.color = Color.Lerp(originalColor, transparentColor, elapsedTime / displayTime); // 투명도 변환
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 이동 및 투명도 변환 완료 후 텍스트 비활성화
        completeText.color = transparentColor;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }


    public void SetText(string message)
    {
        completeText.text = message; // 텍스트 내용 설정
    }
}
