using UnityEngine;

public class HouseInteraction : MonoBehaviour
{
    // خليناها public عشان تبان في الـ Inspector وتقدر تتابعها بعينك
    public bool isPlayerNear = false; 

    void Update()
    {
        // أنا موقف شرط المكسب مؤقتاً عشان نختبر الشاشة تفتح ولا لأ من غير ما نلعب الجيم
        if (GameManager.Instance == null || !GameManager.Instance.IsGameWon()) return;

        // لو اللاعب قريب وداس E
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("🟢 1. تم الضغط على حرف E!");

            if (GameManager.Instance != null)
            {
                Debug.Log("🟢 2. الـ GameManager موجود، بنحاول نفتح شاشة الفوز...");
                GameManager.Instance.ShowWinScreen();
            }
            else
            {
                Debug.LogError("🔴 مشكلة: اللعبة مش لاقية الـ GameManager!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("🟡 اللاعب دخل المربع بتاع البيت! (isPlayerNear = true)"); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
            Debug.Log("⚪ اللاعب خرج بره البيت. (isPlayerNear = false)");
        }
    }
}