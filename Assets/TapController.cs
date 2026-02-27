using System.Collections;
using UnityEngine;
using TMPro;

public class TapController : MonoBehaviour
{
    public Animator animator;
    public TMP_Text statusText;
    public GameObject leakCanvas;

    public bool isLeaking = false;
    public float leakDuration = 20f;
    public float hurryUpTime = 5f;

    private Coroutine leakCoroutine;

    void Start()
    {
        animator = GetComponent<Animator>();
        // إخفاء كل شيء عند البداية
        HideAllUI();
    }

    public void StartLeak()
    {
        if (isLeaking) return;

        isLeaking = true;
        animator.SetBool("IsLeaking", true);

        if (leakCanvas != null) leakCanvas.SetActive(true);
        if (statusText != null)
        {
            statusText.gameObject.SetActive(true);
            statusText.text = "Fix Me!";
            Color myColor;
            if (ColorUtility.TryParseHtmlString("#0D4558", out myColor))
            {
                statusText.color = myColor;
            }
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

            if (currentTime <= hurryUpTime && isLeaking)
            {
                if (statusText != null)
                {
                    statusText.text = "Hurry up!";
                    statusText.color = Color.red;
                }
            }
        }

        if (isLeaking)
        {
            if (statusText != null)
            {
                statusText.text = "Too Late!";
                statusText.color = Color.red;
            }
            yield return new WaitForSeconds(2f);
            StopLeak();
        }
    }

    public void FixTap()
    {
        if (!isLeaking) return;
        isLeaking = false;

        if (leakCoroutine != null) StopCoroutine(leakCoroutine);
        animator.SetBool("IsLeaking", false);

        if (statusText != null)
        {
            statusText.text = "Nice Job!";
            statusText.color = Color.green;
            StartCoroutine(HideUIAfterDelay(1.5f));
        }

        if (GameManager.Instance != null) GameManager.Instance.TapFixed();
    }

    IEnumerator HideUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideAllUI(); // استدعاء وظيفة الإخفاء الشاملة
    }

    public void StopLeak()
    {
        isLeaking = false;
        animator.SetBool("IsLeaking", false);
        HideAllUI(); // استدعاء وظيفة الإخفاء الشاملة
    }

    // وظيفة مساعدة للتأكد من إخفاء الكانفاس والتيكست معاً
    private void HideAllUI()
    {
        if (leakCanvas != null) leakCanvas.SetActive(false);
        if (statusText != null) statusText.gameObject.SetActive(false);
    }
}