using UnityEngine;

public class Window_questPointer : MonoBehaviour 
{
    [Header("UI Objects")]
    public GameObject pointerImage; 
    public GameObject dangerIcon;   
    public GameObject speechBubble; 

    [Header("Target")]
    public Transform target; // الـ HouseTrigger

    void LateUpdate() 
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsGameWon()) 
        {
            if(pointerImage) pointerImage.SetActive(false);
            if(dangerIcon) dangerIcon.SetActive(false);
            if(speechBubble) speechBubble.SetActive(false);
            return; 
        }

        if (target == null) return;

        // تحويل مكان البيت لنقطة على الشاشة
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);
        
        bool isBehindCamera = screenPos.z < 0;
        bool isInsideScreen = !isBehindCamera && 
                              screenPos.x > 0 && screenPos.x < Screen.width && 
                              screenPos.y > 0 && screenPos.y < Screen.height;

        if (isInsideScreen) 
        {
            // البيت جوه الشاشة: إظهار العلامات وإخفاء السهم
            if(dangerIcon) dangerIcon.SetActive(true);
            if(speechBubble) speechBubble.SetActive(true);
            if(pointerImage) pointerImage.SetActive(false);

            // *** مفيش أي كود هنا بيغير مكانهم، هيفضلوا على إحداثياتهم الثابتة اللي إنت عاملها في الـ Inspector ***
        } 
        else 
        {
            // البيت بره الشاشة: إظهار السهم وإخفاء العلامات
            if(dangerIcon) dangerIcon.SetActive(false);
            if(speechBubble) speechBubble.SetActive(false);
            if(pointerImage) pointerImage.SetActive(true);

            Vector3 pointerPos = screenPos;
            if (isBehindCamera) pointerPos *= -1f;

            // تظبيط مكان السهم بس
            float margin = 50f;
            float clampedX = Mathf.Clamp(pointerPos.x, margin, Screen.width - margin);
            float clampedY = Mathf.Clamp(pointerPos.y, margin, Screen.height - margin);
            pointerImage.transform.position = new Vector3(clampedX, clampedY, 0f);

            // تدوير السهم ناحية البيت
            Vector3 centerPosition = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            Vector3 direction = (new Vector3(clampedX, clampedY, 0f) - centerPosition).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            pointerImage.transform.localEulerAngles = new Vector3(0, 0, angle); 
        }
    }
}