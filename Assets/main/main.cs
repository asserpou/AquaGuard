using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video; 

public class MainMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject myRawImage;
    
    // العلبة اللي جمعنا فيها زراير المنيو والخلفية
    public GameObject menuElements; 
    
    [Header("Cinematic & Transition")]
    public VideoPlayer introVideo; 
    public Image fadeImage;        
    public float fadeDuration = 1f; 

    public static bool isComingFromDetails = false;

    void Start()
    {
        if (isComingFromDetails == true)
        {
            if (myRawImage != null) myRawImage.SetActive(false);
            isComingFromDetails = false;
        }

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false);
            fadeImage.color = new Color(0, 0, 0, 0); 
        }

        if (introVideo != null)
        {
            introVideo.gameObject.SetActive(false); // بنقفل الفيديو في البداية
            introVideo.loopPointReached += OnVideoEnded;
        }
    }

    public void PlayGame()
    {
        // 1. نخفي المنيو (عشان متبقاش مغطية على الفيديو)
        if (menuElements != null) menuElements.SetActive(false);

        // 2. نشغل الفيديو وهو واخد الشاشة كلها براحته
        if (introVideo != null)
        {
            introVideo.gameObject.SetActive(true);
            introVideo.Play(); 
        }
        else
        {
            StartCoroutine(FadeAndLoadScene(1));
        }
    }

    void OnVideoEnded(VideoPlayer vp)
    {
        // أول ما الفيديو يخلص، نشغل الـ Fade
        StartCoroutine(FadeAndLoadScene(1));
    }

    IEnumerator FadeAndLoadScene(int sceneIndex)
    {
        // الشاشة السودة بتظهر فوق الفيديو ببطء
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            float elapsedTime = 0f;
            Color c = fadeImage.color;
            
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                c.a = Mathf.Clamp01(elapsedTime / fadeDuration);
                fadeImage.color = c;
                yield return null;
            }
        }

        SceneManager.LoadSceneAsync(sceneIndex);
    }

    // --- باقي دوال الزراير زي ما هي ---
    public void GoToMainMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            isComingFromDetails = true;
        }
        SceneManager.LoadSceneAsync(0);
    }

    public void details()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}