using System.Collections.Generic;
using UnityEngine;

public class CircuitManager2 : MonoBehaviour
{
    public BreakerSystem2 breakerSystem2;
   // public ToggleBoolOnButtonPress2 pushButton2;
    public RelaySnapPoint2 relaySnapPoint21;
    public RelaySnapPoint2 relaySnapPoint22;
    public Mutor2 motor2;
    public WireStateChecker2 wireStateChecker2;

    // We don't need a separate flag like 'isCircuitPoweredAndMotorBaselineSet' anymore,
    // we'll rely on wireStateChecker.IsMotorBaselineEstablished()

    public void CheckCircuit()
{
    string framePrefix = $"[{Time.frameCount}]"; // For clearer logs
    Debug.Log($"{framePrefix} ===== CircuitManager2.CheckCircuit() Called =====");

    if (motor2 == null || wireStateChecker2 == null)
    {
        Debug.LogError($"{framePrefix} CircuitManager2: Motor or WireStateChecker not assigned!");
        return;
    }

    // 1. Always capture the current physical state of phase wires
    wireStateChecker2.CaptureCurrentPhysicalPhaseConnections();

    // 2. Check if the OVERALL circuit is complete using your existing logic
    bool isOverallCircuitComplete = IsCircuitComplete();

    if (isOverallCircuitComplete)
    {
        Debug.Log($"{framePrefix} CircuitManager2: Overall circuit is COMPLETE.");

        // 3. If no motor direction baseline has EVER been established
        if (!wireStateChecker2.IsMotorBaselineEstablished())
        {
            Debug.Log($"{framePrefix} CircuitManager2: Motor direction baseline NOT YET ESTABLISHED. Starting motor forward and setting initial baseline.");
            motor2.Forward(); // Start in default direction
            wireStateChecker2.SetBaselineForMotorDirection();
        }
        // 4. A motor direction baseline EXISTS
        else
        {
            if (wireStateChecker2.HasPhaseConfigChangedFromBaseline())
            {
                Debug.Log($"{framePrefix} CircuitManager2: Phase configuration CHANGED from established baseline. Toggling motor direction.");
                
                // Toggle direction based on current state
                if (motor2.currentState == Mutor2.MotorState.Forward)
                {
                    motor2.Reverse();
                }
                else if (motor2.currentState == Mutor2.MotorState.Reverse)
                {
                    motor2.Forward();
                }
                else // If stopped, start in forward
                {
                    motor2.Forward();
                }
                
                // The new configuration becomes the current baseline
                wireStateChecker2.SetBaselineForMotorDirection();
            }
            else
            {
                Debug.Log($"{framePrefix} CircuitManager2: Phase configuration MATCHES established baseline. Ensuring motor is running in correct direction.");
                
                // Maintain current direction if already running
                if (motor2.currentState == Mutor2.MotorState.Stopped)
                {
                    // Restart in last known direction (need to track this separately or use Forward as default)
                    motor2.Forward(); // Or use last known direction if you track it
                }
            }
        }
    }
    // 5. Overall circuit is NOT complete
    else
    {
        Debug.Log($"{framePrefix} CircuitManager2: Overall circuit is NOT COMPLETE. Stopping motor.");
        motor2.Stop();
        // Note: Baseline is preserved across circuit breaks
    }
    
    Debug.Log($"{framePrefix} ===== CircuitManager2.CheckCircuit() Finished =====");
}
    /// <summary>
    /// Checks all conditions for the circuit to be operational.
    /// THIS METHOD REMAINS AS PER YOUR REQUIREMENT (UNCHANGED).
    /// </summary>
    public bool IsCircuitComplete()
    {
        // --- Your existing checks - DO NOT CHANGE ---
        if (breakerSystem2 == null || !breakerSystem2.isBreakerOn) { /* Debug.Log("Circuit incomplete: Breaker"); */ return false; }
        //if (pushButton2 == null || !pushButton2.isActive) { /* Debug.Log("Circuit incomplete: PushButton"); */ return false; }
        if ((relaySnapPoint21 == null || !relaySnapPoint21.isRelayConnected) | (relaySnapPoint22 == null || !relaySnapPoint22.isRelayConnected)) { /* Debug.Log("Circuit incomplete: Relay"); */ return false; }

        if (wireStateChecker2 == null) { Debug.LogError("CircuitManager2: WireStateChecker is null in IsCircuitComplete!"); return false; }
        if (!wireStateChecker2.AreControlWiresConnected()) { /* Debug.Log("Circuit incomplete: Control Wires"); */ return false; }

        if (wireStateChecker2.phaseSockets == null || wireStateChecker2.phaseSockets.Count == 0) {
             Debug.LogWarning("CircuitManager2: No phase sockets defined in WireStateChecker for IsCircuitComplete.");
            return false;
        }
        foreach (var socket in wireStateChecker2.phaseSockets)
        {
            if (socket == null || !socket.isConnected)
            {
                // Debug.Log($"Circuit incomplete: Phase Socket '{socket?.name ?? "NULL"}' not connected.");
                return false;
            }
        }
        // --- End of your existing checks ---
        return true;
    }

    public void StopMotor()
{
    if (motor2 != null) motor2.Stop();
}

public void StartMotorForward()
{
    if (motor2 != null && IsCircuitComplete())
    {
        motor2.Forward();
        wireStateChecker2.SetBaselineForMotorDirection();
    }
}

public void StartMotorReverse()
{
    if (motor2 != null && IsCircuitComplete())
    {
        motor2.Reverse();
        wireStateChecker2.SetBaselineForMotorDirection();
    }
}
}