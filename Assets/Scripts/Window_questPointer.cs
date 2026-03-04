using UnityEngine;
using UnityEngine.UI;

public class Window_questPointer : MonoBehaviour
{
    [Header("Main References")]
    public Transform target;           // مكان البيت أو الهدف 
    public GameObject dangerMarker;    // علامة الخطر 
    public GameObject questImage;      // الـ Image اللي طلبت نضيفها للكود
    public RectTransform pointerImage; // صورة السهم (الـ UI)

    [Header("Settings")]
    public float borderSize = 50f;     
    public float angleOffset = -90f;   

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Start()
    {
        // أول ما اللعبة تفتح، بنخفيهم كلهم
        if (pointerImage != null) pointerImage.gameObject.SetActive(false);
        if (dangerMarker != null) dangerMarker.SetActive(false);
        if (questImage != null) questImage.SetActive(false); // إخفاء الـ Image
    }

    void Update()
    {
        // بنسأل الـ GameManager، خلصنا الحنفيات؟
        if (GameManager.Instance == null || !GameManager.Instance.IsGameWon())
        {
            return; // لو لسه مخلصناش، وقف الكود ومتعملش حاجة
        }

        // --- من أول هنا الكود بيشتغل عشان الحنفيات اتصلحت ---

        if (target == null || pointerImage == null || cam == null) return;

        // 1. نظهر علامة الخطر
        if (dangerMarker != null && !dangerMarker.activeSelf)
        {
            dangerMarker.SetActive(true);
        }

        // 2. نظهر الـ Image اللي إنت ضفتها
        if (questImage != null && !questImage.activeSelf)
        {
            questImage.SetActive(true);
        }

        // 3. كود السهم عشان يشاور على البيت
        Vector3 targetPosScreenPoint = cam.WorldToScreenPoint(target.position);
        
        bool isOffScreen = targetPosScreenPoint.x <= 0 || targetPosScreenPoint.x >= Screen.width || 
                           targetPosScreenPoint.y <= 0 || targetPosScreenPoint.y >= Screen.height || targetPosScreenPoint.z < 0;

        if (isOffScreen)
        {
            pointerImage.gameObject.SetActive(true);

            if (targetPosScreenPoint.z < 0)
            {
                targetPosScreenPoint *= -1;
            }

            Vector3 cappedTargetScreenPosition = targetPosScreenPoint;
            cappedTargetScreenPosition.x = Mathf.Clamp(cappedTargetScreenPosition.x, borderSize, Screen.width - borderSize);
            cappedTargetScreenPosition.y = Mathf.Clamp(cappedTargetScreenPosition.y, borderSize, Screen.height - borderSize);

            pointerImage.position = cappedTargetScreenPosition;

            Vector3 direction = targetPosScreenPoint - new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            pointerImage.rotation = Quaternion.Euler(0, 0, angle + angleOffset);
        }
        else
        {
            pointerImage.gameObject.SetActive(false);
        }
    }
}