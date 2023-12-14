using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class CameraAndBackgroundMove : MonoBehaviour
{
    [SerializeField] private Transform _cloudsNear;
    [SerializeField] private Transform _cloudsFar;
    [SerializeField] private Transform _environment;
    
    [Space]
    [SerializeField] private Vector3 _cloudsNearNormalPos;
    [SerializeField] private Vector3 _cloudsFarNormalPos;
    [SerializeField] private Vector3 _environmentNormalPos;
    
    [Space]
    [SerializeField] private Vector3 _cloudsNearFlyPos;
    [SerializeField] private Vector3 _cloudsFarFlyPos;
    [SerializeField] private Vector3 _environmentFlyPos;

    [Space]
    [SerializeField] private Vector3 _cameraFlyPos;
    [SerializeField] private Vector3 _cameraNormalPos;
    
    private Camera _camera;
    private UISettings _uiSettings;

    private Ease _ease = Ease.InOutQuart;
    private bool _isFlying = true;

    [Inject]
    private void Construct(Camera camera, UISettings uiSettings)
    {
        _camera = camera;
        _uiSettings = uiSettings;
    }

    private void Awake()
    {
        FastFly();
    }

    public void Normal()
    {
        if (!_isFlying) return;
        _isFlying = false;

        _cloudsFar.DOMove(_cloudsFarNormalPos, _uiSettings.CameraFlyTime).SetEase(_ease);
        _cloudsNear.DOMove(_cloudsNearNormalPos, _uiSettings.CameraFlyTime).SetEase(_ease);
        _environment.DOMove(_environmentNormalPos, _uiSettings.CameraFlyTime).SetEase(_ease);
        _camera.transform.DOMove(_cameraNormalPos, _uiSettings.CameraFlyTime).SetEase(_ease);
    }

    public void Fly()
    {
        if (_isFlying) return;
        _isFlying = true;
        
        _cloudsFar.DOMove(_cloudsFarFlyPos, _uiSettings.CameraFlyTime).SetEase(_ease);
        _cloudsNear.DOMove(_cloudsNearFlyPos, _uiSettings.CameraFlyTime).SetEase(_ease);
        _environment.DOMove(_environmentFlyPos, _uiSettings.CameraFlyTime).SetEase(_ease);
        _camera.transform.DOMove(_cameraFlyPos, _uiSettings.CameraFlyTime).SetEase(_ease);
    }

    public void FastNormal()
    {
        _cloudsFar.position = _cloudsFarNormalPos;
        _cloudsNear.position = _cloudsNearNormalPos;
        _environment.position = _environmentNormalPos;
        _camera.transform.position = _cameraNormalPos;
    }

    public void FastFly()
    {
        _cloudsFar.position = _cloudsFarFlyPos;
        _cloudsNear.position = _cloudsNearFlyPos;
        _environment.position = _environmentFlyPos;
        _camera.transform.position = _cameraFlyPos;
    }
}
