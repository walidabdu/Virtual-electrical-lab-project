using UnityEngine;

public class ToggleAnimationWithSound : MonoBehaviour
{
    private toggleAnimation toggleAnimation; // Reference to the ToggleAnimation component
    private AudioSource audioSource;        // Reference to the AudioSource component
    private bool hasTriggered = false;      // Ensures the sound and animation play only once per trigger

    private void Awake()
    {
        // Get the ToggleAnimation component attached to this GameObject
        toggleAnimation = GetComponent<toggleAnimation>();
        if (toggleAnimation == null)
        {
            Debug.LogError("No ToggleAnimation component found on this GameObject!");
        }

        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on this GameObject!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the tag "Player"
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // Ensure this only triggers once
            Debug.Log("Player entered trigger! Playing toggle animation and sound.");

            // Play the toggle animation
            if (toggleAnimation != null)
            {
                toggleAnimation.Playtoggle();
            }

            // Play the sound
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
}
