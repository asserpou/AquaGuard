using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("إعدادات السرعة")]
    public float dragSpeed = 2f; // سرعة السحب (غيرها براحتك)

    [Header("إعدادات الحدود")]
    public float minX, maxX, minY, maxY;

    void Update()
    {
        // بنتشيك لو اللاعب ضاغط كليك يمين
        if (Input.GetMouseButton(1))
        {
            // بناخد حركة الماوس (الفرق بين مكانه الحالي ومكانه في الفريم اللي فات)
            float moveX = Input.GetAxis("Mouse X") * dragSpeed;
            float moveY = Input.GetAxis("Mouse Y") * dragSpeed;

            // بنطرح الحركة من مكان الكاميرا الحالي
            // ملاحظة: لو الحركة معكوسة (يعني بتشد يمين بتروح شمال)، غير الـ ناقص لـ زائد
            Vector3 newPos = transform.position - new Vector3(moveX, moveY, 0);

            // نطبق الحدود عشان ما نخرجش بره الماب
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
            newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

            // بننقل الكاميرا للمكان الجديد
            transform.position = newPos;
        }
    }
}