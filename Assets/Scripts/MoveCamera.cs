using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private float cameraMoveSpeed = 0.08f;
    [SerializeField] private float keyboardMoveSpeed = 50f;
    [SerializeField] private float cameraRotateSpeed = 0.1f;
    [SerializeField] private float cameraZoomSpeed = 2.5f;
    private Vector3 previousMousePos;
    private bool isLeftDragging = false;
    private bool isRightDragging = false;

    private float angleX; // x軸
    private float angleY; // y軸


    void Start()
    {
        Vector3 angle = transform.eulerAngles;
        angleX = angle.x;
        angleY = angle.y;
    }

    void Update()
    {
        if (Mouse.current == null && Keyboard.current == null)
        {
            return;
        }

        if (Mouse.current != null)
        {
            if (Keyboard.current != null)
            {
                CameraMove();
            }
            UpdateMouseDragStatus();
            CameraRotate();
            CameraZoom();
            previousMousePos = Mouse.current.position.ReadValue();
        }
    }

    // 左クリックをドラッグしたときにカメラを移動させる関数
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

        transform.Translate(keyboardMove.normalized * keyboardMoveSpeed * Time.deltaTime, Space.Self);

        // マウスによる移動
        if (Mouse.current != null && isRightDragging)
        {
            Vector3 currentMousePos = Mouse.current.position.ReadValue();
            Vector3 mousePosDiff = currentMousePos - previousMousePos;

            transform.Translate(-mousePosDiff * cameraMoveSpeed, Space.Self);
        }
    }

    // 右クリックをドラッグしたときにカメラを回転させる関数
    private void CameraRotate()
    {
        if (isLeftDragging)
        {
            Vector3 currentMousePos = Mouse.current.position.ReadValue();
            Vector3 mousePosDiff = currentMousePos - previousMousePos;

            angleX += mousePosDiff.x * cameraRotateSpeed * -1;
            angleY += mousePosDiff.y * cameraRotateSpeed;
            if (angleX % 360f > 180f)
            {
                angleX -= 360f;
            }
            transform.eulerAngles = new Vector3(angleY, angleX, 0f);
        }
    }

    // マウスホイールでカメラをズームさせる関数
    private void CameraZoom()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;
        transform.Translate(transform.forward * scroll * cameraZoomSpeed, Space.World);
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
