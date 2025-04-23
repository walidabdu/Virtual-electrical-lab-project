using UnityEngine;
using TMPro;

public class ResistorComponent : CircuitComponent1
{
    [Header("Resistor Settings")]
    [SerializeField] private float resistance = 1000f;
    [SerializeField] private Color normalColor = Color.white;
    [HideInInspector] public ResistorSO simulationData;

    protected override void Start()
    {
        base.Start();
        simulationData = ScriptableObject.CreateInstance<ResistorSO>();
        UpdateValueDisplay();
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
            simulationData.resistance = resistance;
        }
    }

    protected override void UpdateValueDisplay()
    {
        if (valueLabel != null)
        {
            valueLabel.text = ConvertToOhmNotation(resistance);
            valueLabel.color = normalColor;
        }
    }

    private string ConvertToOhmNotation(float value)
    {
        if (value >= 1e6f) return $"{value / 1e6f:F2}MΩ";
        if (value >= 1e3f) return $"{value / 1e3f:F2}kΩ";
        return $"{value:F2}Ω";
    }

    public void SetResistance(float value)
    {
        resistance = Mathf.Clamp(value, 1f, 10e6f);
        UpdateValueDisplay();
    }

    protected override void OnLabelClicked()
    {
        if (editorUI != null)
        {
            editorUI.ShowEditor(this, "Resistance (Ω)", resistance, SetResistance);
        }
    }
}