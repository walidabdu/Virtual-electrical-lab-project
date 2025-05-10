using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WireStateChecker2 : MonoBehaviour
{
    public List<WireConnection2> phaseSockets;
    public List<WireConnection2> controlSockets;
    public List<WireConnection2> auxiliaryPhase1; // New list for first auxiliary phase set
    public List<WireConnection2> auxiliaryPhase2; // New list for second auxiliary phase set

    private Dictionary<WireConnection2, string> currentPhysicalPhaseConnections = new Dictionary<WireConnection2, string>();
    private Dictionary<WireConnection2, string> motorDirectionBaselinePhaseConnections = null;

    
    public bool AreControlWiresConnected()
    {
        if (controlSockets == null || controlSockets.Count == 0) return true;
        foreach (var socket in controlSockets)
        {
            if (socket == null || !socket.isConnected) return false;
        }
        return true;
    }

    // --- New Method: Checks if either auxiliary phase set is fully connected ---
    public bool AreAuxiliaryPhasesConnected()
    {
        // Check if either auxiliary phase set is fully connected
        bool isAuxiliary1Connected = CheckAuxiliaryPhaseSet(auxiliaryPhase1);
        bool isAuxiliary2Connected = CheckAuxiliaryPhaseSet(auxiliaryPhase2);
        
        return isAuxiliary1Connected || isAuxiliary2Connected;
    }

    // Helper method to check if an auxiliary phase set is fully connected
    private bool CheckAuxiliaryPhaseSet(List<WireConnection2> phaseSet)
    {
        if (phaseSet == null || phaseSet.Count == 0) 
            return false; // or true, depending on your requirements
        
        foreach (var socket in phaseSet)
        {
            if (socket == null || !socket.isConnected) 
                return false;
        }
        return true;
    }

    
    public void CaptureCurrentPhysicalPhaseConnections()
    {
        currentPhysicalPhaseConnections.Clear();
        if (phaseSockets == null) return;

        foreach (var socket in phaseSockets)
        {
            if (socket == null) continue;
            string wireName = socket.isConnected && socket.connectedWire != null ? socket.connectedWire.name : "DISCONNECTED";
            currentPhysicalPhaseConnections[socket] = wireName;
        }
    }

    public void SetBaselineForMotorDirection()
    {
        motorDirectionBaselinePhaseConnections = new Dictionary<WireConnection2, string>(currentPhysicalPhaseConnections);
        Debug.Log("[WireStateChecker] New baseline for motor direction set.");
    }

    public bool HasPhaseConfigChangedFromBaseline()
    {
        if (motorDirectionBaselinePhaseConnections == null)
        {
            return currentPhysicalPhaseConnections.Count > 0 && currentPhysicalPhaseConnections.Values.Any(s => s != "DISCONNECTED");
        }

        if (currentPhysicalPhaseConnections.Count != motorDirectionBaselinePhaseConnections.Count)
        {
            return true;
        }

        foreach (var currentEntry in currentPhysicalPhaseConnections)
        {
            if (!motorDirectionBaselinePhaseConnections.TryGetValue(currentEntry.Key, out string baselineWireName) ||
                baselineWireName != currentEntry.Value)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsMotorBaselineEstablished()
    {
        return motorDirectionBaselinePhaseConnections != null;
    }

    public void ClearMotorDirectionBaseline()
    {
        motorDirectionBaselinePhaseConnections = null;
        Debug.Log("[WireStateChecker] Motor direction baseline EXPLICITLY CLEARED.");
    }
}