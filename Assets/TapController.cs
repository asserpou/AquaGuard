using UnityEngine;

public class TapController : MonoBehaviour
{
    [Header("Leak Settings")]
    public bool isLeaking = false;
    public float waterDrainPerSecond = 5f;
    public float leakDuration = 20f;

    private float leakTimer;
    private bool playerInRange = false;
    private bool isFixed = false;

    [Header("References")]
    public GameObject interactText;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (interactText != null)
            interactText.SetActive(false);
    }

    void Update()
    {
        if (isLeaking && !isFixed)
        {
            leakTimer -= Time.deltaTime;
            GameManager.instance.ReduceWater(waterDrainPerSecond * Time.deltaTime);

            if (leakTimer <= 0)
            {
                StopLeak();
            }
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            FixTap();
        }
    }

    public void StartLeaking()
    {
        isLeaking = true;
        isFixed = false;
        leakTimer = leakDuration;

        if (anim != null)
            anim.SetBool("isLeaking", true);
    }

    void StopLeak()
    {
        isLeaking = false;

        if (anim != null)
            anim.SetBool("isLeaking", false);
    }

    void FixTap()
    {
        if (!isLeaking || isFixed) return;

        isLeaking = false;
        isFixed = true;

        if (anim != null)
            anim.SetBool("isLeaking", false);

        if (interactText != null)
            interactText.SetActive(false);

        GameManager.instance.TapFixed();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactText != null)
                interactText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactText != null)
                interactText.SetActive(false);
        }
    }
}