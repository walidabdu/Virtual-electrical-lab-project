using UnityEngine;

public class JumperWire : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Material wireDefaultMaterial;
    [SerializeField] private Material wireSelectedMaterial;
    [SerializeField] private GameObject wireComponentPrefab; // Add this to reference your Wire prefab

    public NodePoint startNode;
    public NodePoint endNode;

    private bool isPlacing = false;
    private bool isSelected = false;
    private WireComponent createdWireComponent;

    private void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.material = wireDefaultMaterial;
    }

    public void StartPlacement(NodePoint startingNode)
    {
        startNode = startingNode;
        startNode.ConnectWire(this);
        isPlacing = true;

        // Set initial positions
        lineRenderer.SetPosition(0, startNode.transform.position + Vector3.up * 0.05f);
        lineRenderer.SetPosition(1, startNode.transform.position + Vector3.up * 0.05f);
    }

    public void UpdatePlacement(Vector3 currentPosition)
    {
        if (isPlacing)
        {
            lineRenderer.SetPosition(1, currentPosition + Vector3.up * 0.05f);
        }
    }

    public bool CompletePlacement(NodePoint endingNode)
    {
        // Can't connect to the same node
        if (startNode == endingNode)
            return false;

        endNode = endingNode;
        endNode.ConnectWire(this);
        isPlacing = false;

        // Set final positions
        lineRenderer.SetPosition(0, startNode.transform.position + Vector3.up * 0.05f);
        lineRenderer.SetPosition(1, endNode.transform.position + Vector3.up * 0.05f);

        // Create a wire component between the nodes
        CreateWireComponent();

        return true;
    }

    private void CreateWireComponent()
    {
        if (wireComponentPrefab != null && startNode != null && endNode != null)
        {
            // Create the wire component between the nodes
            GameObject wireObj = Instantiate(wireComponentPrefab);
            createdWireComponent = wireObj.GetComponent<WireComponent>();

            if (createdWireComponent != null)
            {
                // Connect it to both nodes
                createdWireComponent.ConnectToNode(startNode, true);
                createdWireComponent.ConnectToNode(endNode, false);

                // Hide the visual representation since we're using the line renderer
                MeshRenderer[] renderers = createdWireComponent.GetComponentsInChildren<MeshRenderer>();
                foreach (var renderer in renderers)
                {
                    renderer.enabled = false;
                }
            }
        }
    }

    public void CancelPlacement()
    {
        if (isPlacing)
        {
            if (startNode != null)
                startNode.DisconnectWire(this);

            Destroy(gameObject);
        }
    }

    public void Select()
    {
        isSelected = true;
        lineRenderer.material = wireSelectedMaterial;
    }

    public void Deselect()
    {
        isSelected = false;
        lineRenderer.material = wireDefaultMaterial;
    }

    public void Remove()
    {
        if (startNode != null)
            startNode.DisconnectWire(this);

        if (endNode != null)
            endNode.DisconnectWire(this);

        // Also remove the created wire component
        if (createdWireComponent != null)
        {
            Destroy(createdWireComponent.gameObject);
        }

        Destroy(gameObject);
    }
}