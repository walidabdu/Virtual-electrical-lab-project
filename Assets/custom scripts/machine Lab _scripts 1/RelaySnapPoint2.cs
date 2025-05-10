using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RelaySnapPoint2 : MonoBehaviour
{
    public bool isRelayConnected = false; // Tracks if the overload relay is connected
    public CircuitManager2 circuitManager2; // Reference to the CircuitManager
    private XRSocketInteractor socketInteractor; // Reference to the XR Socket Interactor

    private void Awake()
    {
        // Get the XR Socket Interactor on this GameObject
        socketInteractor = GetComponent<XRSocketInteractor>();
        if (socketInteractor != null)
        {
            // Register XR interaction events
            socketInteractor.selectEntered.AddListener(OnRelayConnected);
            socketInteractor.selectExited.AddListener(OnRelayDisconnected);
        }
    }

    private void OnDestroy()
    {
        // Unregister events to avoid memory leaks
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.RemoveListener(OnRelayConnected);
            socketInteractor.selectExited.RemoveListener(OnRelayDisconnected);
        }
    }

    private void OnRelayConnected(SelectEnterEventArgs args)
    {
        isRelayConnected = true;
        Debug.Log("Overload relay connected to contactor.");

        // Notify the CircuitManager
        if (circuitManager2 != null)
        {
            circuitManager2.CheckCircuit();
        }
    }

    private void OnRelayDisconnected(SelectExitEventArgs args)
    {
        isRelayConnected = false;
        Debug.Log("Overload relay disconnected from contactor.");

        // Notify the CircuitManager
        if (circuitManager2 != null)
        {
            circuitManager2.CheckCircuit();
        }
    }
}
