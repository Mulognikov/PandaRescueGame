using UnityEngine;

public class CameraToAspectRatio : MonoBehaviour
{
    private Camera _camera;
    
    private void Awake()
    {
        _camera = GetComponent<Camera>();

        float ratio16_9 = 16f / 9f;
        float ratioScreen = Screen.height / (float)Screen.width;
        float cameraRatio = ratioScreen / ratio16_9;
        _camera.orthographicSize *= cameraRatio;
    }
}
