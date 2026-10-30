using UnityEngine;

public class ShowRoute : MonoBehaviour
{
    [SerializeField] private GameObject routeObject;

    private bool isActive = false;
    private RouteUiManager routeUiManager;

    private void Awake()
    {
        routeUiManager = FindFirstObjectByType<RouteUiManager>();
    }

    public void OnButtonClicked()
    {
        isActive = !isActive;
        routeObject.SetActive(isActive);
        routeUiManager.HideRouteButtons();
    }
}
