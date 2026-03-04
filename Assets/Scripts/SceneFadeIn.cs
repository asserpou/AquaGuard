using System.Collections;
using UnityEngine;
using UnityEngine.UI; // عشان نقدر نتحكم في الصور

public class SceneFadeIn : MonoBehaviour
{
    public float fadeDuration = 1.5f; // وقت الفيد (ثانية ونص)
    private Image fadeImage;

    void Start()
    {
        fadeImage = GetComponent<Image>();
        
        if (fadeImage != null)
        {
            // أول ما المشهد يفتح، بنتأكد إن الصورة سودة تماماً ومش شفافة
            Color c = fadeImage.color;
            c.a = 1f; 
            fadeImage.color = c;

            // بنبدأ نشغل أنيميشن الاختفاء
            StartCoroutine(FadeOutBlackScreen());
        }
    }

    IEnumerator FadeOutBlackScreen()
    {
        float elapsedTime = 0f;
        Color c = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            
            // بنقلل الشفافية بالتدريج من 1 (أسود) لحد 0 (شفاف)
            c.a = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = c;
            
            yield return null; // استنى للفريم اللي بعده
        }

        // أول ما الشاشة تروق وتبقى شفافة تماماً، بنقفل الصورة خالص عشان متعملش بلوك للماوس والزراير
        gameObject.SetActive(false);
    }
}