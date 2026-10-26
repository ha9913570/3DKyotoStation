using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    private const float CAMERA_MOVE_SPEED = 0.12f;
    private const float KEYBOARD_MOVE_SPEED = 90f;
    private const float CAMERA_ROTATE_SPEED = 0.1f;
    private const float CAMERA_ZOOM_SPEED = 7f;
    private const float CAMERA_MAX_POS_X = 500f;
    private const float CAMERA_MAX_POS_Y = 300f;
    private const float CAMERA_MAX_POS_Z = 500f;

    private readonly Vector3 CAMERA_INITAL_POSITION = new(17f, 23f, 132f);
    private readonly Vector3 CAMERA_INITAL_ROTATE = new(0f, 180f, 0f);

    private float mouseMoveRatio = 1f;
    private float mouseZoomRatio = 1f;
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
        CalcMouseMoveRatio();
        Vector3 moveAmount = CAMERA_MOVE_SPEED * mouseMoveRatio * mouseMoveAction.ReadValue<Vector2>();
        if (IsCameraPosInLimit(moveAmount + gameObject.transform.position))
        {
            transform.Translate(moveAmount, Space.Self);
        }
    }

    // WASDによってカメラを移動させる関数
    private void CameraMoveByKeyboard()
    {
        Vector3 moveAmount = KEYBOARD_MOVE_SPEED * Time.deltaTime * keyboardMoveAction.ReadValue<Vector3>();
        if (IsCameraPosInLimit(moveAmount + gameObject.transform.position))
        {
            transform.Translate(moveAmount, Space.Self);
        }
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
        CalcMouseZoomRatio();
        float scrollAmount = mouseScrollAction.ReadValue<float>();
        Vector3 moveAmount = CAMERA_ZOOM_SPEED * scrollAmount * mouseZoomRatio * transform.forward;
        if (IsCameraPosInLimit(moveAmount + gameObject.transform.position))
        {
            transform.Translate(moveAmount, Space.World);
        }
    }

    // 移動先の場所がカメラの移動範囲内にいるかどうかを返す関数
    private bool IsCameraPosInLimit(Vector3 dest)
    {
        float x = Math.Abs(dest.x);
        float y = Math.Abs(dest.y);
        float z = Math.Abs(dest.z);

        if (x > CAMERA_MAX_POS_X || y > CAMERA_MAX_POS_Y || z > CAMERA_MAX_POS_Z)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // カメラの位置に応じてマウスによる移動倍率を設定する関数
    private void CalcMouseMoveRatio()
    {
        Vector3 cameraPos = gameObject.transform.position;
        mouseMoveRatio = Math.Min(cameraPos.sqrMagnitude / 70000 + 1, 2f);
    }

    // カメラの位置に応じてズームの移動倍率を設定する関数
    private void CalcMouseZoomRatio()
    {
        Vector3 cameraPos = gameObject.transform.position;
        mouseZoomRatio = Math.Min(cameraPos.sqrMagnitude / 80000 + 0.5f, 3f);
    }
}
