using UnityEngine;

public class Window_questPointer : MonoBehaviour {
    [Header("UI Objects")]
    public GameObject pointerImage; 
    public GameObject dangerIcon;   
    public GameObject speechBubble; 

    [Header("Target")]
    public Transform target; 

    void Update() {
        if (GameManager.Instance == null || !GameManager.Instance.IsGameWon()) {
            if (pointerImage.activeSelf) pointerImage.SetActive(false);
            if (dangerIcon.activeSelf) dangerIcon.SetActive(false);
            if (speechBubble.activeSelf) speechBubble.SetActive(false);
            return; //
        }

        if (target == null) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);
        bool isInside = screenPos.z > 0 && screenPos.x > 50 && screenPos.x < Screen.width - 50 && screenPos.y > 50 && screenPos.y < Screen.height - 50;

        if (isInside) {
            if (!dangerIcon.activeSelf) dangerIcon.SetActive(true);
            if (!speechBubble.activeSelf) speechBubble.SetActive(true);
            if (pointerImage.activeSelf) pointerImage.SetActive(false);
        } else {
            if (dangerIcon.activeSelf) dangerIcon.SetActive(false);
            if (speechBubble.activeSelf) speechBubble.SetActive(false);
            if (!pointerImage.activeSelf) pointerImage.SetActive(true);

            Vector3 cappedPos = screenPos;
            if (cappedPos.z < 0) cappedPos *= -1f;
            float x = Mathf.Clamp(cappedPos.x, 50, Screen.width - 50);
            float y = Mathf.Clamp(cappedPos.y, 50, Screen.height - 50);
            pointerImage.transform.position = new Vector3(x, y, 0f);

            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            Vector3 dir = (new Vector3(x, y, 0f) - screenCenter).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            pointerImage.transform.localEulerAngles = new Vector3(0, 0, angle); 
        }
    }
}