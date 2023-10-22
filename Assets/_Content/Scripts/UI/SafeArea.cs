using UnityEngine;

public class SafeArea : MonoBehaviour
{
    [SerializeField] private RectTransform _canvas;
    
    private void Start()
    {
        RectTransform rect = (RectTransform)transform;
        float screenAspectRatio = Screen.height / (float)Screen.width;
        float canvasAspectRatio = Screen.height / rect.rect.width;
        float muliplier = canvasAspectRatio / screenAspectRatio;
        
        Vector2 size = new(rect.rect.width, Screen.safeArea.height / muliplier);
        rect.anchorMax = Vector2.zero;
        rect.anchorMin = Vector2.zero;
        rect.sizeDelta = size;
        rect.anchoredPosition = new Vector2(size.x / 2 + Screen.safeArea.x, size.y / 2 + Screen.safeArea.y);
    }
}
