using UnityEngine;

public class NPCWalker : MonoBehaviour
{
    public PathNode currentTarget;
    public float speed = 1.5f;
    public float waitTime = 1.0f;
    private bool isWaiting = false;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentTarget == null || isWaiting)
        {
            if (anim) anim.SetBool("isWalking", false);
            return;
        }

        // 1. حساب اتجاه الحركة
        Vector2 direction = (currentTarget.transform.position - transform.position).normalized;

        // 2. بعت الاتجاه للأنيميتور
        if (anim)
        {
            anim.SetFloat("MoveX", direction.x);
            anim.SetFloat("MoveY", direction.y);
            anim.SetBool("isWalking", true);
        }

        // 3. الحركة الفعلية
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.transform.position, speed * Time.deltaTime);

        // 4. الوصول للنقطة
        if (Vector2.Distance(transform.position, currentTarget.transform.position) < 0.1f)
        {
            StartCoroutine(WaitAndPickNext());
        }
    }

    System.Collections.IEnumerator WaitAndPickNext()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        if (currentTarget.neighbors.Count > 0)
        {
            int randomIndex = Random.Range(0, currentTarget.neighbors.Count);
            currentTarget = currentTarget.neighbors[randomIndex];
        }
        isWaiting = false;
    }
}