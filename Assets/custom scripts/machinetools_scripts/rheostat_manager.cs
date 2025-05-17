using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.XR.Content.Interaction; // Add this namespace for TextMeshPro

public class rheostat_manager : MonoBehaviour
{
    [Header("Rheostat Settings")]
    public float rheostat_min_value = 0f;
    public float rheostat_max_value = 100f;
    
    [Header("XR References")]
    public XRKnob xrKnob;
    
    [Header("UI Display")]
    public TMP_Text valueDisplayText; // Reference to your TextMeshPro UI element
    public string valueFormat = "0.00"; // Format for displaying the value
    
    private float _currentValue;
    public float current_value
    {
        get => _currentValue;
        private set
        {
            _currentValue = value;
            UpdateValueDisplay(); // Update display whenever value changes
        }
    }

    private void Start()
    {
        // Get XRKnob if not assigned
        if (xrKnob == null)
            xrKnob = GetComponent<XRKnob>();
        
        if (xrKnob != null)
        {
            xrKnob.onValueChange.AddListener(HandleKnobValueChanged);
            
            // Initialize with current knob value
            current_value = Mathf.Lerp(rheostat_min_value, rheostat_max_value, xrKnob.value);
        }
        else
        {
            Debug.LogError("XRKnob component not found!", this);
        }
    }

    private void HandleKnobValueChanged(float knobValue)
    {
        current_value = Mathf.Lerp(rheostat_min_value, rheostat_max_value, knobValue);
    }

    private void UpdateValueDisplay()
    {
        if (valueDisplayText != null)
        {
            valueDisplayText.text = current_value.ToString(valueFormat);
        }
    }

    private void OnDestroy()
    {
        if (xrKnob != null)
        {
            xrKnob.onValueChange.RemoveListener(HandleKnobValueChanged);
        }
    }
}