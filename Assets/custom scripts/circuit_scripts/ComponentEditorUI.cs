using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ComponentEditorUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject editPanel;
    [SerializeField] private TMP_InputField valueInputField;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI unitText;

    private System.Action<float> onValueConfirmed;

    void Start()
    {
        editPanel.SetActive(false);
        confirmButton.onClick.AddListener(ConfirmValue);
        cancelButton.onClick.AddListener(CancelEdit);
        
    }

    // Updated ShowEditor method with 4 parameters
    public void ShowEditor(CircuitComponent1 component, string title, float currentValue, System.Action<float> callback)
    {
        // Extract unit from title if format is "Name (unit)"
        string unit = title.Contains("(") ? title.Split('(', ')')[1] : "";
        unitText.text = unit;
        titleText.text = title;
        valueInputField.text = currentValue.ToString("F2");
        onValueConfirmed = callback;

        editPanel.SetActive(true);
        valueInputField.Select();
        valueInputField.ActivateInputField();
    }

    private void ConfirmValue()
    {
        if (float.TryParse(valueInputField.text, out float newValue))
        {
            onValueConfirmed?.Invoke(newValue);
        }
        editPanel.SetActive(false);
    }

    private void CancelEdit() => editPanel.SetActive(false);
}