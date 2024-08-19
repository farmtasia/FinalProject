using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpEffect : MonoBehaviour
{
    public float moveDistance = 50f; // 이미지 이동 거리
    public float moveDuration = 1f; // 이동 시간
    public float alphaSpeed = 3f; // 투명도 변환 속도
    public float displayTime = 3f; // 표시 시간
    public TextMeshProUGUI levelUpText;

    private Image levelUpImage;
    private Color originalColor;
    private Color transparentColor;
    private RectTransform rectTransform;
    private Coroutine hideCoroutine;

    private void Awake()
    {
        levelUpImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        originalColor = levelUpImage.color;
        transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }

    private void OnEnable()
    {
        levelUpImage.color = originalColor;
        //rectTransform.anchoredPosition = Vector2.zero;

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(MoveAndFadeImage());
        StartCoroutine(AnimateRainbowText()); // 텍스트 애니메이션 시작
    }

    private IEnumerator MoveAndFadeImage()
    {
        float elapsedTime = 0f;
        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 endPosition = startPosition + new Vector2(0, moveDistance);

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);
            levelUpImage.color = Color.Lerp(originalColor, transparentColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 이동 완료 후 최종 위치에 설정
        rectTransform.anchoredPosition = endPosition;
        //levelUpImage.color = Color.Lerp(originalColor, transparentColor, elapsedTime / displayTime);
        levelUpImage.color = transparentColor; // 이동 완료 시 최종 색상 설정

        // 나머지 표시 시간 동안 유지
        yield return new WaitForSeconds(displayTime - moveDuration);

        // 최종 상태로 설정하고 객체 비활성화
        levelUpImage.color = transparentColor;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private IEnumerator AnimateRainbowText() // 텍스트 색깔 변환 코루틴
    {
        string text = "LEVEL UP";
        int length = text.Length;
        Color[] rainbowColors = {
            Color.red,
            new Color(1f, 0.5f, 0f), // 오렌지
            Color.yellow,
            Color.green,
            Color.blue,
            new Color(0.29f, 0f, 0.51f), // 인디고
            Color.magenta
        };

        while (true)
        {
            for (int i = 0; i < length; i++)
            {
                string coloredText = "";
                for (int j = 0; j < length; j++)
                {
                    int colorIndex = (i + j) % rainbowColors.Length;
                    string colorHex = ColorUtility.ToHtmlStringRGB(rainbowColors[colorIndex]);
                    coloredText += $"<color=#{colorHex}>{text[j]}</color>";
                }
                levelUpText.text = coloredText;
                yield return new WaitForSeconds(0.2f); // 색상 변경 주기
            }
        }
    }
}
