using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameSettings _gameSettings;
    [Space]
    
    [SerializeField] private Survivor[] _doges;
    [Space]
    
    [SerializeField] private Camera _camera;
    [SerializeField] private Line _line;
    [SerializeField] private NavMeshObstacle _lineObstacle;
    [SerializeField] private Bee _bee;
    
    public override void InstallBindings()
    {
        Container.Bind<Survivor[]>().FromInstance(_doges).AsSingle();
        Container.Bind<GameSettings>().FromInstance(_gameSettings).AsSingle();
        Container.Bind<Camera>().FromInstance(_camera).AsSingle();
        Container.Bind<Line>().FromComponentInNewPrefab(_line).AsSingle().NonLazy();
        Container.Bind<NavMeshObstacle>().FromInstance(_lineObstacle).AsSingle();
        Container.Bind<Bee>().FromInstance(_bee).AsSingle();

        Container.BindInterfacesAndSelfTo<GameState>().AsSingle();
    }
}

[System.Serializable]
public class GameSettings
{
    [SerializeField] private float gameDuration = 8f;
    [SerializeField] private float gameEndDuration = 2f;
    [SerializeField] private float minDrawDistance = 0.25f;
    
    [SerializeField] private float oneStarLenght = 10f;
    [SerializeField] private float twoStarsLenght = 10f;
    [SerializeField] private float threeStarLength = 10f;
    
    public float GameDuration => gameDuration;
    public float GameEndDuration => gameEndDuration;
    public float MinDrawDistance => minDrawDistance;
    public float MaxLineLength => oneStarLenght + twoStarsLenght + threeStarLength;
    public float TwoStarsLenght => twoStarsLenght + threeStarLength;
    public float ThreeStarsLength => threeStarLength;
}
