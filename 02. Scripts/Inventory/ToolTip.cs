using TMPro;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDetailTypeText;
    public TextMeshProUGUI itemDescriptionText;

    public RectTransform toolTipCanvas;
    public RectTransform toolTipRect;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ToolTipToggle(ItemSO data)
    {
        if (data != null)
        {
            itemNameText.text = data.itemName;
            itemDetailTypeText.text = ChangeKorean(data);
            itemDescriptionText.text = data.itemDescription;
            gameObject.SetActive(true);

            ShowToolTipPosition();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void ShowToolTipPosition()
    {
        Vector2 localPos;
        Vector2 touchPos = Input.mousePosition;

        // 너비와 높이의 절반을 기준으로 pivot 설정
        float pivotX = touchPos.x / Screen.width < 0.5f ? 0 : 1;
        float pivotY = touchPos.y / Screen.height < 0.5f ? 0 : 1;
        toolTipRect.pivot = new Vector2(pivotX, pivotY);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(toolTipCanvas, touchPos, null, out localPos);

        toolTipRect.anchoredPosition = localPos;
    }

    // EItemDetailType 을 한글로 변환해주는 내용이 필요하여 생성
    public string ChangeKorean(ItemSO item)
    {
        switch (item.detailType)
        {
            case EItemDetailType.TOOLS:
                return "도구";
            case EItemDetailType.SEED:
                return "씨앗";
            case EItemDetailType.FRUITS:
                return "과일";
            case EItemDetailType.FISHES:
                return "해산물";
            case EItemDetailType.TREES:
                return "묘목, 나무";
            case EItemDetailType.FLOWERS:
                return "꽃";
            case EItemDetailType.VEGETABLES:
                return "야채";
            case EItemDetailType.RESOURCE:
                return "자원";
            case EItemDetailType.FOODS:
                return "음식";
            case EItemDetailType.CROPS:
                return "작물";
            default:
                return "";
        }
    }
}
