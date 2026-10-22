using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

class BillboardObjectManager : MonoBehaviour
{
    private const int OBJECT_MAX_VISIBLE_DISTANCE = 30000;
    private const int TEXT_LABEL_BASE_FONT_SIZE = 28;
    private Vector3 TEXT_LABEL_POS_OFFSET = new(0, 1.5f, 0);
    private Vector3 ICON_SCALE = new(0.5f, 0.5f, 0.5f);
    private Vector3 ICON_POS_OFFSET = new(0f, 3f, 0f);

    private readonly List<GameObject> billboardObjectList = new();

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private TMP_FontAsset fontAsset;
    [Header("アイコン画像(1024x1024)")]
    [SerializeField] private Sprite wcIconSprite;
    [SerializeField] private Sprite sevenElevenIconSprite;
    [SerializeField] private Sprite familyMartIconSprite;

    private string currentFloor = "All";

    public void Start()
    {
        List<GameObject> gameObjectList = FindObjectsByType<GameObject>(FindObjectsSortMode.None).ToList();

        GenerateTextLabelObjectList(gameObjectList);
        GenerateIconObjectList(gameObjectList);
    }

    public void LateUpdate()
    {
        foreach (GameObject obj in billboardObjectList)
        {
            float distance = (obj.transform.position - mainCamera.transform.position).sqrMagnitude;
            if (IsObjExistCurrentFloor(obj) && distance < OBJECT_MAX_VISIBLE_DISTANCE)
            {
                obj.SetActive(true);
                // カメラの方を向くように回転させる
                obj.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward, mainCamera.transform.up);
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }

    // 現在選択されている階を設定する
    public void SetFloor(string floorName)
    {
        currentFloor = floorName;
    }

    // ゲームオブジェクトが現在選択されている階に存在するかどうかを返す関数
    private bool IsObjExistCurrentFloor(GameObject obj)
    {
        if (currentFloor == "All")
        {
            return true;
        }
        else if (obj.name.Contains("_" + currentFloor))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // テキストラベルのビルボードオブジェクトを作成する関数
    private void GenerateTextLabelObjectList(List<GameObject> gameObjectList)
    {
        foreach (GameObject obj in gameObjectList)
        {
            if (obj.name.StartsWith("label_"))
            {
                TextMeshPro billboardObject = new GameObject(obj.name).AddComponent<TextMeshPro>();

                string labelText = obj.name[9..]; // 「label_nF_」を削除
                if (labelText.StartsWith("_"))
                {
                    labelText = labelText[1..]; // 先頭の「_」を削除
                }
                int fontSize = CalcFontSize(labelText);
                labelText = "<mark=#00000050 padding=\"60,60,10,10\">" + labelText + "</mark>";

                // テキストラベルの設定
                billboardObject.alignment = TextAlignmentOptions.Center;
                billboardObject.color = Color.white;
                billboardObject.font = fontAsset;
                billboardObject.fontSize = fontSize;
                billboardObject.text = labelText;
                billboardObject.textWrappingMode = TextWrappingModes.NoWrap;
                billboardObject.transform.position = obj.transform.position + TEXT_LABEL_POS_OFFSET;

                billboardObjectList.Add(billboardObject.gameObject);
            }
        }
    }

    // テキストラベルのフォントサイズを計算する関数
    private int CalcFontSize(string labelText)
    {
        int length = labelText.Length;
        if (length <= 5)
        {
            return TEXT_LABEL_BASE_FONT_SIZE;
        }
        // 文字数が多い場合はフォントサイズを小さくする
        else if (length <= 20)
        {
            return TEXT_LABEL_BASE_FONT_SIZE - (length - 5);
        }
        else
        {
            return TEXT_LABEL_BASE_FONT_SIZE - 15;
        }
    }

    // アイコンのビルボードオブジェクトを作成する関数
    private void GenerateIconObjectList(List<GameObject> gameObjectList)
    {
        foreach (GameObject obj in gameObjectList)
        {
            // スプライトの設定（アイコン用の名前ではない場合は生成しない）
            Sprite iconSprite;
            if (obj.name.StartsWith("wc_"))
            {
                iconSprite = wcIconSprite;
            }
            else if (obj.name.StartsWith("7eleven_"))
            {
                iconSprite = sevenElevenIconSprite;
            }
            else if (obj.name.StartsWith("fami_"))
            {
                iconSprite = familyMartIconSprite;
            }
            else
            {
                continue;
            }

            SpriteRenderer iconSpriteRenderer = new GameObject(obj.name).AddComponent<SpriteRenderer>();
            iconSpriteRenderer.sprite = iconSprite;
            iconSpriteRenderer.transform.localScale = ICON_SCALE;
            iconSpriteRenderer.transform.position = obj.transform.position + ICON_POS_OFFSET;

            billboardObjectList.Add(iconSpriteRenderer.gameObject);
        }
    }
}
