using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateFacilityIcon : MonoBehaviour
{
    private Vector3 ICON_POS_OFFSET = new(0f, 3f, 0f);
    private List<GameObject> wcObjectList;
    private string currentSelectedFloor = "All";
    [SerializeField] private Sprite wcImageSprite;
    [SerializeField] private GameObject mainCamera;

    void Start()
    {
        // 「wc_」で始まるオブジェクトを取得
        List<GameObject> gameObjectList = FindObjectsByType<GameObject>(FindObjectsSortMode.None).ToList();
        for (int i = 0; i < gameObjectList.Count; i++)
        {
            if (!gameObjectList[i].name.StartsWith("wc_"))
            {
                gameObjectList.RemoveAt(i);
                i--;
            }
        }
        wcObjectList = gameObjectList;

        // 画像オブジェクトを作成
        for (int i = 0; i < wcObjectList.Count; i++)
        {
            SpriteRenderer wcSpriteObject = new GameObject(wcObjectList[i].name).AddComponent<SpriteRenderer>();
            wcSpriteObject.sprite = wcImageSprite;
            wcSpriteObject.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            wcSpriteObject.transform.position = wcObjectList[i].transform.position + ICON_POS_OFFSET;

            wcObjectList[i] = wcSpriteObject.gameObject;
        }
    }

    void LateUpdate()
    {
        foreach (GameObject obj in wcObjectList)
        {
            bool isSelectedFloor = currentSelectedFloor == "All" || IsWcOnFloor(obj, currentSelectedFloor);
            obj.SetActive(isSelectedFloor);

            obj.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward, mainCamera.transform.up);
        }
    }

    // トイレアイコンが指定した階に存在しているかどうかを判定する関数
    private bool IsWcOnFloor(GameObject label, string floor)
    {
        return label.name.StartsWith($"wc_{floor}");
    }

    public void SetFloor(string floor)
    {
        currentSelectedFloor = floor;
    }
}
