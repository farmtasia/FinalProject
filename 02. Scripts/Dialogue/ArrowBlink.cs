using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ArrowBlink : MonoBehaviour
{
    private float fadeTime; // 페이드 되는 시간
    private float blinkInterval = 0.3f; // 깜빡임 간격
    public Image fadeImage;

    //private void Start()
    //{
    //    fadeImage = GetComponent<Image>();
    //}

    private void OnEnable()
    {
        // Fade 효과 In -> Out 무한 반복
        StartCoroutine("FadeInOut");
    }

    private void OnDisable()
    {
        StopCoroutine("FadeInOut");
    }

    private IEnumerator FadeInOut()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(1, 0)); // 페이드 인
            yield return new WaitForSeconds(blinkInterval);
            yield return StartCoroutine(Fade(0, 1)); // 페이드 아웃
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private IEnumerator Fade(float start, float end)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / fadeTime;

            Color color = fadeImage.color;
            color.a = Mathf.Lerp(start, end, percent);
            fadeImage.color = color;

            yield return null;
        }
    }
}
