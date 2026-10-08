using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TextLabel : MonoBehaviour
{
    private const int TEXT_LABEL_LOD_DISTANCE = 10000;
    private List<GameObject> gameObjectList;
    private Vector3 OFFSET = new Vector3(0, 0.8f, 0); // テキストラベルの位置調整用
    private const int BASE_FONT_SIZE = 28; // 基本のフォントサイズ

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
            string labelText = obj.name.Substring(6); // 「label_」を削除
            textLabel.alignment = TextAlignmentOptions.Center;
            textLabel.font = notoSansFont;
            textLabel.fontSize = calcFontSize(labelText);
            textLabel.color = Color.white;
            textLabel.text = labelText;
            textLabel.textWrappingMode = TextWrappingModes.NoWrap;
            textLabel.transform.position = obj.transform.position + OFFSET;
            gameObjectList[i] = textLabel.gameObject;
        }
    }

    void LateUpdate()
    {
        Camera mainCamera = Camera.main;

        // 各ラベルをカメラの方向へ向ける（ラベル自身の位置を基準にする）
        foreach (GameObject obj in gameObjectList)
        {
            Vector3 direction = obj.transform.position - mainCamera.transform.position;
            float distance = direction.sqrMagnitude;

            // カメラが遠いなら非表示にする
            if (distance > TEXT_LABEL_LOD_DISTANCE)
            {
                obj.SetActive(false);
            }
            else
            {
                obj.SetActive(true);
            }

            // 画面上で常に水平に見えるよう、カメラの向きに合わせる
            obj.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward, mainCamera.transform.up);
        }
    }

    // フォントサイズを計算する関数
    private int calcFontSize(string labelText)
    {
        int length = labelText.Length;
        if (length <= 5)
        {
            return BASE_FONT_SIZE;
        }
        // 文字数が多い場合はフォントサイズを小さくする
        else
        {
            return BASE_FONT_SIZE - (length - 5);
        }
    }
}
