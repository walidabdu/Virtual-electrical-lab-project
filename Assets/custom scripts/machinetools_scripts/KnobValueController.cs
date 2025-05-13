using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRKnob))]
public class KnobValueController : MonoBehaviour
{
    [Header("Value Settings")]
    [Tooltip("Minimum output value")]
    public float minValue = 0f;
    
    [Tooltip("Maximum output value")]
    public float maxValue = 100f;

    [Header("Events")]
    public UnityEvent<float> onValueChanged;

    private XRKnob _knob;

    private void Awake()
    {
        _knob = GetComponent<XRKnob>();
        _knob.onValueChange.AddListener(HandleKnobValueChanged);
    }

    private void HandleKnobValueChanged(float knobValue)
    {
        float mappedValue = Mathf.Lerp(minValue, maxValue, knobValue);
        onValueChanged.Invoke(mappedValue);
    }

    private void OnDestroy()
    {
        if (_knob != null)
        {
            _knob.onValueChange.RemoveListener(HandleKnobValueChanged);
        }
    }
}