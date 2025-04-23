using SpiceSharp.Simulations;
using TMPro;
using UnityEngine;

public abstract class CircuitComponent1 : MonoBehaviour
{
    [Header("Component Settings")]
    public string componentName;
    public NodePoint nodeA;
    public NodePoint nodeB;

    [Header("Visual Elements")]
    [SerializeField] protected TextMeshPro valueLabel;
    [SerializeField] protected float labelColliderDepth = 0.1f;

    [Header("Connection Points")]
    [SerializeField] protected GameObject wireConnectionPointA;
    [SerializeField] protected GameObject wireConnectionPointB;

    protected CircuitBoard circuitBoard;
    protected LabelClickHandler clickHandler;
    protected static ComponentEditorUI editorUI;
    protected bool isInitialized;

    #region Base Implementation
    protected virtual void Start()
    {
        InitializeComponent();
        circuitBoard = FindObjectOfType<CircuitBoard>();
    }

    protected virtual void Awake()
    {
        componentName = GenerateDefaultName();
    }

    private void InitializeComponent()
    {
        if (isInitialized) return;

        InitializeLabelInteraction();
        FindEditorUI();
        isInitialized = true;
    }

    private void InitializeLabelInteraction()
    {
        if (valueLabel != null)
        {
            CreateLabelCollider();
            AddClickHandler();
        }
    }

    private void CreateLabelCollider()
    {
        if (valueLabel.GetComponent<BoxCollider>() == null)
        {
            Vector2 textSize = valueLabel.GetPreferredValues();
            var collider = valueLabel.gameObject.AddComponent<BoxCollider>();
            collider.size = new Vector3(textSize.x, textSize.y, labelColliderDepth);
            collider.center = Vector3.zero;
        }
    }

    private void AddClickHandler()
    {
        clickHandler = valueLabel.gameObject.GetComponent<LabelClickHandler>() ??
                      valueLabel.gameObject.AddComponent<LabelClickHandler>();
        clickHandler.OnClick += OnLabelClicked;
    }

    private void FindEditorUI()
    {
        if (editorUI == null)
        {
            editorUI = FindObjectOfType<ComponentEditorUI>();
        }
    }

    protected virtual string GenerateDefaultName()
    {
        return $"{GetType().Name[0]}_{GetInstanceID()}";
    }
    #endregion

    #region Abstract Methods
    public abstract void PrepareForSimulation();
    protected abstract void OnLabelClicked();
    public abstract void ConnectToNode(NodePoint node, bool isNodeA);
    protected abstract void PositionAtNode(NodePoint node, bool isNodeA);
    #endregion

    #region Common Functionality
    public GameObject GetConnectionPoint(bool isNodeA)
    {
        return isNodeA ? wireConnectionPointA : wireConnectionPointB;
    }

    protected virtual void UpdateValueDisplay()
    {
        // Base implementation can be overridden
    }

    protected void HandleNodeConnection(NodePoint node, bool isNodeA)
    {
        if (isNodeA)
        {
            if (nodeA != null) nodeA.DisconnectComponent(this);
            nodeA = node;
        }
        else
        {
            if (nodeB != null) nodeB.DisconnectComponent(this);
            nodeB = node;
        }

        if (node != null)
        {
            node.ConnectComponent(this);
            PositionAtNode(node, isNodeA);
        }

        UpdateValueDisplay();
    }
    #endregion

    #region Simulation Helpers
    protected string GetSimulationNodeName(NodePoint node)
    {
        if (node == null) return "invalid_node";
        return string.IsNullOrEmpty(node.nodeName)
            ? circuitBoard.RegisterNamedNode(node)
            : node.nodeName;
    }

    protected bool ValidateNodes()
    {
        if (nodeA == null || nodeB == null)
        {
            Debug.LogError($"{name} has unconnected nodes!");
            return false;
        }

        if (nodeA == nodeB)
        {
            Debug.LogError($"{name} has identical nodes!");
            return false;
        }

        return true;
    }
    #endregion
}
