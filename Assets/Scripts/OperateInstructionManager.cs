using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class OperateInstructionManager : MonoBehaviour
{
    private Vector3 VISIBLE_BUTTON_POS = new(165f, 0f, 0f);
    private Vector3 INVISIBLE_BUTTON_POS = new(-120f, 0f, 0f);

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
        buttonObject.GetComponent<RectTransform>().anchoredPosition = INVISIBLE_BUTTON_POS;
        buttonTextObject.text = "Tab で表示";
        panelObject.SetActive(isPanelVisible);
    }

    // 操作説明を表示する関数
    private void SetPanelVisible()
    {
        isPanelVisible = true;
        buttonObject.GetComponent<RectTransform>().anchoredPosition = VISIBLE_BUTTON_POS;
        buttonTextObject.text = "Tab で非表示";
        panelObject.SetActive(isPanelVisible);
    }
}
