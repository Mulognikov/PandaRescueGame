using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private void Start()
    {
        RectTransform rect = (RectTransform)transform;
        float screenAspectRatio = Screen.height / (float)Screen.width;
        float canvasAspectRatio = Screen.height / rect.rect.width;
        float multiplier = canvasAspectRatio / screenAspectRatio;
        
        Vector2 size = new(rect.rect.width, Screen.safeArea.height / multiplier);
        rect.anchorMax = Vector2.zero;
        rect.anchorMin = Vector2.zero;
        rect.sizeDelta = size;
        rect.anchoredPosition = new Vector2(size.x / 2 + Screen.safeArea.x, size.y / 2 + Screen.safeArea.y);
    }
}
