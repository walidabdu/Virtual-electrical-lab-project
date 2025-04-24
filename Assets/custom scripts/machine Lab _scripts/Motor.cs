using UnityEngine;

public class Mutor : MonoBehaviour
{
    public bool isRunning = false; // Motor state
    public float rotationSpeed = 100f; // Speed of rotation
    public AudioSource motorSound; // Reference to the motor sound

    private void Update()
    {
        // Rotate the motor if it is running
        if (isRunning)
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
    }

    public void StartMotor()
    {
        isRunning = true;

        // Play sound if not already playing
        if (motorSound != null && !motorSound.isPlaying)
        {
            motorSound.Play();
        }
    }

    public void StopMotor()
    {
        isRunning = false;

        // Stop sound if playing
        if (motorSound != null && motorSound.isPlaying)
        {
            motorSound.Stop();
        }
    }
}
