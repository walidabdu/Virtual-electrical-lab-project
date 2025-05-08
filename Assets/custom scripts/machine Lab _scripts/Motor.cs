using UnityEngine;

public class Mutor : MonoBehaviour
{
    public bool isRunning = false;
    public float rotationSpeed = 100f;
    public AudioSource motorSound;
    public bool isForward = true;

    public void StartMotor()
    {
        isRunning = true;
        if (motorSound != null && !motorSound.isPlaying) motorSound.Play();
    }

    public void StopMotor()
    {
        isRunning = false;
        if (motorSound != null && motorSound.isPlaying) motorSound.Stop();
    }

    public void ToggleRotationDirection() => isForward = !isForward;

    private void Update()
    {
        if (isRunning)
        {
            float direction = isForward ? 1f : -1f;
            transform.Rotate(Vector3.forward * rotationSpeed * direction * Time.deltaTime);
        }
    }
}