using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class XRStopButton : MonoBehaviour {
    public CircuitManager circuitManager;
     public AudioSource buttonAudioSource;
    public AudioClip startSound;
    public AudioClip stopSound;
     // private bool canPlayStartSound = true;

    public ToggleBoolOnButtonPress startButton; // Reference to your existing Start button

    private void Start() {
        // Subscribe to XR interaction events
        // GetComponent<XRSimpleInteractable>().selectEntered.AddListener(OnStopPressed);
    }



    public void OnButtonPressed() {
        if (circuitManager == null || startButton == null) {
            Debug.LogError("StopButton: Missing references!");
            return;
        }

        // Force the Start button to deactivate (breaking the circuit)
        
         // Immediately update circuit state
        if (circuitManager.IsCircuitComplete()) {
            PlaySound(startSound);
            startButton.isActive = false;
        }
        circuitManager.CheckCircuit();
        Debug.Log($"[{Time.frameCount}] StopButton: Circuit forced OPEN");
    }
     private void PlaySound(AudioClip clip)
    {
        if (buttonAudioSource != null && clip != null)
        {
            buttonAudioSource.PlayOneShot(clip);
        }
    }

   
}