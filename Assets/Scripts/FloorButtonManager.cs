using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class FloorButtonManager : MonoBehaviour
{
    private int currentSelectedFloor = 0;

    private List<FloorEmphasisButton> childrenList;

    private InputAction floorUpAction;
    private InputAction floorDownAction;

    void Awake()
    {
        childrenList = GetComponentsInChildren<FloorEmphasisButton>().ToList();

        floorUpAction = InputSystem.actions.FindAction("FloorUp");
        floorDownAction = InputSystem.actions.FindAction("FloorDown");
    }

    void Update()
    {
        if (floorUpAction.WasPressedThisFrame())
        {
            FloorUp();
        }
        else if (floorDownAction.WasPressedThisFrame())
        {
            FloorDown();
        }
    }

    // 現在選択中の階を設定する関数
    public void SetCurrentFloor(int floor)
    {
        currentSelectedFloor = floor;
    }

    // 階層名をインデックス値に変換する関数
    public int FloorStringToInt(string floorName)
    {
        return floorName switch
        {
            "All" => 0,
            "4F"  => 1,
            "3F"  => 2,
            "2F"  => 3,
            "1F"  => 4,
            "B1F" => 5,
            "B2F" => 6,
            "B3F" => 7,
            _ => -1,
        };
    }

    // 現在の階を1つ上げる
    private void FloorUp()
    {
        // Allが0
        if (currentSelectedFloor == 0)
        {
            return;
        }
        else
        {
            currentSelectedFloor--;
            childrenList[currentSelectedFloor].OnButtonClicked();
        }
    }

    // 現在の階を1つ下げる
    private void FloorDown()
    {
        // B3Fが7
        if (currentSelectedFloor >= 7)
        {
            return;
        }
        else
        {
            currentSelectedFloor++;
            childrenList[currentSelectedFloor].OnButtonClicked();
        }
    }
}
