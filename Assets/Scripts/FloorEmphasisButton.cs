using UnityEngine;

public class FloorEmphasisButton : MonoBehaviour
{
    private string objectName;
    private GameObject[] modelObjects;
    void Start()
    {
        objectName = gameObject.name.Replace("Button_", "");
        modelObjects = GameObject.FindGameObjectsWithTag("3dModel");
    }

    // ボタンがクリックされたときの処理
    public void onButtonClicked()
    {
        if (objectName == "All")
        {
            AllModelObjectsActive();
        }
        else
        {
            SetButtonActive();
        }
    }

    // 自分のボタンに対応する3Dモデルオブジェクトをアクティブにし、それ以外を非アクティブにする関数
    private void SetButtonActive()
    {
        foreach (GameObject obj in modelObjects)
        {
            if (obj.name.Contains(objectName))
            {
                obj.SetActive(true);
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }

    // すべての3Dモデルオブジェクトをアクティブにする関数
    private void AllModelObjectsActive()
    {
        foreach (GameObject obj in modelObjects)
        {
            obj.SetActive(true);
        }
    }
}
