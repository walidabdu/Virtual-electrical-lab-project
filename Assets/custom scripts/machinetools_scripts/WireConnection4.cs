using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WireConnection4 : MonoBehaviour
{
    public bool isConnected = false;
    public GameObject connectedWire; // The actual wire GameObject plugged in
    public CircuitManager circuitManager;
    private XRSocketInteractor socketInteractor;

    // Public property to get the socket's GameObject name
    public string SocketName => gameObject.name;

    // Public property to get the connected wire's name (or null if not connected)
    public string ConnectedWireName => isConnected ? connectedWire.name : null;

    private void Awake()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        socketInteractor.selectEntered.AddListener(OnWireConnected);
        socketInteractor.selectExited.AddListener(OnWireDisconnected);
    }

    private void OnWireConnected(SelectEnterEventArgs args)
    {
        isConnected = true;
        connectedWire = args.interactableObject.transform.gameObject;
        
        // Debug log to show both names when connected
        Debug.Log($"Wire connected: Socket '{SocketName}' received wire '{ConnectedWireName}'");
        
        if (circuitManager != null) circuitManager.CheckCircuit();
    }

    private void OnWireDisconnected(SelectExitEventArgs args)
    {
        // Debug log to show both names when disconnected
        if (isConnected)
        {
            Debug.Log($"Wire disconnected: Socket '{SocketName}' lost wire '{ConnectedWireName}'");
        }
        
        isConnected = false;
        connectedWire = null;
        if (circuitManager != null) circuitManager.CheckCircuit();
    }

    // Helper method to get connection info
    public string GetConnectionInfo()
    {
        return isConnected 
            ? $"Socket '{SocketName}' is connected to wire '{ConnectedWireName}'" 
            : $"Socket '{SocketName}' is not connected to any wire";
    }
}