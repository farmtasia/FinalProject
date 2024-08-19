using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    // 앵커의 범위는 0부터 1까지입니다.
    // 앵커는 x와 y로 이뤄져있습니다.
    // 스트레치로 확장시켜놓은 상태에서 left나 top 같은 수치를 0으로 고정시킨 채로,
    // Anchor의 값을 바꾸면 그 수치대로 UI의 크기가 바뀌게됩니다.


    // 게임 화면 크기보다 safeArea크기가 무조건 더 작으므로,
    // Anchor의 값을 변경해서 safeArea크기에 맞추는 원리.
    RectTransform rectTransform;
    Rect safeArea;
    Vector2 minAnchor;
    Vector2 maxAnchor;

    void Awake()
    {
        // UI는 Transform이 아니라 RectTransform으로 이뤄져있기 때문에 RectTransform을 가져옴.
        rectTransform = GetComponent<RectTransform>();

        // 스크린(게임화면)에서 safeArea 영역의 크기를 가져옵니다.
        // 핸드폰 기종마다 safeArea 크기가 다르지만, Screen.safeArea가 기종별로 알맞은 수치를 가져다 줌.
        safeArea = Screen.safeArea;

        minAnchor = safeArea.position;
        maxAnchor = minAnchor + safeArea.size;

        // Screen의 넓이와 높이로 x와 y를 나누는 이유.
        // 0 ~ 1 사이의 크기로 변경하기 위해서.
        // 수학적인 계산 -> 전체 길이에서 현재 위치를 나누면 0.x가 나옵니다. 0일경우는 시작점. 1일경우는 끝점.
        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        // min을 anchorMin에 넣고, max를 anchorMax에 넣으면 ui가 적절하게 세팅됨.
        rectTransform.anchorMin = minAnchor;
        rectTransform.anchorMax = maxAnchor;
    }
}
