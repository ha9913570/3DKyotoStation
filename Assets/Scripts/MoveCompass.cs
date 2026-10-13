using UnityEngine;

public class MoveCompass : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;

    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, 180 - mainCamera.transform.eulerAngles.y);
    }
}
