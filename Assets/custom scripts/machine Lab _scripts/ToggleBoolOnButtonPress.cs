using UnityEngine;



public class ToggleBoolOnButtonPress : MonoBehaviour
{
    [Tooltip("The initial state of the bool (default: false)")]
    public bool isActive = false;

    


   

    // Called when the XR button is pressed
    public void OnButtonPressed()
    {
        // Toggle the bool
        isActive = !isActive;
        

        
        

        
    }
}