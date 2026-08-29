using UnityEditor.SpeedTree.Importer;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private float cameraMoveSpeed = 0.05f;
    [SerializeField] private float cameraRotateSpeed = 0.1f;
    [SerializeField] private float cameraZoomSpeed = 1.5f;
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
        if (Mouse.current == null)
        {
            return;
        }

        CameraMove();
        CameraRotate();
        CameraZoom();
    }

    // 左クリックをドラッグしたときにカメラを移動させる関数
    private void CameraMove()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isLeftDragging = true;
            previousMousePos = Mouse.current.position.ReadValue();
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isLeftDragging = false;
        }

        if (isLeftDragging)
        {
            Vector3 currentMousePos = Mouse.current.position.ReadValue();
            Vector3 mousePosDiff = currentMousePos - previousMousePos;

            transform.Translate(-mousePosDiff * cameraMoveSpeed, Space.Self);

            previousMousePos = currentMousePos;
        }
    }

    // 右クリックをドラッグしたときにカメラを回転させる関数
    private void CameraRotate()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            isRightDragging = true;
            previousMousePos = Mouse.current.position.ReadValue();
        }
        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            isRightDragging = false;
        }

        if (isRightDragging)
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

            previousMousePos = currentMousePos;
        }
    }

    // マウスホイールでカメラをズームさせる関数
    private void CameraZoom()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;
        transform.Translate(transform.forward * scroll * cameraZoomSpeed, Space.World);
    }
}
