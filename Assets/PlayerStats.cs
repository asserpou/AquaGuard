using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxValue = 100f;
    public float currentValue = 100f;

    public static PlayerStats instance;

    void Awake()
    {
        instance = this;
    }
}
