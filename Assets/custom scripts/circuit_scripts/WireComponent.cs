using UnityEngine;

public class WireComponent : ResistorComponent
{
    protected override void Start()
    {
        base.Start();
        componentName = "W" + GetInstanceID();
        SetResistance(0.001f); // Very small value (not exactly 0 to avoid numerical issues)
        UpdateValueDisplay();
    }

    protected override void UpdateValueDisplay()
    {
        if (valueLabel != null)
        {
            valueLabel.text = "Wire";
        }
    }

    // Use a different visual appearance in the Start method or Awake
    protected override void Awake()
    {
        base.Awake();

        // You can modify the appearance to look like a wire
        // For example, make it thinner, change color, etc.
        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        if (renderer != null)
        {
            // Make it look like a wire instead of a resistor
            renderer.material.color = Color.red; // Or use a wire material
        }
    }
}