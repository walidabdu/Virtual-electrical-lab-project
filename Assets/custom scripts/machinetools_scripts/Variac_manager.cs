using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Collections.Generic;
using UnityEngine.XR.Content.Interaction;

public class Variac_manager : MonoBehaviour
{
    [Header("Rheostat Settings")]
    public float rheostat_min_value = 0f;
    public float rheostat_max_value = 100f;

    [Header("XR References")]
    public XRKnob xrKnob;

    [Header("Controlled Needles")]
    public List<NeedlePointer> targetNeedles; // Assign needles in Inspector

    [Header("Rheostat Display (Optional)")]
    public TMP_Text rheostatDisplayText; // TextMeshPro for rheostat's value
    public string valueFormat = "0.0";   // Format (e.g., "0.0", "0.00 Ω")

    private float _currentRheostatValue;

    private void Start()
    {
        if (xrKnob == null)
            xrKnob = GetComponent<XRKnob>();

        if (xrKnob != null)
        {
            xrKnob.onValueChange.AddListener(UpdateNeedles);
            _currentRheostatValue = Mathf.Lerp(rheostat_min_value, rheostat_max_value, xrKnob.value);
            UpdateRheostatDisplay();
        }
        else
        {
            Debug.LogError("XRKnob not assigned!", this);
        }
    }

    private void UpdateNeedles(float knobValue)
    {
        // Calculate the mapped rheostat value (0-1 → min/max range)
        _currentRheostatValue = Mathf.Lerp(rheostat_min_value, rheostat_max_value, knobValue);

        // Update all assigned needles
        foreach (var needle in targetNeedles)
        {
            if (needle != null)
            {
                needle.SetValue(_currentRheostatValue);
            }
        }

        UpdateRheostatDisplay(); // Update TextMeshPro
    }

    private void UpdateRheostatDisplay()
    {
        if (rheostatDisplayText != null)
        {
            rheostatDisplayText.text = _currentRheostatValue.ToString(valueFormat);
        }
    }

    private void OnDestroy()
    {
        if (xrKnob != null)
            xrKnob.onValueChange.RemoveListener(UpdateNeedles);
    }

    // Runtime needle assignment (optional)
    public void AddNeedle(NeedlePointer needle) => targetNeedles.Add(needle);
    public void RemoveNeedle(NeedlePointer needle) => targetNeedles.Remove(needle);
}