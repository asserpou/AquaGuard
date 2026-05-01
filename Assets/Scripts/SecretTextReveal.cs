using UnityEngine;
using TMPro;

public class SecretTextReveal : MonoBehaviour
{
    public Transform player;
    public float revealDistance = 3f;

    private TextMeshProUGUI text;
    private Color baseColor;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        baseColor = text.color;
    }

    void Update()
    {
        float distance = Vector2.Distance(player.position, transform.position);

        if (distance < revealDistance)
        {
            // Flicker when close
            float alpha = Mathf.PingPong(Time.time, 0.5f) + 0.5f;
            text.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
        }
        else
        {
            // Stay faded when far
            text.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0.1f);
        }
    }
}