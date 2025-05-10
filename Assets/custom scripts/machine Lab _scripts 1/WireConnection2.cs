using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WireConnection2 : MonoBehaviour
{
    public bool isConnected = false;
    public GameObject connectedWire; // The actual wire GameObject plugged in
    public CircuitManager2 circuitManager2;
    private XRSocketInteractor socketInteractor;

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
        if (circuitManager2 != null) circuitManager2.CheckCircuit();
    }

    private void OnWireDisconnected(SelectExitEventArgs args)
    {
        isConnected = false;
        connectedWire = null;
        if (circuitManager2 != null) circuitManager2.CheckCircuit();
    }
}