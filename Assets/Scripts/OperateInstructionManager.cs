using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class OperateInstructionManager : MonoBehaviour
{
    [SerializeField] private GameObject panelObject;
    [SerializeField] private GameObject buttonObject;
    [SerializeField] private TextMeshProUGUI buttonTextObject;

    private bool isPanelVisible = true;

    private InputAction toggleOperateInstruction;

    void Awake()
    {
        toggleOperateInstruction = InputSystem.actions.FindAction("ToggleOperateInstruction");
    }

    void Update()
    {
        if(toggleOperateInstruction.WasPressedThisFrame())
        {
            OnButtonClicked();
        }
    }

    public void OnButtonClicked()
    {
        if (isPanelVisible)
        {
            SetPanelInvisible();
        }
        else
        {
            SetPanelVisible();
        }
    }

    // 操作説明を非表示にする関数
    private void SetPanelInvisible()
    {
        isPanelVisible = false;
        panelObject.SetActive(isPanelVisible);
        buttonObject.SetActive(isPanelVisible);
    }

    // 操作説明を表示する関数
    private void SetPanelVisible()
    {
        isPanelVisible = true;
        panelObject.SetActive(isPanelVisible);
        buttonObject.SetActive(isPanelVisible);
    }
}
