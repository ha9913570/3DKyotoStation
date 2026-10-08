using UnityEngine;
using UnityEngine.UI;

public class FloorEmphasisButton : MonoBehaviour
{
    private string objectName;
    private GameObject[] modelObjects;
    private GameObject[] buttonObjects;
    void Start()
    {
        objectName = gameObject.name.Replace("Button_", "");
        modelObjects = GameObject.FindGameObjectsWithTag("3dModel");
        buttonObjects = GameObject.FindGameObjectsWithTag("FloorButton");
    }

    // ボタンがクリックされたときの処理
    public void onButtonClicked()
    {
        SetButtonColor();
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
            string[] floors = obj.name.Split('-');
            bool match = System.Array.IndexOf(floors, objectName) >= 0;
            obj.SetActive(match);
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

    // 自分のボタンの背景色を変更する関数
    private void SetButtonColor()
    {
        foreach (GameObject button in buttonObjects)
        {
            if (button != gameObject)
            {
                button.GetComponent<Image>().color = new Color(1f, 1f, 1f);
            }
        }
        GetComponent<Image>().color = new Color(0.1f, 1f, 1f);
    }
}
