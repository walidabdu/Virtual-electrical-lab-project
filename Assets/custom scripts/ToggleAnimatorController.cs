using System.Collections.Generic;
using UnityEngine;

public class ToggleAnimatorController : MonoBehaviour
{
    // List of GameObjects with Animator components
    [SerializeField] private List<GameObject> gameObjectsWithAnimators;

    // List of GameObjects to toggle active/inactive
    [SerializeField] private List<GameObject> gameObjectsToToggle;

    // Boolean to track animation direction
    private bool isPlayingForward = true;

    // Function to toggle animations and set GameObjects active/inactive
    public void ToggleAnimations()
    {
        // Play animations forward or backward
        foreach (GameObject obj in gameObjectsWithAnimators)
        {
            if (obj.TryGetComponent(out Animator animator))
            {
                // Enable the Animator if disabled
                if (!animator.enabled)
                    animator.enabled = true;

                // Play the animation based on direction
                if (isPlayingForward)
                {
                    animator.SetFloat("SpeedMultiplier", 1f); // Forward speed
                    animator.Play("AnimationName", 0, 0f); // Start at the beginning
                }
                else
                {
                    animator.SetFloat("SpeedMultiplier", -1f); // Reverse speed
                    animator.Play("AnimationName", 0, 1f); // Start at the end
                }
            }
        }

        // Toggle the active state of the second list of GameObjects
        foreach (GameObject toggleObject in gameObjectsToToggle)
        {
            if (toggleObject != null)
            {
                toggleObject.SetActive(isPlayingForward); // Active when forward, inactive when backward
            }
        }

        // Toggle direction state
        isPlayingForward = !isPlayingForward;
    }
}
