using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TextLabel : MonoBehaviour
{
    private const int TEXT_LABEL_LOD_DISTANCE = 10000;
    private const int BASE_FONT_SIZE = 28; // 基本のフォントサイズ
    private Vector3 OFFSET = new(0, 1f, 0); // テキストラベルの位置調整用
    private List<GameObject> gameObjectList;
    private string selectedFloor = "All";
    [SerializeField] private GameObject mainCamera;

    void Start()
    {
        // フォントを読み込み
        Font notoSans = Resources.Load<Font>("Fonts/NotoSansJP-Regular");
        TMP_FontAsset notoSansFont = TMP_FontAsset.CreateFontAsset(notoSans);

        // 「label_」で始まるオブジェクトを取得
        gameObjectList = FindObjectsByType<GameObject>(FindObjectsSortMode.None).ToList();
        for (int i = 0; i < gameObjectList.Count; i++)
        {
            if (!gameObjectList[i].name.StartsWith("label_"))
            {
                gameObjectList.RemoveAt(i);
                i--;
            }
        }

        // TextMeshProを生成
        for (int i = 0; i < gameObjectList.Count; i++)
        {
            GameObject obj = gameObjectList[i];
            TextMeshPro textLabel = new GameObject(obj.name).AddComponent<TextMeshPro>();
            string labelText = obj.name[9..]; // 「label_nF_」を削除
            if (labelText.StartsWith("_"))
            {
                labelText = labelText[1..]; // 先頭の「_」を削除
            }

            textLabel.alignment = TextAlignmentOptions.Center;
            textLabel.font = notoSansFont;
            textLabel.fontSize = CalcFontSize(labelText);
            textLabel.color = Color.white;
            textLabel.text = labelText;
            textLabel.textWrappingMode = TextWrappingModes.NoWrap;
            textLabel.transform.position = obj.transform.position + OFFSET;
            gameObjectList[i] = textLabel.gameObject;
        }
    }

    void LateUpdate()
    {
        foreach (GameObject obj in gameObjectList)
        {
            Vector3 direction = obj.transform.position - mainCamera.transform.position;
            float distance = direction.sqrMagnitude;
            bool isSelectedFloor = selectedFloor == "All" || IsLabelOnFloor(obj, selectedFloor);

            // 選択階にあるかつカメラからの距離が一定以内の場合にラベルを表示する
            obj.SetActive(isSelectedFloor && distance <= TEXT_LABEL_LOD_DISTANCE);

            // 画面上で常に水平に見えるよう、カメラの向きに合わせる
            obj.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward, mainCamera.transform.up);
        }
    }

    // 現在選択されている階を設定する関数
    public void SetFloor(string floor)
    {
        selectedFloor = floor;
    }

    // ラベルが指定した階に存在しているかどうかを判定する関数
    private bool IsLabelOnFloor(GameObject label, string floor)
    {
        return label.name.StartsWith($"label_{floor}_");
    }

    // フォントサイズを計算する関数
    private int CalcFontSize(string labelText)
    {
        int length = labelText.Length;
        if (length <= 5)
        {
            return BASE_FONT_SIZE;
        }
        // 文字数が多い場合はフォントサイズを小さくする
        else if (length <= 20)
        {
            return BASE_FONT_SIZE - (length - 5);
        }
        else
        {
            return BASE_FONT_SIZE - 15;
        }
    }
}
