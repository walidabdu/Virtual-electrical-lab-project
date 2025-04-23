using UnityEngine;

public class BreakerSystem : MonoBehaviour
{
    public bool isBreakerOn = false; // Tracks if the breaker is ON
    public CircuitManager circuitManager; // Reference to the CircuitManager

    public Transform leverTransform; // Reference to the lever's Transform
    public float offThresholdAngle = 45f;   // Lever is OFF when X angle >= 45
    public float onThresholdAngle = -45f;   // Lever is ON when X angle <= -45

    private void Update()
    {
        // Get the lever's current X-axis angle in local space
        float currentXAngle = leverTransform.localEulerAngles.x;
        


        // Normalize the angle to the range of -180 to 180 for easier comparison
        if (currentXAngle > 180f)
        {
            currentXAngle -= 360f;
        }

        // Check if the lever is in the ON state
        if (currentXAngle <= onThresholdAngle)
        {
            if (!isBreakerOn) // Only update if it wasn't already ON
            {
                isBreakerOn = true;
                Debug.Log("Breaker is ON");
                NotifyCircuitManager();
            }
        }
        // Check if the lever is in the OFF state
        else if (currentXAngle >= offThresholdAngle)
        {
            if (isBreakerOn) // Only update if it wasn't already OFF
            {
                isBreakerOn = false;
                Debug.Log("Breaker is OFF");
                NotifyCircuitManager();
            }
        }
    }

    private void NotifyCircuitManager()
    {
        // Notify the CircuitManager to recheck the circuit
        if (circuitManager != null)
        {
            circuitManager.CheckCircuit();
        }
        else
        {
            Debug.LogError("CircuitManager is not assigned in BreakerSystem!");
        }
    }
}
