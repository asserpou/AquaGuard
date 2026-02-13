using UnityEngine;
using System.Collections;

public class NPCWander : MonoBehaviour
{
    public float speed = 2f;
    public float moveTime = 2f;
    public float waitTime = 1f;

    private Vector2 moveDirection;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Wander());
    }

    IEnumerator Wander()
    {
        while (true)
        {
            // اختار اتجاه راندم
            moveDirection = RandomDirection();

            // وقت المشي
            float timer = 0;
            while (timer < moveTime)
            {
                rb.linearVelocity = moveDirection * speed;
                timer += Time.deltaTime;
                yield return null;
            }

            // وقف
            rb.linearVelocity = Vector2.zero;

            // استنى شوية
            yield return new WaitForSeconds(waitTime);
        }
    }

    Vector2 RandomDirection()
    {
        int dir = Random.Range(0, 4);

        switch (dir)
        {
            case 0: return Vector2.up;
            case 1: return Vector2.down;
            case 2: return Vector2.left;
            default: return Vector2.right;
        }
    }
}
