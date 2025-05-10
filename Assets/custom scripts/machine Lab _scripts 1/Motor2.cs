using UnityEngine;

public class Mutor2 : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public AudioSource motorSound;
    
    // Current motor state (automatically private)
    public enum MotorState { Stopped, Forward, Reverse }
    public MotorState currentState = MotorState.Stopped;

    // Public methods for motor control
    public void Stop()
    {
        currentState = MotorState.Stopped;
        if (motorSound != null && motorSound.isPlaying)
            motorSound.Stop();
        Debug.Log("Motor stopped");
    }

    public void Forward()
    {
        currentState = MotorState.Forward;
        if (motorSound != null && !motorSound.isPlaying)
            motorSound.Play();
        Debug.Log("Motor running forward");
    }

    public void Reverse()
    {
        currentState = MotorState.Reverse;
        if (motorSound != null && !motorSound.isPlaying)
            motorSound.Play();
        Debug.Log("Motor running in reverse");
    }

    private void Update()
    {
        switch (currentState)
        {
            case MotorState.Forward:
                transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
                break;
                
            case MotorState.Reverse:
                transform.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
                break;
                
            case MotorState.Stopped:
                // No rotation
                break;
        }
    }

    // Helper property to check if motor is running (optional)
    public bool IsRunning => currentState != MotorState.Stopped;
}