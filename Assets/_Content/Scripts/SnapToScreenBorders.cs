using System;
using System.Collections;
using System.Collections.Generic;
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

        Vector2 newPos;

        if (snapSide == Sides.Top)      transform.position = new Vector2(transform.position.x, _camera.orthographicSize);
        if (snapSide == Sides.Bottom)   transform.position = new Vector2(transform.position.x, _camera.orthographicSize * -1);
        
        if (snapSide == Sides.Left)     transform.position = new Vector2(_camera.orthographicSize / cameraRatio * -1, transform.position.y);
        if (snapSide == Sides.Right)    transform.position = new Vector2(_camera.orthographicSize / cameraRatio, transform.position.y);
    }
}
