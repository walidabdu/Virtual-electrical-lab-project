using UnityEngine;
using TMPro;

public class InductorComponent : CircuitComponent1
{
    [Header("Inductor Settings")]
    [SerializeField] private float inductance = 0.001f; // Default 1 mH
    [SerializeField] private Color normalColor = Color.white;
    [HideInInspector] public InductorSO simulationData;

    protected override void Start()
    {
        base.Start(); // Call the base class initialization
        simulationData = ScriptableObject.CreateInstance<InductorSO>();
        UpdateValueDisplay();
    }

    public void SetInductance(float value)
    {
        inductance = Mathf.Max(value, 0.000000001f); // Prevent zero or negative values
        UpdateValueDisplay();
    }

    public float GetInductance()
    {
        return inductance;
    }

    protected override void UpdateValueDisplay()
    {
        if (valueLabel != null)
        {
            string displayValue = ConvertToHenryNotation(inductance);
            valueLabel.text = displayValue;
            valueLabel.color = normalColor;
        }
    }

    private string ConvertToHenryNotation(float value)
    {
        if (value >= 1f) return $"{value:F2}H";
        if (value >= 0.001f) return $"{(value * 1000f):F2}mH";
        if (value >= 0.000001f) return $"{(value * 1000000f):F2}�H";
        return $"{(value * 1000000000f):F2}nH";
    }

    protected override void OnLabelClicked()
    {
        if (editorUI != null)
        {
            editorUI.ShowEditor(this, "Set Inductance (H)", inductance, SetInductance);
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
            simulationData.inductance = inductance;
        }
    }
}