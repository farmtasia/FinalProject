using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetItemInfoBox : MonoBehaviour
{
    public float moveSpeed; // 50f, 텍스트 이동 속도 (픽셀/초)-> 1초에 50픽셀만큼 이동
    public float alphaSpeed; // 2f, 투명도 변환 속도
    public float displayTime; // 2f, 표시 시간
    public Image itemBG;
    public Image itemImg;
    public TextMeshProUGUI itemCount;

    private Color originalColor;
    private Color transparentColor;
    private RectTransform rectTransform;
    private Coroutine hideCoroutine; // 코루틴 참조

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalColor = Color.white;
        transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }

    private void OnEnable()
    {
        // 텍스트 초기화(색깔, 위치)      
        rectTransform.anchoredPosition = new Vector2(8f, 123f);

        // 기존 코루틴이 실행 중이면 중단
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        // 새로운 코루틴 시작
        hideCoroutine = StartCoroutine(MoveAndFadeImage());
    }

    private IEnumerator MoveAndFadeImage()
    {
        float elapsedTime = 0f;

        while (elapsedTime < displayTime)
        {
            rectTransform.anchoredPosition += new Vector2(0, moveSpeed * Time.deltaTime); // 위로 이동
            itemBG.color = Color.Lerp(originalColor, transparentColor, elapsedTime / displayTime); // 투명도 변환
            itemImg.color = Color.Lerp(originalColor, transparentColor, elapsedTime / displayTime); // 투명도 변환
            itemCount.color = Color.Lerp(Color.black, transparentColor, elapsedTime / displayTime); // 투명도 변환
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 이동 및 투명도 변환 완료 후 텍스트 비활성화
        itemBG.color = transparentColor;
        itemImg.color = transparentColor;
        itemCount.color = transparentColor;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void GetItemInfoSet(Sprite itemImage, int itemQuantity)
    {
        itemImg.sprite = itemImage; // 해당 아이템의 이미지
        itemCount.text = "+ " + itemQuantity.ToString(); // 아이템 개수 설정
    }
}
