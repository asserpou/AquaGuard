using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

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
    private bool isGameWon = false; // ضفنا دي عشان نعرف هو كسب ولا خسر

    [Header("UI Panels")]
    public GameObject winPanel; // صفحة الفوز

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
        isGameWon = true; // اللاعب كسب
        CancelInvoke();
        Debug.Log("YOU WIN - Goal Reached!");
    }

    void LoseGame()
    {
        gameEnded = true;
        isGameWon = false; // اللاعب خسر
        CancelInvoke();
        Debug.Log("YOU LOSE - Out of Water or Time!");
    }

    public void ShowWinScreen()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f;      
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;    
        }
    }

    // الدالة دي اللي كود البيت وكود السهم هيسألوها
    public bool IsGameWon() 
    {
        return isGameWon;
    }
}