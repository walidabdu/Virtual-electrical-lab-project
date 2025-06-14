using UnityEngine;

public class CircuitManager : MonoBehaviour
{
    public BreakerSystem breakerSystem;
     public XRStopButton stopButton;
    public ToggleBoolOnButtonPress pushButton;
    public RelaySnapPoint relaySnapPoint;
    public Mutor motor;
    public WireStateChecker wireStateChecker;
    

    // We don't need a separate flag like 'isCircuitPoweredAndMotorBaselineSet' anymore,
    // we'll rely on wireStateChecker.IsMotorBaselineEstablished()

    public void CheckCircuit() {
        string framePrefix = $"[{Time.frameCount}]"; // For clearer logs
        Debug.Log($"{framePrefix} ===== CircuitManager.CheckCircuit() Called =====");

        if (motor == null || wireStateChecker == null) {
            Debug.LogError($"{framePrefix} CircuitManager: Motor or WireStateChecker not assigned!");
            return;
        }

        // 1. Always capture the current physical state of phase wires.
        wireStateChecker.CaptureCurrentPhysicalPhaseConnections();

        // 2. Check if the OVERALL circuit is complete using your existing logic.
        bool isOverallCircuitComplete = IsCircuitComplete() && pushButton.isActive; // This is your unchanged method

        if (isOverallCircuitComplete) {
            Debug.Log($"{framePrefix} CircuitManager: Overall circuit is COMPLETE.");

            // 3. If no motor direction baseline has EVER been established
            if (!wireStateChecker.IsMotorBaselineEstablished()) {
                Debug.Log($"{framePrefix} CircuitManager: Motor direction baseline NOT YET ESTABLISHED. Starting motor and setting initial baseline.");
                motor.StartMotor(); // Start in default direction
                wireStateChecker.SetBaselineForMotorDirection();
            }
            // 4. A motor direction baseline EXISTS. Check if the current phase config matches it.
            else {
                if (wireStateChecker.HasPhaseConfigChangedFromBaseline()) {
                    Debug.Log($"{framePrefix} CircuitManager: Phase configuration CHANGED from established baseline. Toggling motor direction.");
                    motor.StartMotor(); // Ensure motor is running before toggle (if it somehow stopped)
                    motor.ToggleRotationDirection();
                    // The new configuration becomes the current baseline for this motor direction
                    wireStateChecker.SetBaselineForMotorDirection();
                } else {
                    Debug.Log($"{framePrefix} CircuitManager: Phase configuration MATCHES established baseline. Ensuring motor is running in correct direction.");
                    // If the circuit just re-completed with the SAME phase config, motor should start/continue
                    motor.StartMotor();
                }
            }
        }
        // 5. Overall circuit is NOT complete
        else {
            Debug.Log($"{framePrefix} CircuitManager: Overall circuit is NOT COMPLETE. Stopping motor.");
            motor.StopMotor();
            // CRITICAL: We are NOT calling wireStateChecker.ClearMotorDirectionBaseline() here.
            // This means the baseline is REMEMBERED across circuit breaks.
            // It will only be updated if a new, different configuration is detected when the circuit is next complete.
        }
        Debug.Log($"{framePrefix} ===== CircuitManager.CheckCircuit() Finished =====");
    }

    /// <summary>
    /// Checks all conditions for the circuit to be operational.
    /// THIS METHOD REMAINS AS PER YOUR REQUIREMENT (UNCHANGED).
    /// </summary>
    public bool IsCircuitComplete()
    {


         // --- NEW: Stop Button Check ---
    //  if (stopButton != null && !pushButton.isActive)
    //     {
    //         return false; // Stop button forces Start button inactive
    //     }


        // --- Your existing checks - DO NOT CHANGE ---
        if (breakerSystem == null || !breakerSystem.isBreakerOn) { /* Debug.Log("Circuit incomplete: Breaker"); */ return false; }
        // if (pushButton == null || !pushButton.is Active) { /* Debug.Log("Circuit incomplete: PushButton"); */ return false; }
        if (relaySnapPoint == null || !relaySnapPoint.isRelayConnected) { /* Debug.Log("Circuit incomplete: Relay"); */ return false; }

        if (wireStateChecker == null) { Debug.LogError("CircuitManager: WireStateChecker is null in IsCircuitComplete!"); return false; }
        if (!wireStateChecker.AreControlWiresConnected()) { /* Debug.Log("Circuit incomplete: Control Wires"); */ return false; }

        if (wireStateChecker.phaseSockets == null || wireStateChecker.phaseSockets.Count == 0) {
             Debug.LogWarning("CircuitManager: No phase sockets defined in WireStateChecker for IsCircuitComplete.");
            return false;
        }
        foreach (var socket in wireStateChecker.phaseSockets)
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
}