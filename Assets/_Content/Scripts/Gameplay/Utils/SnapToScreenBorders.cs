using UnityEngine;
using Zenject;

public class SnapToScreenBorders : MonoBehaviour
{
    public enum Sides
    {
        Top,
        Bottom,
        Left,
        Right
    }

    [SerializeField] private Sides snapSide;
    [SerializeField] private bool _snapToEdge;
    private Camera _camera;

    [Inject]
    private void Construct(Camera camera)
    {
        _camera = camera;
    }

    private void Start()
    {
        float ratio16_9 = 16f / 9f;
        float ratioScreen = Screen.height / (float)Screen.width;
        float cameraRatio = ratioScreen / ratio16_9;

        float offsetY = 0;
        float offsetX = 0;
        
        if (TryGetComponent(out BoxCollider2D collider) && _snapToEdge)
        {
            offsetY = collider.size.y / 2;
            offsetX = collider.size.x / 2;
        }

        if (snapSide == Sides.Top) transform.position = new Vector2(transform.position.x, _camera.orthographicSize + offsetY);
        if (snapSide == Sides.Bottom) transform.position = new Vector2(transform.position.x, _camera.orthographicSize * -1 - offsetY);


        if (snapSide == Sides.Left)     transform.position = new Vector2(_camera.orthographicSize / cameraRatio * -1, transform.position.y - offsetX);
        if (snapSide == Sides.Right)    transform.position = new Vector2(_camera.orthographicSize / cameraRatio, transform.position.y + offsetX);
    }
}
