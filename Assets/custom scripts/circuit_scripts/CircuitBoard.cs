using UnityEngine;
using System.Collections.Generic;

public class CircuitBoard : MonoBehaviour
{
    [SerializeField] private GameObject nodePointPrefab;
    [SerializeField] private int rows = 10;
    [SerializeField] private int columns = 15;
    [SerializeField] private float spacing = 0.5f;

    private NodePoint[,] nodePoints;

    public Dictionary<string, NodePoint> namedNodes = new Dictionary<string, NodePoint>();

    void Start()
    {
        CreateBoard();
    }

    void CreateBoard()
    {
        nodePoints = new NodePoint[rows, columns];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Vector3 position = new Vector3(c * spacing, 0, r * spacing);
                GameObject nodeObj = Instantiate(nodePointPrefab, position, Quaternion.identity, transform);
                nodeObj.name = $"Node_{r}_{c}";

                NodePoint nodePoint = nodeObj.GetComponent<NodePoint>();
                nodePoint.Initialize(r, c);
                nodePoints[r, c] = nodePoint;

                // Add some predefined named nodes (like ground)
                if (r == 0 && c == 0)
                {
                    nodePoint.SetAsGround();
                    namedNodes.Add("0", nodePoint); // SPICE ground node
                }
            }
        }
    }

    public NodePoint GetNodePoint(int row, int col)
    {
        if (row >= 0 && row < rows && col >= 0 && col < columns)
            return nodePoints[row, col];
        return null;
    }

    public string RegisterNamedNode(NodePoint node)
    {
        // Check if this node already has a name
        if (!string.IsNullOrEmpty(node.nodeName))
            return node.nodeName;

        // Check if this node is already registered with a different name
        foreach (var entry in namedNodes)
        {
            if (entry.Value == node)
                return entry.Key;
        }

        // Generate a name for the node if it doesn't have one
        if (node.IsGround())
            return "0";

        string nodeName = $"node_{namedNodes.Count + 1}";
        namedNodes.Add(nodeName, node);
        node.nodeName = nodeName;
        return nodeName;
    }
}