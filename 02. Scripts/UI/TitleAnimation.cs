using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    public float scaleAmount = 0.1f; // 크기 변화 정도
    public float speed = 2f; // 크기 변화 속도
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale; // 초기 크기 저장
    }

    void Update()
    {
        // Sin 함수를 이용해 크기 변화 적용
        float scale = 1 + Mathf.Sin(Time.unscaledTime * speed) * scaleAmount;
        transform.localScale = originalScale * scale;
    }
}
