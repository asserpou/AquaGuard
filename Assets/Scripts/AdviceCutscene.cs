using UnityEngine;
using System.Collections;
using TMPro;

public class AdviceCutscene : MonoBehaviour
{
    [Header("Characters")]
    public GameObject neighborWasting; 
    public GameObject neighborFixed;   

    [Header("Bubbles")]
    public GameObject aquaGuardBubble; 
    public TextMeshProUGUI aquaGuardText;
    public GameObject neighborBubble; 
    public TextMeshProUGUI neighborText;

    [Header("Feedback Panel (Right Choice)")]
    public GameObject feedbackPanel; 
    public TextMeshProUGUI feedbackText; 
    public TextMeshProUGUI scoreChangeText; 

    [Header("Reward Settings")]
    public float rewardAmount = 10f; 

    [Header("Dialogue Timing Settings")]
    public float typingSpeed = 0.03f; 
    public float stayOnScreenTime = 3f; 

    [Header("Camera Control")]
    public GameObject cutsceneCamera; 

    [Header("End Game (Good Win)")]
    public GameObject appreciationEffect; 
    public GameObject winPanel; // شاشة الفوز العادية
    public TextMeshProUGUI winScoreText; 

    public void PlayAdviceEffect()
    {
        StartCoroutine(ExecuteCutscene());
    }

    IEnumerator ExecuteCutscene()
    {
        if (cutsceneCamera != null) cutsceneCamera.SetActive(true);
        if (neighborWasting != null) neighborWasting.SetActive(false);
        if (neighborFixed != null) neighborFixed.SetActive(true);

        if (aquaGuardBubble != null) aquaGuardBubble.SetActive(false);
        if (neighborBubble != null) neighborBubble.SetActive(false);
        if (feedbackPanel != null) feedbackPanel.SetActive(false); 

        // حوار AquaGuard
        if (aquaGuardBubble != null && aquaGuardText != null)
        {
            yield return StartCoroutine(PopInPanel(aquaGuardBubble));
            yield return StartCoroutine(TypeText(aquaGuardText, "Excuse me! Leaving the hose running wastes a lot of clean water. We must conserve it!"));
            yield return new WaitForSecondsRealtime(stayOnScreenTime); 
            yield return StartCoroutine(PopOutPanel(aquaGuardBubble));
        }

        // حوار الجار
        if (neighborBubble != null && neighborText != null)
        {
            yield return StartCoroutine(PopInPanel(neighborBubble));
            yield return StartCoroutine(TypeText(neighborText, "You're right. I'm really sorry! I will turn it off now and promise it won't happen again."));
            yield return new WaitForSecondsRealtime(stayOnScreenTime); 
            yield return StartCoroutine(PopOutPanel(neighborBubble));
        }

        if (appreciationEffect != null) appreciationEffect.SetActive(true);
        
        // ظهور لوحة التوعية بأنيميشن
        if (feedbackPanel != null)
        {
            feedbackText.text = "Great job! Advising others helps save our precious water resources.";
            scoreChangeText.color = Color.green;
            scoreChangeText.text = $"+{rewardAmount} Water Saved!";
            
            yield return StartCoroutine(PopInPanel(feedbackPanel));
            yield return new WaitForSecondsRealtime(4f); 
            yield return StartCoroutine(PopOutPanel(feedbackPanel));
        }

        if (appreciationEffect != null) appreciationEffect.SetActive(false);

        // إضافة السكور و إظهار شاشة الفوز العادية
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddWaterEvent(rewardAmount); 
        }

        if (winPanel != null)
        {
            winPanel.SetActive(true);
            if (winScoreText != null && GameManager.Instance != null) 
            {
                winScoreText.text = Mathf.Round(GameManager.Instance.currentWater).ToString(); 
            }
            Time.timeScale = 0f; // نوقف اللعبة
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

    private IEnumerator PopInPanel(GameObject panel)
    {
        panel.SetActive(true);
        panel.transform.localScale = Vector3.zero;
        float elapsed = 0f;
        while (elapsed < 0.3f)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / 0.3f;
            float easeOut = 1f - Mathf.Pow(1f - t, 3f);
            panel.transform.localScale = new Vector3(easeOut, easeOut, easeOut);
            yield return null;
        }
        panel.transform.localScale = Vector3.one;
    }

    private IEnumerator PopOutPanel(GameObject panel)
    {
        float elapsed = 0f;
        Vector3 startScale = panel.transform.localScale;
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