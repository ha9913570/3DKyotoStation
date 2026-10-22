using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    private const float CAMERA_MOVE_SPEED = 0.12f;
    private const float KEYBOARD_MOVE_SPEED = 90f;
    private const float CAMERA_ROTATE_SPEED = 0.1f;
    private const float CAMERA_ZOOM_SPEED = 7f;

    private readonly Vector3 CAMERA_INITAL_POSITION = new(17f, 23f, 132f);
    private readonly Vector3 CAMERA_INITAL_ROTATE = new(0f, 180f, 0f);

    private float yawAngle;
    private float pitchAngle;

    private InputAction mouseMoveAction;
    private InputAction mouseScrollAction;
    private InputAction mouseRightClickAction;
    private InputAction keyboardMoveAction;
    private InputAction cameraResetAction;
    private InputAction showMouseCursorAction;

    void Awake()
    {
        mouseMoveAction = InputSystem.actions.FindAction("MouseMove");
        mouseScrollAction = InputSystem.actions.FindAction("MouseScroll");
        mouseRightClickAction = InputSystem.actions.FindAction("MouseRightClick");
        keyboardMoveAction = InputSystem.actions.FindAction("KeyboardMove");
        cameraResetAction = InputSystem.actions.FindAction("CameraReset");
        showMouseCursorAction = InputSystem.actions.FindAction("ShowMouseCursor");
    }

    void Start()
    {
        Vector3 angle = transform.eulerAngles;
        yawAngle = angle.y;
        pitchAngle = angle.x;
    }

    void Update()
    {
        if (cameraResetAction.WasPressedThisFrame())
        {
            gameObject.transform.position = CAMERA_INITAL_POSITION;

            gameObject.transform.eulerAngles = CAMERA_INITAL_ROTATE;
            yawAngle = CAMERA_INITAL_ROTATE.y;
            pitchAngle = CAMERA_INITAL_ROTATE.x;
        }

        if (showMouseCursorAction.IsPressed())
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            CameraMoveByKeyboard();
            // 右クリックが押されている間はカメラ移動
            if (mouseRightClickAction.IsPressed())
            {
                CameraMoveByMouse();
            }
            // 右クリックが押されていない間はカメラ回転
            else
            {
                CameraRotate();
            }
            CameraZoom();
        }
    }

    // マウスを動かしてカメラを移動させる関数
    private void CameraMoveByMouse()
    {
        Vector2 mouseMoveAmount = mouseMoveAction.ReadValue<Vector2>();
        transform.Translate(mouseMoveAmount * CAMERA_MOVE_SPEED, Space.Self);
    }

    // WASDによってカメラを移動させる関数
    private void CameraMoveByKeyboard()
    {
        Vector3 keyboardMoveAmount = keyboardMoveAction.ReadValue<Vector3>();
        transform.Translate(KEYBOARD_MOVE_SPEED * Time.deltaTime * keyboardMoveAmount, Space.Self);
    }

    // マウスを動かしてカメラを回転させる関数
    private void CameraRotate()
    {
        Vector2 mouseMoveAmount = mouseMoveAction.ReadValue<Vector2>();

        yawAngle += mouseMoveAmount.x * CAMERA_ROTATE_SPEED;
        pitchAngle += mouseMoveAmount.y * CAMERA_ROTATE_SPEED * -1;
        if (yawAngle % 360f > 180f)
        {
            yawAngle -= 360f;
        }
        transform.eulerAngles = new Vector3(pitchAngle, yawAngle, 0f);
    }

    // マウスホイールでカメラをズームさせる関数
    private void CameraZoom()
    {
        float scrollAmount = mouseScrollAction.ReadValue<float>();
        transform.Translate(CAMERA_ZOOM_SPEED * scrollAmount * transform.forward, Space.World);
    }
}
