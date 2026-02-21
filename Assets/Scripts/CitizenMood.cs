using UnityEngine;
using UnityEngine.UI;

public class CitizenMood : MonoBehaviour
{
    public Image barFill;
    public Image face;

    public Sprite happy;
    public Sprite normal;
    public Sprite sad;

    public float maxValue = 100f;
    public float currentValue = 100f;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        // تجربة بس عشان تشوفه شغال
        currentValue -= Time.deltaTime * 5f;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);

        UpdateUI();
    }

    void UpdateUI()
    {
        float percent = currentValue / maxValue;

        // تحديث البار
        barFill.fillAmount = percent;

        // تغيير الصورة
        if (percent > 0.5f)
        {
            face.sprite = happy;
        }
        else if (percent > 0f)
        {
            face.sprite = normal;
        }
        else
        {
            face.sprite = sad;
        }
    }
}