using UnityEngine;

public class Window_questPointer : MonoBehaviour {

    [Header("UI Objects")]
    public GameObject pointerImage; // اسحب صورة السهم هنا

    [Header("Target")]
    public Transform target;        // اسحب "البيت" من الـ Hierarchy هنا

    [Header("Settings")]
    public float borderSize = 50f;  // المسافة عن حافة الشاشة

    void Update() {
        if (target == null || pointerImage == null) return;

        // 1. حساب مكان الهدف على الشاشة
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);

        // 2. هل الهدف "جوه" كادر الشاشة؟
        // (screenPos.z > 0) يعني الهدف قدام الكاميرا مش وراها
        bool isInside = screenPos.z > 0 && 
                        screenPos.x > 0 && screenPos.x < Screen.width && 
                        screenPos.y > 0 && screenPos.y < Screen.height;

        if (isInside) {
            // لو وصلت للهدف والبيت باين قدامك: اخفي السهم خالص
            pointerImage.SetActive(false);
        } 
        else {
            // لو البيت بعيد أو بره الشاشة: اظهر السهم عشان يوجّهك
            pointerImage.SetActive(true);

            // لو الهدف ورا الكاميرا، بنعكس الإحداثيات عشان السهم ميتجننش
            Vector3 cappedScreenPos = screenPos;
            if (cappedScreenPos.z < 0) cappedScreenPos *= -1f;

            // 3. حبس السهم على أطراف الشاشة (Clamping)
            float x = Mathf.Clamp(cappedScreenPos.x, borderSize, Screen.width - borderSize);
            float y = Mathf.Clamp(cappedScreenPos.y, borderSize, Screen.height - borderSize);
            
            pointerImage.transform.position = new Vector3(x, y, 0);

            // 4. تدوير السهم ليشير لمكان البيت
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Vector3 dir = (cappedScreenPos - screenCenter).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            pointerImage.transform.localEulerAngles = new Vector3(0, 0, angle);
        }
    }
}