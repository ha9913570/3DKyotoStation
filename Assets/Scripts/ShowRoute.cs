using UnityEngine;

public class ShowRoute : MonoBehaviour
{
    [SerializeField] private GameObject routeObject;

    private RouteUiManager routeUiManager;

    public void Awake()
    {
        routeUiManager = FindFirstObjectByType<RouteUiManager>();
    }

    public void OnButtonClicked()
    {
        SetIsActive(true);
        routeUiManager.HideRouteButtons();
    }

    public void SetIsActive(bool value)
    {
        routeObject.SetActive(value);
    }
}
