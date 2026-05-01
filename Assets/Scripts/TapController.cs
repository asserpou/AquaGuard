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
    
    // متغير جديد عشان نعرف اللاعب قريب ولا لأ
    private bool isPlayerNear = false; 

    void Start()
    {
        animator = GetComponent<Animator>();
        // إخفاء كل شيء عند البداية
        HideAllUI();
    }

    void Update()
    {
        // لو اللاعب قريب، والحنفية بتسرب، وداس حرف الـ E
        if (isPlayerNear && isLeaking && Input.GetKeyDown(KeyCode.E))
        {
            FixTap();
        }
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

        // لو اللاعب كان واقف أصلاً جنب الحنفية وهي بدأت تسرب، نظهرله اللوحة فوراً
        if (isPlayerNear && InteractionPrompt.Instance != null)
        {
            InteractionPrompt.Instance.ShowPrompt("Fix Tap");
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

        // أول ما تتصلح، نخفي رسالة (Press E) فوراً
        if (InteractionPrompt.Instance != null) 
        {
            InteractionPrompt.Instance.HidePrompt();
        }

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

        // لو الوقت خلص والماية وقفت لوحدها، نخفي الرسالة برضو
        if (InteractionPrompt.Instance != null) 
        {
            InteractionPrompt.Instance.HidePrompt();
        }
    }

    private void HideAllUI()
    {
        if (leakCanvas != null) leakCanvas.SetActive(false);
        if (statusText != null) statusText.gameObject.SetActive(false);
    }

    // ==========================================
    // دوال اكتشاف اللاعب (جديد)
    // ==========================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;

            // نظهر اللوحة بس لو الحنفية بايظة
            if (isLeaking && InteractionPrompt.Instance != null)
            {
                InteractionPrompt.Instance.ShowPrompt("Fix Tap");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;

            // لما اللاعب يبعد، نخفي اللوحة
            if (InteractionPrompt.Instance != null)
            {
                InteractionPrompt.Instance.HidePrompt();
            }
        }
    }
}