using UnityEngine;

public class RandomWander : MonoBehaviour
{
    public Transform[] moveSpots;
    public float speed = 2.0f;
    private int randomSpot;
    private float waitTime;
    public float startWaitTime = 1.0f;

    void Start()
    {
        randomSpot = Random.Range(0, moveSpots.Length);
        waitTime = startWaitTime;
    }

    void Update()
    {
        // التأكد إن النقطة مش فاضية عشان ميعملش Error
        if (moveSpots[randomSpot] == null) return;

        // 1. حساب الاتجاه مع تجاهل الـ Z تماماً
        Vector3 targetPos = new Vector3(moveSpots[randomSpot].position.x, moveSpots[randomSpot].position.y, transform.position.z);

        // 2. الحركة
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // 3. المسافة (زودنا الرقم لـ 0.3 عشان نضمن إنه وصل)
        if (Vector2.Distance(transform.position, targetPos) < 0.3f)
        {
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);
                waitTime = startWaitTime;
                FlipSprite();
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    void FlipSprite()
    {
        if (moveSpots[randomSpot].position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveSpots[randomSpot].position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    // حركة صايعة عشان تشوف النقط والخطوط في الـ Scene view بس
    private void OnDrawGizmos()
    {
        if (moveSpots == null) return;
        Gizmos.color = Color.red;
        foreach (Transform spot in moveSpots)
        {
            if (spot != null) Gizmos.DrawWireSphere(spot.position, 0.5f);
        }
    }
}