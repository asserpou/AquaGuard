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

    [Header("Post-Dialogue Events")]
    public AdviceCutscene adviceCutscene; // اسحب سكريبت الـ Cutscene الجديد هنا


    // ==========================================
    // 2. الدوال الأساسية للتشغيل والقفل
    // ==========================================

    public void OpenDialogue()
    {
        // تشغيل الشاشة الرئيسية
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        // إخفاء كل العناصر يدوياً قبل بدأ الأنيميشن
        HideGroup(saveButtonGroup);
        HideGroup(leaveButtonGroup);
        HideGroup(extraTextGroup);
        HideGroup(extraPanelGroup);

        // تشغيل تسلسل الأحداث (الكتابة والظهور)
        StartCoroutine(DialogueSequence());
    }

    private void CloseDialogue() 
    { 
        // قفل الشاشة
        if (dialoguePanel != null) 
        {
            dialoguePanel.SetActive(false); 
        }
        
        // إرجاع عداد المياه
        if (waterBarUI != null) 
        {
            waterBarUI.SetActive(true); 
        }
        
        // إرجاع الوقت لطبيعته
        Time.timeScale = 1f; 
    }


    // ==========================================
    // 3. تسلسل الأحداث (الأنيميشن)
    // ==========================================

    private IEnumerator DialogueSequence()
    {
        // الخطوة الأولى: إظهار الشاشة الرئيسية ببطء
        if (mainPanelGroup != null)
        {
            yield return StartCoroutine(FadeCanvasGroup(mainPanelGroup, 1f, 0.5f));
        }

        // الخطوة الثانية: كتابة النص حرف بحرف
        if (dialogueText != null)
        {
            dialogueText.text = ""; 
            foreach (char c in textToType)
            {
                dialogueText.text += c;
                yield return new WaitForSecondsRealtime(typeSpeed);
            }
        }

        // الخطوة الثالثة: إظهار التيكست الإضافي واللوحة اللي وراه مع بعض
        if (extraPanelGroup != null) 
        {
            StartCoroutine(FadeCanvasGroup(extraPanelGroup, 1f, 0.4f));
        }
        
        if (extraTextGroup != null) 
        {
            yield return StartCoroutine(FadeCanvasGroup(extraTextGroup, 1f, 0.4f));
        }
        else if (extraPanelGroup != null) 
        {
            yield return new WaitForSecondsRealtime(0.4f);
        }

        // الخطوة الرابعة: إظهار الزراير مع بعض
        if (saveButtonGroup != null) 
        {
            StartCoroutine(FadeCanvasGroup(saveButtonGroup, 1f, 0.4f));
        }
        
        if (leaveButtonGroup != null) 
        {
            yield return StartCoroutine(FadeCanvasGroup(leaveButtonGroup, 1f, 0.4f));
        }
    }


    // ==========================================
    // 4. دوال مساعدة (للإخفاء والإظهار)
    // ==========================================

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float targetAlpha, float duration)
    {
        if (cg == null) 
        {
            yield break; // لو الخانة فاضية، تجاهلها وكمل الكود
        }
        
        float elapsed = 0f;
        float startAlpha = cg.alpha;
        
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            yield return null;
        }
        
        cg.alpha = targetAlpha;
        cg.interactable = (targetAlpha == 1f);
        cg.blocksRaycasts = (targetAlpha == 1f);
    }

    private void HideGroup(CanvasGroup cg)
    {
        if (cg == null) 
        {
            return; // لو الخانة فاضية، تجاهلها
        }
        
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }


    // ==========================================
    // 5. دوال الزراير (للربط في الـ OnClick)
    // ==========================================

    public void ChooseGood() 
    {
        if (GameManager.Instance != null) 
        {
            GameManager.Instance.AddWaterEvent(10f);
        }

        // تشغيل الكات سين الخاصة بالنصيحة
        if (adviceCutscene != null)
        {
            adviceCutscene.PlayAdviceEffect();
        }

        CloseDialogue(); 
    }

    public void ChooseBad() 
    {
        if (GameManager.Instance != null) 
        {
            GameManager.Instance.SubtractWaterEvent(10f);
        }
        CloseDialogue(); 
    }

    public void ChooseNeutral() 
    {
        CloseDialogue();
    }
}