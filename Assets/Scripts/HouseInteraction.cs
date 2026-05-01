using System.Collections;
using UnityEngine;
using TMPro;

public class HouseInteraction : MonoBehaviour
{
    public bool isPlayerNear = false; 
    public DialogueManager dialogueManager; 
    private bool hasInteracted = false; 

    [Header("Cutscene Settings")]
    public GameObject cutsceneCharacter; // الراجل هيفضل هنا
    public GameObject zoomCamera; 
    public float cutsceneDuration = 3.5f; 

    [Header("UI Elements to Hide")]
    public CanvasGroup goToFarmTextGroup; // الرسالة اللي عايز تمسحها
    public GameObject waterBarUI; // عداد الماية عشان يخفيه

    void Update()
    {
        if (GameManager.Instance == null) return;

        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !hasInteracted)
        {
            hasInteracted = true; 
            StartCoroutine(PlayCutscene()); 
        }
    }

    IEnumerator PlayCutscene()
    {
        // 1. نخفي رسالة (Go to farm) بـ Fade
        if (goToFarmTextGroup != null)
        {
            StartCoroutine(FadeOutCanvasGroup(goToFarmTextGroup, 0.5f));
        }

        // 2. نخفي عداد الماية خالص من الشاشة
        if (waterBarUI != null) waterBarUI.SetActive(false);

        // 3. نظهر الراجل والزووم
        if (cutsceneCharacter != null) cutsceneCharacter.SetActive(true);
        if (zoomCamera != null) zoomCamera.SetActive(true);

        // 4. نستنى وقت اللقطة 
        yield return new WaitForSeconds(cutsceneDuration);

        // 5. نقفل كاميرا الزووم بس (عشان نرجع للكاميرا الأساسية)
        if (zoomCamera != null) zoomCamera.SetActive(false);
        // *ملحوظة: شيلنا كود إخفاء الراجل عشان يفضل موجود في الخريطة!*

        // 6. نوقف اللعبة ونفتح الحوار
        Time.timeScale = 0f; 
        if (dialogueManager != null)
        {
            dialogueManager.OpenDialogue();
        }
    }

    // دالة بتعمل Fade Out لأي حاجة
    IEnumerator FadeOutCanvasGroup(CanvasGroup cg, float duration)
    {
        float elapsed = 0f;
        float startAlpha = cg.alpha;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
            yield return null;
        }
        cg.alpha = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isPlayerNear = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isPlayerNear = false;
    }
}