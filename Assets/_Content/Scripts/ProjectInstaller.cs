using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<LoadScene>().FromComponentInNewPrefabResource("LoadScene").AsSingle().NonLazy();
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
