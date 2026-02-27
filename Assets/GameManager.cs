using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Water System")]
    public float maxWater = 100f;
    public float currentWater;
    public float waterGainPerTap = 10f;
    public Image waterBar;

    [Header("Timer System")]
    public float phaseTime = 120f;
    private float currentTime;
    public TMP_Text timerText;

    [Header("Tap System")]
    public TapController[] taps;
    public int tapsRequired = 7;
    private int fixedTaps = 0;

    [Header("Leak Spawner")]
    public float leakInterval = 20f;
    private float leakTimer;

    private bool gameEnded = false;

    // ======================
    // AWAKE
    // ======================
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // ======================
    // START
    // ======================
    void Start()
    {
        currentWater = maxWater;
        currentTime = phaseTime;
        leakTimer = leakInterval;
        fixedTaps = 0;
        gameEnded = false;
    }

    // ======================
    // UPDATE
    // ======================
    void Update()
    {
        if (gameEnded) return;

        UpdateTimer();
        UpdateLeaks();
        UpdateWaterBar();

        if (currentWater <= 0)
            LoseGame();
    }

    // ======================
    // TIMER
    // ======================
    void UpdateTimer()
    {
        currentTime -= Time.deltaTime;

        if (timerText != null)
            timerText.text = Mathf.Ceil(currentTime).ToString();

        if (currentTime <= 0)
            CheckPhaseResult();
    }

    void CheckPhaseResult()
    {
        if (fixedTaps >= tapsRequired)
            WinGame();
        else
            LoseGame();
    }

    // ======================
    // LEAK SYSTEM
    // ======================
    void UpdateLeaks()
    {
        leakTimer -= Time.deltaTime;

        if (leakTimer <= 0)
        {
            TriggerRandomLeaks();
            leakTimer = leakInterval;
        }
    }

    void TriggerRandomLeaks()
    {
        int leaksToStart = Random.Range(2, 4);

        for (int i = 0; i < leaksToStart; i++)
        {
            TapController tap = taps[Random.Range(0, taps.Length)];

            if (!tap.isLeaking)
                tap.StartLeaking();
        }
    }

    // ======================
    // WATER SYSTEM
    // ======================
    void UpdateWaterBar()
    {
        if (waterBar != null)
            waterBar.fillAmount = currentWater / maxWater;
    }

    public void ReduceWater(float amount)
    {
        currentWater -= amount;
        currentWater = Mathf.Clamp(currentWater, 0, maxWater);
    }

    public void AddWater(float amount)
    {
        currentWater += amount;
        currentWater = Mathf.Clamp(currentWater, 0, maxWater);
    }

    public void AddWaterPercent(float percent)
    {
        float amount = maxWater * (percent / 100f);
        AddWater(amount);
    }

    public void RemoveWaterPercent(float percent)
    {
        float amount = maxWater * (percent / 100f);
        currentWater -= amount;
        currentWater = Mathf.Clamp(currentWater, 0, maxWater);
    }

    // ======================
    // TAP FIX
    // ======================
    public void TapFixed()
    {
        fixedTaps++;
        AddWater(waterGainPerTap);

        if (fixedTaps >= tapsRequired)
            WinGame();
    }

    // ======================
    // WIN / LOSE
    // ======================
    void WinGame()
    {
        if (gameEnded) return;

        gameEnded = true;
        Debug.Log("YOU WIN");
    }

    void LoseGame()
    {
        if (gameEnded) return;

        gameEnded = true;
        Debug.Log("YOU LOSE");
    }

    // ======================
    // DEBUG INFO
    // ======================
    public int GetFixedTaps()
    {
        return fixedTaps;
    }
}