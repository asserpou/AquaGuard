using UnityEngine;
using System.Collections;
using TMPro;

public class AdviceCutscene : MonoBehaviour
{
    [Header("Characters")]
    public GameObject neighborWasting; // الراجل اللي بيرش ماية
    public GameObject neighborFixed;   // الراجل المظبوط

    [Header("AquaGuard Bubble")]
    public GameObject aquaGuardBubble; // فقاعة كلام البطل
    public TextMeshProUGUI aquaGuardText;

    [Header("Neighbor Bubble")]
    public GameObject neighborBubble; // فقاعة كلام الجار
    public TextMeshProUGUI neighborText;

    [Header("Effects (Optional)")]
    public GameObject appreciationEffect; 

    public void PlayAdviceEffect()
    {
        StartCoroutine(ExecuteCutscene());
    }

    IEnumerator ExecuteCutscene()
    {
        // 1. تبديل الشخصية (يقفل الخرطوم)
        if (neighborWasting != null) neighborWasting.SetActive(false);
        if (neighborFixed != null) neighborFixed.SetActive(true);

        // التأكد إن الفقاعتين مقفولين في البداية
        if (aquaGuardBubble != null) aquaGuardBubble.SetActive(false);
        if (neighborBubble != null) neighborBubble.SetActive(false);

        // ==========================================
        // 2. إظهار كلام AquaGuard
        // ==========================================
        if (aquaGuardBubble != null)
        {
            aquaGuardBubble.SetActive(true);
            if (aquaGuardText != null)
            {
                aquaGuardText.text = "Excuse me! Leaving the hose running wastes a lot of clean water. We must conserve it!";
            }
        }
        
        // نستنى 4 ثواني
        yield return new WaitForSecondsRealtime(4f); 

        // نقفل فقاعة AquaGuard
        if (aquaGuardBubble != null) aquaGuardBubble.SetActive(false);


        // ==========================================
        // 3. إظهار كلام الجار
        // ==========================================
        if (neighborBubble != null)
        {
            neighborBubble.SetActive(true);
            if (neighborText != null)
            {
                neighborText.text = "You're right. I'm really sorry! I will turn it off now and promise it won't happen again.";
            }
        }

        // إظهار تأثير الشكر (لو ضايفه)
        if (appreciationEffect != null) appreciationEffect.SetActive(true);

        // نستنى 4 ثواني كمان
        yield return new WaitForSecondsRealtime(4f); 


        // ==========================================
        // 4. نهاية الكات سين
        // ==========================================
        if (neighborBubble != null) neighborBubble.SetActive(false);
        if (appreciationEffect != null) appreciationEffect.SetActive(false);
    }
}