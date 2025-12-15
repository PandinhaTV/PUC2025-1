using UnityEngine;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI promptText;
    public float fadeSpeed = 10f;
    public Vector3 offset = new Vector3(0, 2f, 0);

    private Transform targetTransform;
    private bool visible;

    public void ShowPrompt(string text, Transform target)
    {
        promptText.text = text;
        targetTransform = target;
        visible = true;
    }

    public void HidePrompt()
    {
        visible = false;
        targetTransform = null;
    }

    void Update()
    {
        if (targetTransform != null)
        {
            // Position prompt above target
            transform.position = targetTransform.position + offset;
            // Face camera
            transform.LookAt(Camera.main.transform);
            transform.rotation *= Quaternion.Euler(0, 180f, 0); // Correct backward
        }

        float targetAlpha = visible ? 1 : 0;
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
    }
}