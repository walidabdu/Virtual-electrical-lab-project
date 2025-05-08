using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WireStateChecker : MonoBehaviour
{
    public List<WireConnection> phaseSockets;
    public List<WireConnection> controlSockets;

    private Dictionary<WireConnection, string> currentPhysicalPhaseConnections = new Dictionary<WireConnection, string>();
    // This baseline will now persist across circuit breaks until a new configuration is actively set.
    private Dictionary<WireConnection, string> motorDirectionBaselinePhaseConnections = null;

    // --- Methods for IsCircuitComplete (as per your original structure) ---
    public bool AreControlWiresConnected()
    {
        if (controlSockets == null || controlSockets.Count == 0) return true;
        foreach (var socket in controlSockets)
        {
            if (socket == null || !socket.isConnected) return false;
        }
        return true;
    }

    // --- Methods for Phase Change Detection Logic ---
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
        motorDirectionBaselinePhaseConnections = new Dictionary<WireConnection, string>(currentPhysicalPhaseConnections);
        Debug.Log("[WireStateChecker] New baseline for motor direction set.");
    }

    public bool HasPhaseConfigChangedFromBaseline()
    {
        if (motorDirectionBaselinePhaseConnections == null)
        {
            // This should ideally not be the primary check for "is it new?"
            // CircuitManager will use IsMotorBaselineEstablished() for that.
            // If called when no baseline, means any connection is a "change" from "nothing".
            // For robustness, if baseline is null, any valid current connection implies it's "different".
            // However, CircuitManager's logic flow should prevent this from being the sole deciding factor for toggle.
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

    /// <summary>
    /// Checks if a baseline for motor direction has ever been established.
    /// </summary>
    public bool IsMotorBaselineEstablished()
    {
        return motorDirectionBaselinePhaseConnections != null;
    }

    /// <summary>
    /// Clears the motor direction baseline.
    /// Call this explicitly if you need a full reset (e.g., a level reset, not on simple circuit break).
    /// </summary>
    public void ClearMotorDirectionBaseline() // Renamed for clarity
    {
        motorDirectionBaselinePhaseConnections = null;
        Debug.Log("[WireStateChecker] Motor direction baseline EXPLICITLY CLEARED.");
    }
}