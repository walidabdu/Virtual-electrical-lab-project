using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WireConnection4 : MonoBehaviour {
    public bool isConnected = false;
    public GameObject connectedWire;

    private XRSocketInteractor socketInteractor;

    public string SocketName => gameObject.name;
    public string ConnectedWireName => isConnected ? connectedWire.name.Split("_")[0] : null;

    private void Awake() {
        socketInteractor = GetComponent<XRSocketInteractor>();
        socketInteractor.selectEntered.AddListener(OnWireConnected);
        socketInteractor.selectExited.AddListener(OnWireDisconnected);
    }

    private void OnWireConnected(SelectEnterEventArgs args) {
        isConnected = true;
        connectedWire = args.interactableObject.transform.gameObject;
        CircuitManager3.ConnectWire(ConnectedWireName, SocketName);

        Debug.Log($"Wire connected: Socket '{SocketName}' received wire '{ConnectedWireName}'");
        Debug.Log($"wire connection called from : ${gameObject.name}");
    }

    private void OnWireDisconnected(SelectExitEventArgs args) {
        if (isConnected) {
            CircuitManager3.DisconnectWire(ConnectedWireName, SocketName);
            Debug.Log($"Wire disconnected: Socket '{SocketName}' lost wire '{ConnectedWireName}'");
        }

        isConnected = false;
        connectedWire = null;
    }
}