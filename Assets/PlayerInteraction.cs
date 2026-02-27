using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 1.5f; // Range to check for taps
    public LayerMask tapLayer; // To ensure we only check for taps

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        // 1. Create a circle around the player to check for colliders (2D Logic)
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactionRange, tapLayer);

        if (hit != null)
        {
            // 2. Check if the object has the TapController
            TapController tap = hit.GetComponent<TapController>();

            if (tap != null)
            {
                // The FixTap function inside TapController handles the logic
                // of whether it *can* be fixed or not.
                tap.FixTap(); 
            }
        }
    }

    // This draws a circle in the editor so you can see the range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}