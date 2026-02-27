using System.Collections;
using UnityEngine;
using TMPro; // Added this to control the words!

public class TapController : MonoBehaviour
{
    public Animator animator;
    public TextMeshProUGUI statusText; // Changed from GameObject to TextMeshProUGUI

    public bool isLeaking = false;
    public float leakDuration = 20f;
    public float hurryUpTime = 5f; // When to show the warning

    private Coroutine leakCoroutine;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        // Hide text at start
        if (statusText != null) statusText.gameObject.SetActive(false);
    }

    public void StartLeak()
    {
        if (isLeaking) return;

        isLeaking = true;
        animator.SetBool("IsLeaking", true);
        
        if (statusText != null)
        {
            statusText.gameObject.SetActive(true);
            statusText.text = "Fix Me!"; // Initial message
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

            // Change message if time is running low
            if (currentTime <= hurryUpTime && isLeaking)
            {
                statusText.text = "Hurry up! Water is decreasing!";
                statusText.color = Color.red;
            }
        }

        // If the loop finishes and it's still leaking, they failed
        if (isLeaking)
        {
            statusText.text = "Too late! Tap is broken.";
            yield return new WaitForSeconds(2f);
            StopLeak();
        }
    }

    public void FixTap()
    {
        if (!isLeaking) return;

        isLeaking = false;

        if (leakCoroutine != null)
            StopCoroutine(leakCoroutine);

        animator.SetBool("IsLeaking", false);
        
        // Success Message
        if (statusText != null)
        {
            statusText.text = "Nice Job!";
            statusText.color = Color.green;
            StartCoroutine(HideTextAfterDelay(2f)); // Hide it after 2 seconds
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.TapFixed();
        }
    }

    IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        statusText.gameObject.SetActive(false);
    }

    public void StopLeak()
    {
        isLeaking = false;
        animator.SetBool("IsLeaking", false);
        if (statusText != null) statusText.gameObject.SetActive(false);
    }
}