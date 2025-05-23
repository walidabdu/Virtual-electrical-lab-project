using UnityEngine;
using TMPro;
using UnityEngine.XR.Content.Interaction;

public class Variac_manager : MonoBehaviour {
    [Header("Variac Settings")]
    public float variac_min_value = 0f;
    public float variac_max_value = 100f;

    [Header("XR References")]
    public XRKnob xrKnob;

    [Header("UI Display")]
    public TMP_Text valueDisplayText;
    public string valueFormat = "0.00";

    private float _currentValue;
    public float Current_value {
        get => _currentValue;
        private set {
            _currentValue = value;
            UpdateValueDisplay();
        }
    }

    private void Start() {
        if (xrKnob == null)
            xrKnob = GetComponent<XRKnob>();

        if (xrKnob != null) {
            xrKnob.onValueChange.AddListener(HandleKnobValueChanged);
            Current_value = Mathf.Lerp(variac_min_value, variac_max_value, xrKnob.value);
        } else {
            Debug.LogError("XRKnob component not found!", this);
        }
    }

    private void HandleKnobValueChanged(float knobValue) {
        Current_value = Mathf.Lerp(variac_min_value, variac_max_value, knobValue);
        CircuitManager3.UpdateVariacValue(Current_value);
    }

    private void UpdateValueDisplay() {
        if (valueDisplayText != null) {
            valueDisplayText.text = Current_value.ToString(valueFormat);
        }
    }

    private void OnDestroy() {
        if (xrKnob != null) {
            xrKnob.onValueChange.RemoveListener(HandleKnobValueChanged);
        }
    }
}