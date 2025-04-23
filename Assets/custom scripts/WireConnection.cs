using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WireConnection : MonoBehaviour
{
    public bool isConnected = false; // Is a wire connected to this socket?
    public CircuitManager circuitManager; // Reference to the CircuitManager
    private XRSocketInteractor socketInteractor; // Reference to the XR Socket Interactor

    private void Awake()
    {
        // Get the XR Socket Interactor on this GameObject
        socketInteractor = GetComponent<XRSocketInteractor>();
        if (socketInteractor != null)
        {
            // Register XR interaction events
            socketInteractor.selectEntered.AddListener(OnWireConnected);
            socketInteractor.selectExited.AddListener(OnWireDisconnected);
        }
    }

    private void OnDestroy()
    {
        // Unregister events to avoid memory leaks
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.RemoveListener(OnWireConnected);
            socketInteractor.selectExited.RemoveListener(OnWireDisconnected);
        }
    }

    private void OnWireConnected(SelectEnterEventArgs args)
    {
        isConnected = true;
        Debug.Log($"Wire connected to socket: {name}");

        // Notify the CircuitManager
        if (circuitManager != null)
        {
            circuitManager.CheckCircuit();
        }
    }

    private void OnWireDisconnected(SelectExitEventArgs args)
    {
        isConnected = false;
        Debug.Log($"Wire disconnected from socket: {name}");

        // Notify the CircuitManager
        if (circuitManager != null)
        {
            circuitManager.CheckCircuit();
        }
    }
}
