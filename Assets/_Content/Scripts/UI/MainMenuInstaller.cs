using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    [SerializeField] private LoadLevelButton _loadLevelButton;
    
    public override void InstallBindings()
    {
        Container.Bind<LoadLevelButton>().FromInstance(_loadLevelButton).AsSingle();
    }
}
