using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : TopDownController
{
    // Input System을 사용하여 입력을 처리하고, 입력된 방향을 TopDownController로 전달-> 지시만 하는 것.

    [SerializeField] private VirtualJoystick joystick;

    private Camera camera;

    private void Awake()
    {
        camera = Camera.main; // 메인 카메라 가져오기
    }

    private void Update()
    {

        if (joystick != null && joystick.gameObject.activeSelf)
        {
            Vector2 joystickInput = new Vector2(joystick.Horizontal(), joystick.Vertical());
            CallMoveEvent(joystickInput.normalized); // 빌드 후 확인해야함
        }
    }

    public void OnMove(InputValue value) // WASD로 들어온 값이 value로 들어옴
    {
        Vector2 moveInput = value.Get<Vector2>().normalized; // value에서 벡터2 가져와서 크기가 1인 벡터(정규화)로 만듦
        CallMoveEvent(moveInput); // 전달
        // 실제 움직이는 처리는 PlayerMovement에서 함
    }

    public void SetJoystick(VirtualJoystick newJoystick)
    {
        joystick = newJoystick;
    }

}
