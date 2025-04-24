using UnityEngine;

public class CircuitBuilderController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CircuitBoard circuitBoard;
    [SerializeField] private GameObject resistorPrefab;
    [SerializeField] private GameObject dcSourcePrefab;
    [SerializeField] private GameObject jumperWirePrefab;
    [SerializeField] private GameObject inductorPrefab; // New prefab for inductor
    [SerializeField] private GameObject capacitorPrefab; // New prefab for capacitor
    [SerializeField] private LayerMask nodeLayerMask;
    [SerializeField] private LayerMask componentLayerMask;

    private CircuitBuilderUI.BuildMode currentMode = CircuitBuilderUI.BuildMode.Select;
    private NodePoint selectedNode;
    private CircuitComponent1 selectedComponent;
    private JumperWire selectedWire;
    private JumperWire wireBeingPlaced;

    private CircuitComponent1 componentBeingPlaced;
    private bool isPlacingComponent = false;
    private NodePoint firstNode;

    void Update()
    {
        HandleInput();
    }

    public void SetBuildMode(CircuitBuilderUI.BuildMode mode)
    {
        DeselectAll();
        currentMode = mode;
    }

    private void HandleInput()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Update wire placement
        if (wireBeingPlaced != null)
        {
            wireBeingPlaced.UpdatePlacement(GetMouseWorldPosition());
        }

        // Update component placement
        if (isPlacingComponent && componentBeingPlaced != null && firstNode != null)
        {
            Vector3 mousePos = GetMouseWorldPosition();
            componentBeingPlaced.transform.position = (firstNode.transform.position + mousePos) / 2f;

            Vector3 direction = mousePos - firstNode.transform.position;
            if (direction != Vector3.zero)
            {
                componentBeingPlaced.transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        // Handle mouse click
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, 100f, nodeLayerMask))
            {
                NodePoint hitNode = hit.collider.GetComponent<NodePoint>();
                if (hitNode != null)
                {
                    HandleNodeClick(hitNode);
                }
            }
            else if (Physics.Raycast(ray, out hit, 100f, componentLayerMask))
            {
                CircuitComponent1 hitComponent = hit.collider.GetComponent<CircuitComponent1>();
                JumperWire hitWire = hit.collider.GetComponent<JumperWire>();

                if (hitComponent != null)
                {
                    HandleComponentClick(hitComponent);
                }
                else if (hitWire != null)
                {
                    HandleWireClick(hitWire);
                }
            }
            else
            {
                DeselectAll();
            }
        }

        // Handle cancel
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelCurrentAction();
        }
    }

    private void HandleNodeClick(NodePoint node)
    {
        switch (currentMode)
        {
            case CircuitBuilderUI.BuildMode.Select:
                SelectNode(node);
                break;

            case CircuitBuilderUI.BuildMode.PlaceResistor:
                PlaceComponentAtNode(node, resistorPrefab);
                break;

            case CircuitBuilderUI.BuildMode.PlaceDCSource:
                PlaceComponentAtNode(node, dcSourcePrefab);
                break;

            case CircuitBuilderUI.BuildMode.PlaceInductor: // New case for inductor
                PlaceComponentAtNode(node, inductorPrefab);
                break;

            case CircuitBuilderUI.BuildMode.PlaceCapacitor: // New case for capacitor
                PlaceComponentAtNode(node, capacitorPrefab);
                break;

            case CircuitBuilderUI.BuildMode.PlaceWire:
                PlaceWireAtNode(node);
                break;

            case CircuitBuilderUI.BuildMode.Remove:
                // Nothing to do for nodes
                break;
        }
    }

    private void HandleComponentClick(CircuitComponent1 component)
    {
        switch (currentMode)
        {
            case CircuitBuilderUI.BuildMode.Select:
                SelectComponent(component);
                break;

            case CircuitBuilderUI.BuildMode.Remove:
                RemoveComponent(component);
                break;

            default:
                break;
        }
    }

    private void HandleWireClick(JumperWire wire)
    {
        switch (currentMode)
        {
            case CircuitBuilderUI.BuildMode.Select:
                SelectWire(wire);
                break;

            case CircuitBuilderUI.BuildMode.Remove:
                wire.Remove();
                break;

            default:
                break;
        }
    }

    private void SelectNode(NodePoint node)
    {
        DeselectAll();
        selectedNode = node;
        selectedNode.Select();
    }

    private void SelectComponent(CircuitComponent1 component)
    {
        DeselectAll();
        selectedComponent = component;
        // Highlight the component somehow
    }

    private void SelectWire(JumperWire wire)
    {
        DeselectAll();
        selectedWire = wire;
        wire.Select();
    }

    private void DeselectAll()
    {
        if (selectedNode != null)
        {
            selectedNode.Deselect();
            selectedNode = null;
        }

        if (selectedComponent != null)
        {
            selectedComponent = null;
        }

        if (selectedWire != null)
        {
            selectedWire.Deselect();
            selectedWire = null;
        }
    }

    private void PlaceComponentAtNode(NodePoint node, GameObject prefab)
    {
        if (!isPlacingComponent)
        {
            firstNode = node;
            GameObject componentObj = Instantiate(prefab, node.transform.position, Quaternion.identity);
            componentBeingPlaced = componentObj.GetComponent<CircuitComponent1>();

            if (componentBeingPlaced != null)
            {
                componentBeingPlaced.ConnectToNode(node, true);
                isPlacingComponent = true;
            }
        }
        else
        {
            if (componentBeingPlaced != null)
            {
                componentBeingPlaced.ConnectToNode(node, false);
                isPlacingComponent = false;
                componentBeingPlaced = null;
                firstNode = null;
            }
        }
    }

    private void PlaceWireAtNode(NodePoint node)
    {
        if (wireBeingPlaced == null)
        {
            GameObject wireObj = Instantiate(jumperWirePrefab);
            wireBeingPlaced = wireObj.GetComponent<JumperWire>();

            if (wireBeingPlaced != null)
            {
                wireBeingPlaced.StartPlacement(node);
            }
        }
        else
        {
            bool success = wireBeingPlaced.CompletePlacement(node);

            if (success)
            {
                wireBeingPlaced = null;
            }
        }
    }

    private void RemoveComponent(CircuitComponent1 component)
    {
        if (component.nodeA != null)
            component.nodeA.DisconnectComponent(component);

        if (component.nodeB != null)
            component.nodeB.DisconnectComponent(component);

        Destroy(component.gameObject);
    }

    private void CancelCurrentAction()
    {
        if (isPlacingComponent && componentBeingPlaced != null)
        {
            RemoveComponent(componentBeingPlaced);
            isPlacingComponent = false;
            componentBeingPlaced = null;
            firstNode = null;
        }

        if (wireBeingPlaced != null)
        {
            wireBeingPlaced.CancelPlacement();
            wireBeingPlaced = null;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }
}