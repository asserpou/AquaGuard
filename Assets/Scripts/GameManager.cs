using System.Collections; // ضفنا دي عشان الـ Coroutine (الأنيميشن)
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;   

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Taps")]
    public List<TapController> allTaps;
    public int tapsRequired = 7;

    [Header("Timer")]
    public float phaseTime = 60f;
    private float currentTime;
    public TextMeshProUGUI timerText;

    [Header("Water")]
    public Slider waterBar;
    public float maxWater = 100f;
    private float currentWater;
    public float waterDecreaseRate = 1f;

    [Header("Leak Settings")]
    public float leakInterval = 20f;
    public int minLeaks = 2;
    public int maxLeaks = 3;

    private int fixedTaps = 0;
    private bool gameEnded = false;
    private bool isGameWon = false;

    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject instructionPanel;

    [Header("Animation Settings")]
    public float fadeDuration = 1.5f; // الوقت اللي الشاشة هتاخده عشان تظهر (تقدر تغيره من اليونيتي)

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        gameEnded = false;
        isGameWon = false;
        fixedTaps = 0;
        currentTime = phaseTime;
        currentWater = maxWater;

        if (instructionPanel != null) instructionPanel.SetActive(false);

        if (waterBar != null)
        {
            waterBar.maxValue = maxWater;
            waterBar.value = currentWater;
        }

        InvokeRepeating(nameof(ActivateRandomTaps), 5f, leakInterval);
    }

    void Update()
    {
        if (gameEnded) return;

        UpdateTimer();
        DecreaseWaterOverTime();
    }

    void UpdateTimer()
    {
        currentTime -= Time.deltaTime;
        if (currentTime < 0) currentTime = 0;

        if (timerText != null) timerText.text = Mathf.Ceil(currentTime).ToString();

        if (currentTime <= 0) CheckPhaseResult();
    }

    void DecreaseWaterOverTime()
    {
        currentWater -= waterDecreaseRate * Time.deltaTime;
        currentWater = Mathf.Clamp(currentWater, 0, maxWater);

        if (waterBar != null) waterBar.value = currentWater;

        if (currentWater <= 0) LoseGame();
    }

    void ActivateRandomTaps()
    {
        if (gameEnded) return;

        List<TapController> inactiveTaps = new List<TapController>();

        foreach (TapController tap in allTaps)
        {
            if (tap != null && !tap.isLeaking) inactiveTaps.Add(tap);
        }

        if (inactiveTaps.Count == 0) return;

        int leaksCount = Random.Range(minLeaks, maxLeaks + 1);

        for (int i = 0; i < leaksCount && inactiveTaps.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, inactiveTaps.Count);
            inactiveTaps[randomIndex].StartLeak();
            inactiveTaps.RemoveAt(randomIndex);
        }
    }

    public void TapFixed()
    {
        if (gameEnded) return;

        fixedTaps++;
        currentWater += 5f;
        currentWater = Mathf.Clamp(currentWater, 0, maxWater);

        if (waterBar != null) waterBar.value = currentWater;

        if (fixedTaps >= tapsRequired) WinGame();
    }

    void CheckPhaseResult()
    {
        if (fixedTaps >= tapsRequired) WinGame();
        else LoseGame();
    }
    void WinGame()
    {
        gameEnded = true;
        isGameWon = true;
        CancelInvoke();

        Debug.Log("YOU WIN - Goal Reached!");

        // ←←←←← إضافة جديدة: ابعت البيانات فوراً
        StartCoroutine(SendScoreToServer());

        if (instructionPanel != null)
        {
            instructionPanel.SetActive(true);
            StartCoroutine(FadeInInstruction());
        }
    }

    // ====================== إرسال الوقت للـ PHP ======================
    private IEnumerator SendScoreToServer()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Unknown Player");
        float timeLeft = Mathf.Ceil(currentTime); // الوقت المتبقي (بالثانية)

        WWWForm form = new WWWForm();
        form.AddField("player_name", playerName);
        form.AddField("time_remaining", timeLeft.ToString());

        // غير الرابط ده برابط ملفك على السيرفر
        string url = "http://localhost/NilEvo'sWebsite/save_score.php";

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("✅ تم إرسال الوقت بنجاح: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("❌ فشل الإرسال: " + www.error);
            }
        }
    }

    // ==========================================
    // دالة الأنيميشن (Coroutine) لعمل الـ Fade-in
    // ==========================================
    IEnumerator FadeInInstruction()
    {
        // بنحاول نجيب الـ CanvasGroup، ولو مش موجود الكود بيضيفه أوتوماتيك
        CanvasGroup canvasGroup = instructionPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = instructionPanel.AddComponent<CanvasGroup>();
        }

        // بنبدأ وشفافية الشاشة صفر (مخفية)
        canvasGroup.alpha = 0f;
        float elapsedTime = 0f;

        // بنزود الشفافية بالتدريج لحد ما توصل 1 (ظاهرة بالكامل)
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null; // استنى للفريم اللي بعده
        }
        canvasGroup.alpha = 1f;
    }

    void LoseGame()
    {
        gameEnded = true;
        isGameWon = false;
        CancelInvoke();
        Debug.Log("YOU LOSE - Out of Water or Time!");

        if (losePanel != null)
        {
            losePanel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ShowWinScreen()
    {
        if (instructionPanel != null) instructionPanel.SetActive(false);

        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public bool IsGameWon()
    {
        return isGameWon;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("main mune");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}