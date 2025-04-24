using UnityEngine;
using TMPro;

public class DCSourceComponent : CircuitComponent1
{
    [Header("DC Source Settings")]
    [SerializeField] private float voltage = 5f;
    [SerializeField] private Color validColor = Color.green;
    [SerializeField] private Color invalidColor = Color.red;
    [HideInInspector] public DCSourceSO simulationData;

    protected override void Start()
    {
        base.Start();
        simulationData = ScriptableObject.CreateInstance<DCSourceSO>();
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
            simulationData.posNode = GetSimulationNodeName(nodeA);
            simulationData.negNode = GetSimulationNodeName(nodeB);
            simulationData.voltage = voltage;
        }
    }

    protected override void UpdateValueDisplay()
    {
        if (valueLabel != null)
        {
            valueLabel.text = $"{voltage:F2}V";
            valueLabel.color = (nodeA != null && nodeB != null) ? validColor : invalidColor;
        }
    }

    public void SetVoltage(float value)
    {
        voltage = Mathf.Clamp(value, 0.1f, 1000f);
        UpdateValueDisplay();
    }

    protected override void OnLabelClicked()
    {
        if (editorUI != null)
        {
            editorUI.ShowEditor(this, "Voltage (V)", voltage, SetVoltage);
        }
    }

   
}