using UnityEngine;
using UnityEngine.InputSystem;

public class RouteUiManager : MonoBehaviour
{
    [SerializeField] private GameObject routeSelectMenu;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject[] routeObjects;
    [SerializeField] private GameObject[] routeSelectButtons;

    private InputAction showRouteMenuAction;

    private bool isMenuActive = false;

    public void Awake()
    {
        showRouteMenuAction = InputSystem.actions.FindAction("ShowRouteMenu");
    }

    public void Update()
    {
        if (showRouteMenuAction.WasPressedThisFrame())
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
        HideAllRoutes();
        isMenuActive = true;
        routeSelectMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        mainCamera.GetComponent<CameraManager>().enabled = false;
    }

    private void HideAllRoutes()
    {
        foreach (GameObject routeObject in routeObjects)
        {
            routeObject.SetActive(false);
        }

        foreach (GameObject routeSelectButton in routeSelectButtons)
        {
            routeSelectButton.GetComponent<ShowRoute>().SetIsActive(false);
        }
    }
}
