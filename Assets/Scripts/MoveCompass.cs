using UnityEngine;

public class MoveCompass : MonoBehaviour
{
    private GameObject mainCamera;
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
    }

    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, 180 - mainCamera.transform.eulerAngles.y);
    }
}
