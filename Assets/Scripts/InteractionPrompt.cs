using UnityEngine;
using TMPro;
using System.Collections;

public class InteractionPrompt : MonoBehaviour
{
    public static InteractionPrompt Instance; // عشان نقدر ننادي عليه من أي مكان في اللعبة
    
    private CanvasGroup canvasGroup;
    public TextMeshProUGUI promptText;

    private void Awake()
    {
        Instance = this;
        canvasGroup = GetComponent<CanvasGroup>();
        
        // نخفيها في بداية اللعبة
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void ShowPrompt(string actionName)
    {
        // بنغير الكلمة حسب إنت واقف فين (مثلاً: Press E to Talk, أو Press E to Fix)
        promptText.text = "<color=yellow>[E]</color> " + actionName;
        
        StopAllCoroutines();
        StartCoroutine(FadeAlpha(1f, 0.2f)); // تظهر في جزء من الثانية
    }

    public void HidePrompt()
    {
        StopAllCoroutines();
        StartCoroutine(FadeAlpha(0f, 0.2f)); // تختفي بسرعة
    }

    private IEnumerator FadeAlpha(float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }
}