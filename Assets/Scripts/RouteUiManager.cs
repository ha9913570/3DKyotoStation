using UnityEngine;
using UnityEngine.InputSystem;

public class RouteUiManager : MonoBehaviour
{
    [SerializeField] private GameObject routeSelectMenu;
    [SerializeField] private GameObject mainCamera;

    private InputAction showRouteMenuAction;

    private bool isMenuActive = false;

    public void Awake()
    {
        showRouteMenuAction = InputSystem.actions.FindAction("ShowRouteMenu");
    }

    public void Update()
    {
        if(showRouteMenuAction.WasPressedThisFrame())
        {
            if (isMenuActive)
            {
                HideRouteButtons();
            }
            else
            {
                ShowRouteButtons();
            }
        }
    }

    public void HideRouteButtons()
    {
        isMenuActive = false;
        routeSelectMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mainCamera.GetComponent<CameraManager>().enabled = true;
    }

    private void ShowRouteButtons()
    {
        isMenuActive = true;
        routeSelectMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        mainCamera.GetComponent<CameraManager>().enabled = false;
    }
}
