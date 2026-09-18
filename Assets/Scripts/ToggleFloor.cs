using System;
using Unity.VisualScripting;
using UnityEngine;

public class ToggleFloor : MonoBehaviour
{
    GameObject target;
    void Start()
    {
        string objectName = gameObject.name;
        string floorName = objectName.Replace("Toggle", "");
        target = GameObject.Find(floorName);
    }
    void Update()
    {

    }
    public void onToggleChanged()
    {
        if (target.activeSelf == true)
        {
            target.SetActive(false);
        }
        else
        {
            target.SetActive(true);
        }
    }
}
