using UnityEngine;
using System.Collections;
using TMPro;

public class IgnoreCutscene : MonoBehaviour
{
    [Header("Bubbles")]
    public GameObject aquaGuardBubble; 
    public TextMeshProUGUI aquaGuardText;
    public GameObject neighborBubble; 
    public TextMeshProUGUI neighborText;

    [Header("Feedback Panel (Wrong Choice)")]
    public GameObject feedbackPanel; 
    public TextMeshProUGUI feedbackText; 
    public TextMeshProUGUI penaltyText; 

    [Header("Penalty Settings")]
    public float penaltyAmount = 20f; 

    [Header("Dialogue Timing Settings")]
    public float typingSpeed = 0.03f; 
    public float stayOnScreenTime = 3f;

    [Header("Camera Control")]
    public GameObject cutsceneCamera; 

    [Header("End Game Panels")]
    public GameObject winbadPanel; // شاشة الفوز السيء
    public TextMeshProUGUI winbadScoreText; 
    public GameObject losePanel; // ===== السطر الجديد: شاشة الخسارة =====

    private Vector3 aquaBubbleOriginalScale;
    private Vector3 neighborBubbleOriginalScale;
    private Vector3 feedbackOriginalScale;

    void Start()
    {
        if (aquaGuardBubble != null) aquaBubbleOriginalScale = aquaGuardBubble.transform.localScale;
        if (neighborBubble != null) neighborBubbleOriginalScale = neighborBubble.transform.localScale;
        if (feedbackPanel != null) feedbackOriginalScale = feedbackPanel.transform.localScale;
    }

    public void PlayIgnoreEffect()
    {
        StartCoroutine(ExecuteCutscene());
    }

    IEnumerator ExecuteCutscene()
    {
        if (cutsceneCamera != null) cutsceneCamera.SetActive(true);

        if (aquaGuardBubble != null) aquaGuardBubble.SetActive(false);
        if (neighborBubble != null) neighborBubble.SetActive(false);
        if (feedbackPanel != null) feedbackPanel.SetActive(false); 

        // حوار AquaGuard
        if (aquaGuardBubble != null && aquaGuardText != null)
        {
            yield return StartCoroutine(PopInPanel(aquaGuardBubble, aquaBubbleOriginalScale));
            yield return StartCoroutine(TypeText(aquaGuardText, "Never mind... It's not my problem."));
            yield return new WaitForSecondsRealtime(stayOnScreenTime); 
            yield return StartCoroutine(PopOutPanel(aquaGuardBubble, aquaBubbleOriginalScale));
        }

        // رد الجار 
        if (neighborBubble != null && neighborText != null)
        {
            yield return StartCoroutine(PopInPanel(neighborBubble, neighborBubbleOriginalScale));
            yield return StartCoroutine(TypeText(neighborText, "Yeah, mind your own business!"));
            yield return new WaitForSecondsRealtime(stayOnScreenTime); 
            yield return StartCoroutine(PopOutPanel(neighborBubble, neighborBubbleOriginalScale));
        }

        if (cutsceneCamera != null) cutsceneCamera.SetActive(false);

        // ظهور اللوحة الحمراء
        if (feedbackPanel != null)
        {
            feedbackText.text = "Ignoring water waste is wrong! Every drop counts. By walking away, clean water is lost.";
            penaltyText.color = Color.red;
            penaltyText.text = $"-{penaltyAmount} Water Lost!";
            
            yield return StartCoroutine(PopInPanel(feedbackPanel, feedbackOriginalScale));
            yield return new WaitForSecondsRealtime(4f);
            yield return StartCoroutine(PopOutPanel(feedbackPanel, feedbackOriginalScale));
        }

        // خصم الماية
        if (GameManager.Instance != null) GameManager.Instance.SubtractWaterEvent(penaltyAmount);
        
        // ===== التعديل هنا: تحديد الخسارة أو الفوز السيء =====
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.currentWater > 0)
            {
                // لو لسه عايش (معاه ماية)
                if (winbadPanel != null)
                {
                    winbadPanel.SetActive(true); 
                    if (winbadScoreText != null) winbadScoreText.text = Mathf.Round(GameManager.Instance.currentWater).ToString();
                    Time.timeScale = 0f; 
                }
                else
                {
                    Time.timeScale = 1f; 
                }
            }
            else 
            {
                // لو الماية بقت صفر أو تحت الصفر (خسر اللعبة)
                if (losePanel != null)
                {
                    losePanel.SetActive(true);
                    Time.timeScale = 0f; // نوقف اللعبة
                }
            }
        }
    }

    private IEnumerator TypeText(TextMeshProUGUI textComponent, string fullText)
    {
        textComponent.text = ""; 
        foreach (char c in fullText)
        {
            textComponent.text += c; 
            yield return new WaitForSecondsRealtime(typingSpeed); 
        }
    }

    private IEnumerator PopInPanel(GameObject panel, Vector3 targetScale)
    {
        panel.SetActive(true);
        panel.transform.localScale = Vector3.zero;
        float elapsed = 0f;
        while (elapsed < 0.3f)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / 0.3f;
            float easeOut = 1f - Mathf.Pow(1f - t, 3f);
            panel.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, easeOut);
            yield return null;
        }
        panel.transform.localScale = targetScale;
    }

    private IEnumerator PopOutPanel(GameObject panel, Vector3 startScale)
    {
        float elapsed = 0f;
        while (elapsed < 0.2f)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / 0.2f;
            panel.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            yield return null;
        }
        panel.SetActive(false);
    }
}