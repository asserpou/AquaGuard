using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 60f; // الوقت الكلي بالثواني
    public Text timerText; // نص على الشاشة لعرض الوقت
    public bool gameOverWhenTimeEnds = true; // هل ينتهي اللعبة لما الوقت يخلص

    private bool timerIsRunning = false;

    void Start()
    {
        timerIsRunning = true;
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (!timerIsRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            timeRemaining = 0;
            timerIsRunning = false;
            UpdateTimerDisplay();
            if (gameOverWhenTimeEnds)
            {
                Debug.Log("Game Over!"); // هنا ممكن تحط أي حاجة بدل الـ Debug
            }
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
