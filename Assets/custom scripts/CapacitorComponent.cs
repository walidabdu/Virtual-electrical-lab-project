using UnityEngine;

public class CapacitorComponent : CircuitComponent1
{
    [Header("Capacitor Settings")]
    [SerializeField] private float capacitance = 1e-6f; // Default 1 �F
    [SerializeField] private Color normalColor = Color.white;
    [HideInInspector] public CapacitorSO simulationData;

    protected override void Start()
    {
        base.Start(); // Call the base class initialization
        simulationData = ScriptableObject.CreateInstance<CapacitorSO>();
        UpdateValueDisplay();
    }

    public void SetCapacitance(float value)
    {
        capacitance = Mathf.Max(value, 1e-12f); // Prevent zero or negative values
        UpdateValueDisplay();
    }

    public float GetCapacitance()
    {
        return capacitance;
    }

    protected override void UpdateValueDisplay()
    {
        if (valueLabel != null)
        {
            string displayValue = ConvertToFaradNotation(capacitance);
            valueLabel.text = displayValue;
            valueLabel.color = normalColor;
        }
    }

    private string ConvertToFaradNotation(float value)
    {
        if (value >= 1e-3f) return $"{(value * 1000f):F2}mF";
        if (value >= 1e-6f) return $"{(value * 1000000f):F2}�F";
        if (value >= 1e-9f) return $"{(value * 1000000000f):F2}nF";
        return $"{(value * 1000000000000f):F2}pF";
    }

    protected override void OnLabelClicked()
    {
        if (editorUI != null)
        {
            editorUI.ShowEditor(this, "Set Capacitance (F)", capacitance, SetCapacitance);
        }
    }

    public override void ConnectToNode(NodePoint node, bool isNodeA)
    {
        HandleNodeConnection(node, isNodeA);
    }

    protected override void PositionAtNode(NodePoint node, bool isNodeA)
    {
        if ((isNodeA && nodeB != null) || (!isNodeA && nodeA != null))
        {
            transform.position = (nodeA.transform.position + nodeB.transform.position) / 2f;
            Vector3 direction = nodeB.transform.position - nodeA.transform.position;
            if (direction != Vector3.zero) transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public override void PrepareForSimulation()
    {
        if (ValidateNodes())
        {
            simulationData.name = componentName;
            simulationData.nodeA = GetSimulationNodeName(nodeA);
            simulationData.nodeB = GetSimulationNodeName(nodeB);
            simulationData.capacitance = capacitance;
        }
    }
}