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

    private Vector3 previousMousePos;
    private bool isLeftDragging = false;
    private bool isRightDragging = false;

    private float yawAngle;
    private float pitchAngle;

    private InputAction cameraResetAction;

    void Awake()
    {
        cameraResetAction = InputSystem.actions.FindAction("CameraReset");
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

        UpdateMouseDragStatus();
        CameraMove();
        CameraRotate();
        CameraZoom();

        previousMousePos = Mouse.current.position.ReadValue();
    }

    // 右クリックをドラッグしたときにカメラを移動させる関数
    private void CameraMove()
    {
        // キーボードによる移動
        Vector3 keyboardMove = Vector3.zero;
        if (Keyboard.current.wKey.isPressed)
        {
            keyboardMove.z += 1f;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            keyboardMove.z -= 1f;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            keyboardMove.x -= 1f;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            keyboardMove.x += 1f;
        }

        transform.Translate(KEYBOARD_MOVE_SPEED * Time.deltaTime * keyboardMove.normalized, Space.Self);

        // マウスによる移動
        if (Mouse.current != null && isRightDragging)
        {
            Vector3 currentMousePos = Mouse.current.position.ReadValue();
            Vector3 mousePosDiff = currentMousePos - previousMousePos;

            transform.Translate(-mousePosDiff * CAMERA_MOVE_SPEED, Space.Self);
        }
    }

    // 左クリックをドラッグしたときにカメラを回転させる関数
    private void CameraRotate()
    {
        if (isLeftDragging)
        {
            Vector3 currentMousePos = Mouse.current.position.ReadValue();
            Vector3 mousePosDiff = currentMousePos - previousMousePos;

            yawAngle += mousePosDiff.x * CAMERA_ROTATE_SPEED * -1;
            pitchAngle += mousePosDiff.y * CAMERA_ROTATE_SPEED;
            if (yawAngle % 360f > 180f)
            {
                yawAngle -= 360f;
            }
            transform.eulerAngles = new Vector3(pitchAngle, yawAngle, 0f);
        }
    }

    // マウスホイールでカメラをズームさせる関数
    private void CameraZoom()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;
        transform.Translate(CAMERA_ZOOM_SPEED * scroll * transform.forward, Space.World);
    }

    // マウスのドラッグクリックの状態を変更する関数
    private void UpdateMouseDragStatus()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            isRightDragging = true;

        }
        else if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            isRightDragging = false;
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isLeftDragging = true;

        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isLeftDragging = false;
        }
    }
}
