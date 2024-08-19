using System;
using UnityEngine;

public class TopDownController : MonoBehaviour
{
    // 플레이어 이동 이벤트 정의하고 호출하는 역할

    public event Action<Vector2> OnMoveEvent;

    public void CallMoveEvent(Vector2 direction) // 이동 명령 전달
    {
        OnMoveEvent?.Invoke(direction);
    }

}
