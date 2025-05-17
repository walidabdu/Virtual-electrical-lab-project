using UnityEngine;
using TMPro; // Added for TextMeshPro support

public class NeedlePointer : MonoBehaviour
{
    [Header("Needle Settings")]
    [Tooltip("The current value to display")]
    public float currentValue = 0f;
    
    [Tooltip("Minimum possible value")]
    public float minValue = 0f;
    
    [Tooltip("Maximum possible value")]
    public float maxValue = 100f;
    
    [Header("Rotation References")]
    [Tooltip("Transform representing the minimum rotation")]
    public Transform minAngleTransform;
    
    [Tooltip("Transform representing the maximum rotation")]
    public Transform maxAngleTransform;
    
    [Header("Smoothing")]
    [Tooltip("Smoothing speed (0 for instant)")]
    public float smoothingSpeed = 5f;

    [Header("Value Display (Optional)")]
    [Tooltip("TextMeshPro component to display the value")]
    public TMP_Text valueDisplayText;
    
    [Tooltip("Display format (e.g., '0.0', '0.00 Ω')")]
    public string valueFormat = "0.0";

    private Quaternion _minRotation;
    private Quaternion _maxRotation;
    private Quaternion _targetRotation;

    private void Start()
    {
        // Cache the rotations at startup
        if (minAngleTransform != null) 
        {
            _minRotation = minAngleTransform.rotation;
        }
        else
        {
            Debug.LogError("Min Angle Transform not assigned!");
            _minRotation = Quaternion.identity;
        }

        if (maxAngleTransform != null)
        {
            _maxRotation = maxAngleTransform.rotation;
        }
        else
        {
            Debug.LogError("Max Angle Transform not assigned!");
            _maxRotation = Quaternion.identity;
        }

        // Initialize display if available
        UpdateValueDisplay();
    }

    void Update()
    {
        if (minAngleTransform == null || maxAngleTransform == null)
            return;

        // Calculate the normalized value (0 to 1)
        float normalizedValue = Mathf.InverseLerp(minValue, maxValue, currentValue);
        
        // Interpolate between the min and max rotations
        _targetRotation = Quaternion.Slerp(_minRotation, _maxRotation, normalizedValue);
        
        // Apply rotation with or without smoothing
        if (smoothingSpeed > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * smoothingSpeed);
        }
        else
        {
            transform.rotation = _targetRotation;
        }
    }

    public void SetValue(float value)
    {
        currentValue = Mathf.Clamp(value, minValue, maxValue);
        UpdateValueDisplay();
    }

    public void ConfigureNeedle(float value, float min, float max, Transform minTransform, Transform maxTransform)
    {
        currentValue = value;
        minValue = min;
        maxValue = max;
        minAngleTransform = minTransform;
        maxAngleTransform = maxTransform;
        
        // Update cached rotations
        if (minAngleTransform != null) _minRotation = minAngleTransform.rotation;
        if (maxAngleTransform != null) _maxRotation = maxAngleTransform.rotation;
        
        UpdateValueDisplay();
    }

    // Call this if you change the transform rotations at runtime
    public void UpdateRotationReferences()
    {
        if (minAngleTransform != null) _minRotation = minAngleTransform.rotation;
        if (maxAngleTransform != null) _maxRotation = maxAngleTransform.rotation;
    }

    // Added TextMeshPro display functionality
    private void UpdateValueDisplay()
    {
        if (valueDisplayText != null)
        {
            valueDisplayText.text = currentValue.ToString(valueFormat);
        }
    }
}