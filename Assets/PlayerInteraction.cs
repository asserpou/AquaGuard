using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f; // How close you must be to the tap

    void Update()
    {
        // When the player presses 'E'
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckForTap();
        }
    }

    void CheckForTap()
    {
        RaycastHit hit;
        // Shoots a "laser" forward from the center of the screen
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance))
        {
            // Try to find the TapController on the object we hit
            TapController tap = hit.collider.GetComponent<TapController>();

            if (tap != null)
            {
                tap.FixTap(); // This triggers the "Nice Job!" message!
            }
        }
    }
}