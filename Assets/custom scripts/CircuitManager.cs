using UnityEngine;

public class CircuitManager : MonoBehaviour
{
    public WireConnection[] breakerToContactorSockets; // Sockets on the breaker
    public WireConnection[] relayToMotorSockets;      // Sockets on the relay
    public RelaySnapPoint relaySnapPoint;             // The snap point for the overload relay
    public BreakerSystem breakerSystem;               // Reference to the breaker system

    public Mutor motor; // Reference to the motor script

    public void CheckCircuit()
    {
        // 1. Check if the breaker is ON
       /* if (breakerSystem != null && !breakerSystem.isBreakerOn)
        {
            Debug.Log("Breaker is OFF. Circuit is incomplete.");
            StopMotor();
            return; // Exit if the breaker is OFF
        }
       */
        // 2. Check breaker-to-contactor sockets
        foreach (WireConnection socket in breakerToContactorSockets)
        {
            if (!socket.isConnected)
            {
                Debug.Log("Breaker to contactor sockets are not fully connected.");
                StopMotor();
                return; // Exit if any socket is disconnected
            }
        }

        // 3. Check if the overload relay is snapped to the contactor
        if (relaySnapPoint != null && !relaySnapPoint.isRelayConnected&& breakerSystem != null && !breakerSystem.isBreakerOn)
        {
            Debug.Log("Overload relay is not snapped to the contactor.");
            StopMotor();
            return; // Exit if the relay is not snapped
        }

        // 4. Check relay-to-motor sockets
        foreach (WireConnection socket in relayToMotorSockets)
        {
            if (!socket.isConnected)
            {
                Debug.Log("Relay to motor sockets are not fully connected.");
                StopMotor();
                return; // Exit if any socket is disconnected
            }
        }

        // If all checks pass, start the motor
        Debug.Log("All connections are complete. Starting the motor.");
        StartMotor();
    }

    private void StartMotor()
    {
        if (!motor.isRunning)
        {
            Debug.Log("Motor started.");
            motor.StartMotor();
        }
    }

    private void StopMotor()
    {
        if (motor.isRunning)
        {
            Debug.Log("Motor stopped.");
            motor.StopMotor();
        }
    }
}
