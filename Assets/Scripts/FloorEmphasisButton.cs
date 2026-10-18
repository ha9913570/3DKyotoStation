using UnityEngine;
using UnityEngine.UI;

public class FloorEmphasisButton : MonoBehaviour
{
    private const float OTHER_FLOOR_ALPHA = 0.05f;

    private string objectName;
    private GameObject[] modelObjects;
    private GameObject[] buttonObjects;
    private GenerateTextLabel textLabel;
    private GenerateFacilityIcon facilityIcon;

    void Start()
    {
        objectName = gameObject.name.Replace("Button_", "");
        modelObjects = GameObject.FindGameObjectsWithTag("3dModel");
        buttonObjects = GameObject.FindGameObjectsWithTag("FloorButton");
        textLabel = FindFirstObjectByType<GenerateTextLabel>();
        facilityIcon = FindFirstObjectByType<GenerateFacilityIcon>();

        // 半透明用マテリアルを Resources から読み込み、各モデルに FloorTransparency を付与
        Material transparentBase = Resources.Load<Material>("TransparentBase");
        foreach (GameObject obj in modelObjects)
        {
            if (obj.GetComponent<FloorTransparency>() != null) continue;
            obj.AddComponent<FloorTransparency>().Init(transparentBase);
        }
    }

    // ボタンがクリックされたときの処理
    public void OnButtonClicked()
    {
        SetButtonColor();
        if (objectName == "All")
        {
            AllModelObjectsOpaque();
        }
        else
        {
            SetButtonActive();
        }
        textLabel.SetFloor(objectName);
        facilityIcon.SetFloor(objectName);
    }

    // 選択した階は不透明、それ以外は半透明(または非表示)にする
    private void SetButtonActive()
    {
        foreach (GameObject obj in modelObjects)
        {
            string[] floors = obj.name.Split('-');
            bool match = System.Array.IndexOf(floors, objectName) >= 0;

            var ft = obj.GetComponent<FloorTransparency>();
            if (ft != null) ft.SetTransparent(!match, OTHER_FLOOR_ALPHA);
        }
    }

    // すべての3Dモデルを不透明にする
    private void AllModelObjectsOpaque()
    {
        foreach (GameObject obj in modelObjects)
        {
            var ft = obj.GetComponent<FloorTransparency>();
            if (ft != null) ft.SetTransparent(false, 1f);
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
