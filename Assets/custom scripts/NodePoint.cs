using UnityEngine;
using System.Collections.Generic;

public class NodePoint : MonoBehaviour
{
    public int row;
    public int column;
    public string nodeName;
    public bool isGround = false;

    public List<CircuitComponent1> connectedComponents = new List<CircuitComponent1>();
    public List<JumperWire> connectedWires = new List<JumperWire>();

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Material groundMaterial;

    private Renderer nodeRenderer;

    public void Initialize(int r, int c)
    {
        row = r;
        column = c;
        nodeRenderer = GetComponent<Renderer>();
        nodeRenderer.material = defaultMaterial;
    }

    public void SetAsGround()
    {
        isGround = true;
        nodeRenderer.material = groundMaterial;
    }

    public bool IsGround()
    {
        return isGround;
    }

    public void Select()
    {
        if (!isGround)
            nodeRenderer.material = selectedMaterial;
    }

    public void Deselect()
    {
        nodeRenderer.material = isGround ? groundMaterial : defaultMaterial;
    }

    public void ConnectComponent(CircuitComponent1 component)
    {
        if (!connectedComponents.Contains(component))
            connectedComponents.Add(component);
    }

    public void DisconnectComponent(CircuitComponent1 component)
    {
        connectedComponents.Remove(component);
    }

    public void ConnectWire(JumperWire wire)
    {
        if (!connectedWires.Contains(wire))
            connectedWires.Add(wire);
    }

    public void DisconnectWire(JumperWire wire)
    {
        connectedWires.Remove(wire);
    }
    public void ResetNodeName()
    {
        if (!isGround)
            nodeName = null;
    }

    // Make sure the IsGround method is properly implemented it creates error other wise
 
}