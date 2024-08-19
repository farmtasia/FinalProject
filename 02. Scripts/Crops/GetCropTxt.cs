using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetCropTxt : MonoBehaviour
{
    public float moveSpeed = 0.001f; // 텍스트 이동 속도 (픽셀/초)
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
        completeText.color = transparentColor; // 초기 투명도 설정
    }

    public void ShowText(string message)
    {
        completeText.text = message;
        completeText.color = originalColor; // 텍스트 색상 초기화
        rectTransform.anchoredPosition = Vector2.zero; // 텍스트 위치 초기화

        // 기존 코루틴이 실행 중이면 중단
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        // 새로운 코루틴 시작
        gameObject.SetActive(true); // 활성화
        hideCoroutine = StartCoroutine(MoveAndFadeText());
    }

    private IEnumerator MoveAndFadeText()
    {
        float elapsedTime = 0f;

        while (elapsedTime < displayTime)
        {
            //rectTransform.anchoredPosition += new Vector2(0, moveSpeed * Time.deltaTime); // 위로 이동
            transform.position += new Vector3(0, moveSpeed * Time.deltaTime * Time.deltaTime); // 위로 이동
            completeText.color = Color.Lerp(originalColor, transparentColor, elapsedTime / displayTime); // 투명도 변환
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 이동 및 투명도 변환 완료 후 텍스트 비활성화
        completeText.color = transparentColor;
        //gameObject.SetActive(false);
        Destroy(transform.parent.gameObject);
    }
}
