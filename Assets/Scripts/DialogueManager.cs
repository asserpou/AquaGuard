using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // ==========================================
    // 1. المتغيرات والخانات (Variables)
    // ==========================================

    [Header("Main Dialogue UI")]
    public GameObject dialoguePanel;
    public CanvasGroup mainPanelGroup; 
    public TextMeshProUGUI dialogueText; 
    
    [Header("Dialogue Settings")]
    [TextArea(2, 5)]
    public string textToType = "Your neighbor is wasting water. What should you do?"; 
    public float typeSpeed = 0.05f; 

    [Header("Extra UI to Fade (Optional)")]
    public CanvasGroup extraTextGroup; 
    public CanvasGroup extraPanelGroup; 

    [Header("Buttons Groups (Separate)")]
    public CanvasGroup saveButtonGroup; 
    public CanvasGroup leaveButtonGroup; 

    [Header("Other UI Elements")]
    public GameObject waterBarUI; 
    public GameObject timerUI; 

    [Header("Post-Dialogue Events")]
    public AdviceCutscene adviceCutscene; 
    public IgnoreCutscene ignoreCutscene; 
    

    // ==========================================
    // 2. الدوال الأساسية للتشغيل والقفل
    // ==========================================

    public void OpenDialogue()
    {
        if (timerUI != null) timerUI.SetActive(false);

        if (dialoguePanel != null) dialoguePanel.SetActive(true);

        // إخفاء وتصفير حجم كل العناصر قبل الأنيميشن
        HideGroup(saveButtonGroup);
        HideGroup(leaveButtonGroup);
        HideGroup(extraTextGroup);
        HideGroup(extraPanelGroup);

        StartCoroutine(DialogueSequence());
    }

    private void CloseDialogue() 
    { 
        if (dialoguePanel != null) dialoguePanel.SetActive(false); 
        if (waterBarUI != null) waterBarUI.SetActive(true); 
        if (timerUI != null) timerUI.SetActive(true);
        
        Time.timeScale = 1f; 
    }


    // ==========================================
    // 3. تسلسل الأحداث (الأنيميشن الجامد)
    // ==========================================

    private IEnumerator DialogueSequence()
    {
        // 1. دخول اللوحة الرئيسية بأنيميشن السوستة (Pop-in Bounce)
        if (mainPanelGroup != null)
        {
            yield return StartCoroutine(PopInElement(mainPanelGroup, 0.4f));
        }

        // 2. كتابة النص حرف بحرف
        if (dialogueText != null)
        {
            dialogueText.text = ""; 
            foreach (char c in textToType)
            {
                dialogueText.text += c;
                yield return new WaitForSecondsRealtime(typeSpeed);
            }
        }

        // 3. دخول الأجزاء الإضافية (إن وجدت)
        if (extraPanelGroup != null) 
        {
            StartCoroutine(PopInElement(extraPanelGroup, 0.3f));
        }
        if (extraTextGroup != null) 
        {
            yield return new WaitForSecondsRealtime(0.1f);
            yield return StartCoroutine(PopInElement(extraTextGroup, 0.3f));
        }

        // 4. دخول الزراير ورا بعض بشياكة
        if (saveButtonGroup != null) 
        {
            StartCoroutine(PopInElement(saveButtonGroup, 0.3f));
        }
        
        if (leaveButtonGroup != null) 
        {
            yield return new WaitForSecondsRealtime(0.15f); // تأخير بسيط عشان يدخلوا ورا بعض مش مع بعض
            yield return StartCoroutine(PopInElement(leaveButtonGroup, 0.3f));
        }
    }


    // ==========================================
    // 4. دالة الأنيميشن السحرية (Pop-In & Fade)
    // ==========================================

    private IEnumerator PopInElement(CanvasGroup cg, float duration)
    {
        if (cg == null) yield break; 
        
        Transform t = cg.transform;
        cg.alpha = 0f;
        t.localScale = Vector3.zero;
        cg.interactable = false;
        cg.blocksRaycasts = false;

        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float percent = elapsed / duration;

            // معادلة رياضية بتعمل تأثير السوستة (Ease Out Back)
            float c1 = 1.70158f;
            float c3 = c1 + 1f;
            float scale = 1f + c3 * Mathf.Pow(percent - 1f, 3f) + c1 * Mathf.Pow(percent - 1f, 2f);

            // حماية عشان الحجم ماينزلش بالسالب
            if(scale < 0) scale = 0;

            t.localScale = new Vector3(scale, scale, scale);
            cg.alpha = Mathf.Lerp(0f, 1f, percent * 1.5f); // الظهور بيكون أسرع شوية من الحجم
            
            yield return null;
        }
        
        t.localScale = Vector3.one;
        cg.alpha = 1f;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    private void HideGroup(CanvasGroup cg)
    {
        if (cg == null) return; 
        
        cg.alpha = 0f;
        cg.transform.localScale = Vector3.zero; // نخليه صفر عشان الأنيميشن يشتغل صح
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }


    // ==========================================
    // 5. دوال الزراير (للربط في الـ OnClick)
    // ==========================================

    public void ChooseGood() 
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (waterBarUI != null) waterBarUI.SetActive(true);

        // السطر ده اللي هيفك الـ Freeze ويخلي الكاميرا والأنيميشن يتحركوا
        Time.timeScale = 1f; 

        if (adviceCutscene != null)
        {
            adviceCutscene.PlayAdviceEffect();
        }
    }
    public void ChooseBad() 
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (waterBarUI != null) waterBarUI.SetActive(true);

        // السطر ده اللي هيفك الـ Freeze ويخلي الكاميرا والأنيميشن يتحركوا
        Time.timeScale = 1f; 

        if (ignoreCutscene != null)
        {
            ignoreCutscene.PlayIgnoreEffect();
        }
        else if (GameManager.Instance != null) 
        {
            GameManager.Instance.SubtractWaterEvent(20f);
        }
    }

    public void ChooseNeutral() 
    {
        CloseDialogue();
    }
}