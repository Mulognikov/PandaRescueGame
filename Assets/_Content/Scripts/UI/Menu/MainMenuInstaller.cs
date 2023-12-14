using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraAndBackgroundMove _backgroundMove;

    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromInstance(_camera).AsSingle().NonLazy();
        Container.Bind<CameraAndBackgroundMove>().FromComponentInNewPrefab(_backgroundMove).AsSingle().NonLazy();
    }
}
