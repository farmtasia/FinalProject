using UnityEngine;

public class HarvestArea : MonoBehaviour
{
    public float scaleAmount = 0.05f; // 크기 변화 정도
    public float speed = 5f; // 크기 변화 속도
    private Vector3 originalScale;

    public CropHarvest cropHarvest;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float scale = 1 + Mathf.Sin(Time.unscaledTime * speed) * scaleAmount;
        transform.localScale = originalScale * scale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (cropHarvest != null)
            {
                cropHarvest.OnPlayerEnterHarvestArea();
            }
        }
    }
}
