using UnityEngine;
using UnityEngine.UI;

public class HUDBar : MonoBehaviour
{
    public Image fillImage;

    void Update()
    {
        float value = PlayerStats.instance.currentValue;
        float max = PlayerStats.instance.maxValue;

        fillImage.fillAmount = value / max;
    }
}
