using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TextLabel : MonoBehaviour
{
    private const int TEXT_LABEL_LOD_DISTANCE = 10000;
    List<GameObject> gameObjectList;

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
            textLabel.alignment = TextAlignmentOptions.Center;
            textLabel.font = notoSansFont;
            textLabel.fontSize = 20;
            textLabel.text = obj.name.Substring(6); // 「label_」を削除
            textLabel.transform.position = obj.transform.position;
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

            // カメラが移動したら向きを変更する
            if (direction.sqrMagnitude > Mathf.Epsilon)
            {
                obj.transform.rotation = Quaternion.LookRotation(direction, mainCamera.transform.up);
            }
        }
    }
}
