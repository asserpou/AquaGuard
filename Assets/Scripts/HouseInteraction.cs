using UnityEngine;

public class HouseInteraction : MonoBehaviour
{
    private bool isPlayerNear = false;

    void Update()
    {
        // 1. لو اللعبة مخلصتش بفوز، متعملش حاجة
        if (GameManager.Instance == null || !GameManager.Instance.IsGameWon()) return;

        // 2. لو اللاعب جوه الـ Collider وداس E
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            GameManager.Instance.ShowWinScreen(); // افتح الـ Pop-up
        }
    }

    // الدالة دي بتشتغل لما اللاعب يدخل جوه الـ Box Collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    // الدالة دي بتشتغل لما اللاعب يخرج بره الـ Box Collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}