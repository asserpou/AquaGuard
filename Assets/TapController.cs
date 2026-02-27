using System.Collections;
using UnityEngine;
using TMPro;

public class TapController : MonoBehaviour
{
    public Animator animator;
    public TMP_Text statusText;

    public bool isLeaking = false;
    public float leakDuration = 20f; // The "Cooldown"
    public float hurryUpTime = 5f;   // Last 5 seconds

    private Coroutine leakCoroutine;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        // Ensure text is invisible at the start
        if (statusText != null) statusText.gameObject.SetActive(false);
    }

    public void StartLeak()
    {
        // If already leaking, don't do anything
        if (isLeaking) return;

        isLeaking = true;
        animator.SetBool("IsLeaking", true);
        
        // Show text ONLY when leak starts
        if (statusText != null)
        {
            statusText.gameObject.SetActive(true);
            statusText.text = "Fix Me!";
            statusText.color = Color.white;
        }

        leakCoroutine = StartCoroutine(LeakTimer());
    }

    IEnumerator LeakTimer()
    {
        float currentTime = leakDuration;

        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentTime--;

            // Logic: If user fixed it, this coroutine is stopped, so this code won't run.
            // If we are here, it is still leaking.

            // WARNING ALERT (Last 5 Seconds)
            if (currentTime <= hurryUpTime)
            {
                if (statusText != null)
                {
                    statusText.text = "Hurry up! Water is decreasing!";
                    statusText.color = Color.red; // Visual urgency
                }
            }
        }

        // --- TIME IS UP (COOLDOWN ENDED) ---
        // Player failed to fix it in time.
        if (isLeaking)
        {
            if (statusText != null)
            {
                statusText.text = "Better watch out water is important";
                statusText.color = Color.red;
            }

            // Keep the text for 2 seconds so the player sees they failed
            yield return new WaitForSeconds(2f);

            // Reset the tap so it stops leaking and waits for GameManager to pick it again
            StopLeak();
        }
    }

    public void FixTap()
    {
        // If the tap isn't leaking (or the time ran out and we called StopLeak),
        // this returns immediately, making it "not interactive."
        if (!isLeaking) return;

        isLeaking = false;

        // Stop the timer so the "Time Up" logic doesn't happen
        if (leakCoroutine != null) StopCoroutine(leakCoroutine);

        animator.SetBool("IsLeaking", false);
        
        // Success Message
        if (statusText != null)
        {
            statusText.text = "Nice Job!";
            statusText.color = Color.green;
            StartCoroutine(HideTextAfterDelay(1.5f));
        }

        // Notify Manager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TapFixed();
        }
    }

    IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (statusText != null) statusText.gameObject.SetActive(false);
    }

    public void StopLeak()
    {
        isLeaking = false;
        animator.SetBool("IsLeaking", false);
        // Hide text immediately when reset/failed
        if (statusText != null) statusText.gameObject.SetActive(false);
    }
}