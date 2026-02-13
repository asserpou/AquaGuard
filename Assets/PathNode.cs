using UnityEngine;
using System.Collections.Generic;

public class PathNode : MonoBehaviour
{
    // قائمة بالنقط "القريبة" اللي ينفع الـ NPC يروح لها من هنا
    public List<PathNode> neighbors;

    // رسم خطوط في الـ Scene عشان تشوف الشبكة اللي عملتها
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        foreach (var neighbor in neighbors)
        {
            if (neighbor != null)
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }
}