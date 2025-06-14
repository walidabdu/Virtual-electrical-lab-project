using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MotorControlButton : MonoBehaviour
{
    public CircuitManager2 circuitManager2;
    public enum ButtonType { Stop, Forward, Reverse }
    public ButtonType buttonType = ButtonType.Stop;

    [Header("Sound Settings")]
    public AudioSource buttonAudioSource;
    public AudioClip startSound;
    public AudioClip stopSound;
    private bool canPlayStartSound = true;

    //private XRSimpleInteractable xrInteractable;

    void Awake()
    {
       // xrInteractable = GetComponent<XRSimpleInteractable>();
        //xrInteractable.selectEntered.AddListener(HandleButtonPress);
    }

    public void HandleButtonPress()
    {
        if (!circuitManager2.IsCircuitComplete())
        {
            Debug.Log("Circuit not complete - ignoring button press");
            return;
        }

        switch (buttonType)
        {
            case ButtonType.Stop:
                circuitManager2.StopMotor();
                PlaySound(stopSound);
                ResetStartSound();
                break;
                
            case ButtonType.Forward:
                if (canPlayStartSound)
                {
                    circuitManager2.StartMotorForward();
                    PlaySound(startSound);
                    canPlayStartSound = false;
                }
                break;
                
            case ButtonType.Reverse:
                if (canPlayStartSound)
                {
                    circuitManager2.StartMotorReverse();
                    PlaySound(startSound);
                    canPlayStartSound = false;
                }
                break;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (buttonAudioSource != null && clip != null)
        {
            buttonAudioSource.PlayOneShot(clip);
        }
    }

    private void ResetStartSound()
    {
        canPlayStartSound = true;
    }
}