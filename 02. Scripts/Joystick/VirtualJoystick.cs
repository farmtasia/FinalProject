using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : SceneUI, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private Image bg;
    [SerializeField] private Image controller;
    private Vector2 touchPosition;

    public void OnPointerDown(PointerEventData eventData) // IPointerDownHandler -> 터치 1회 실행
    {
        Debug.Log("터치 시작 : " + eventData);
    }

    public void OnDrag(PointerEventData eventData) // IDragHandler -> 터치 상태일 때 매 프레임마다 실행
    {
        touchPosition = Vector2.zero;

        // 조이스틱의 위치가 어디에 있든 동일한 값을 연산하기 위해 터치포지션의 위치값은 이미지의 현재 위치를 기준으로 얼마나 떨어져 있는지에 따라 다르게 나옴
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bg.rectTransform, eventData.position, eventData.pressEventCamera, out touchPosition))
        {
            // 터치포지션 값의 정규화 0 ~ 1 / 터치포지션을 이미지 크기로 나눔
            touchPosition.x = (touchPosition.x / bg.rectTransform.sizeDelta.x);
            touchPosition.y = (touchPosition.y / bg.rectTransform.sizeDelta.y);

            // 터치포지션값의 정규화 -n ~ n
            // 왼쪽(-1), 중심(0), 오른쪽(1)로 변경하기 위해 touchPosition.x * 2 - 1
            // 아래(-1), 중심(0), 위(1)로 변경하기 위해 touchPosition.y * 2 - 1
            // 이 수식은 Pivot에 따라 달라짐(좌하단 Pivot 기준)
            touchPosition = new Vector2(touchPosition.x * 2 - 1, touchPosition.y * 2 - 1);

            // 터치포지션 값의 정규화 -1 ~ 1
            // 가상 조이스틱 배경 밖으로 터치가 나가게 되면 -1 ~ 1 보다 큰 값이 나올 수 있음
            // 이때 normailzed를 이용해 -1 ~ 1 사이의 값으로 정규화
            touchPosition = (touchPosition.magnitude > 1) ? touchPosition.normalized : touchPosition;

            // 가상 조이스틱 컨트롤러 이미지 이동
            controller.rectTransform.anchoredPosition = new Vector2(
                touchPosition.x * bg.rectTransform.sizeDelta.x / 2,
                touchPosition.y * bg.rectTransform.sizeDelta.y / 2);
        }
    }

    public void OnPointerUp(PointerEventData eventData) // IPointerUpHandler -> 터치 종료 시(터치했다가 떼었을 때) 1회 실행
    {
        controller.rectTransform.anchoredPosition = Vector2.zero; // 터치 종료 시 이미지 위치를 중앙으로 다시 옮김
        touchPosition = Vector2.zero; // 다른 오브젝트에서 이동 방향으로 사용하기 때문에 이동 방향도 초기화
    }

    public float Horizontal()
    {
        return touchPosition.x;
    }

    public float Vertical()
    {
        return touchPosition.y;
    }
}
