using UnityEngine;



public class ToggleBoolOnButtonPress : MonoBehaviour {
    public CircuitManager circuitManager; // Reference to the CircuitManager
     public AudioSource buttonAudioSource;
    public AudioClip startSound;
    public AudioClip stopSound;
    [Tooltip("The initial state of the bool (default: false)")]
    public bool isActive = false;






    // Called when the XR button is pressed
    public void OnButtonPressed() {
        // Toggle the bool


        Debug.Log("push button is pressed");

        if (circuitManager.IsCircuitComplete()) {
            PlaySound(startSound);
            isActive = !isActive;
        }
        circuitManager.CheckCircuit();







    }
    private void PlaySound(AudioClip clip)
    {
        if (buttonAudioSource != null && clip != null)
        {
            buttonAudioSource.PlayOneShot(clip);
        }
    }
}