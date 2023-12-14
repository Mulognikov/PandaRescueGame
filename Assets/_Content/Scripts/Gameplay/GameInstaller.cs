using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameSettings _gameSettings;
    
    [Space]
    [SerializeField] private Camera _camera;
    [SerializeField] private Survivor[] _doges;
    
    [Space]
    [SerializeField] private Line _line;
    [SerializeField] private NavMeshObstacle _lineObstacle;
    [SerializeField] private Bee _bee;
    [SerializeField] private ParticleSystem _hitParticle;
    
    [Space]
    [SerializeField] private CameraAndBackgroundMove _backgroundMove;
    [SerializeField] private GameplayUI _gameplayUI;
    [SerializeField] private WinPanel _winPanel;
    [SerializeField] private LosePanel _losePanel;

    [Space]
    [SerializeField] private SoundController _soundController;
    
    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromInstance(_camera).AsSingle();
        Container.Bind<Survivor[]>().FromInstance(_doges).AsSingle();
        Container.Bind<GameSettings>().FromInstance(_gameSettings).AsSingle();
        Container.Bind<Line>().FromComponentInNewPrefab(_line).AsSingle().NonLazy();
        Container.Bind<NavMeshObstacle>().FromInstance(_lineObstacle).AsSingle();
        Container.Bind<Bee>().FromInstance(_bee).AsSingle();
        Container.Bind<ParticleSystem>().FromInstance(_hitParticle).AsSingle();
        
        Container.Bind<CameraAndBackgroundMove>().FromComponentInNewPrefab(_backgroundMove).AsSingle().NonLazy();
        Container.Bind<GameplayUI>().FromComponentInNewPrefab(_gameplayUI).AsSingle().NonLazy();
        Container.Bind<WinPanel>().FromComponentInNewPrefab(_winPanel).AsSingle().NonLazy();
        Container.Bind<LosePanel>().FromComponentInNewPrefab(_losePanel).AsSingle().NonLazy();
        Container.Bind<UIController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        
        Container.Bind<SoundController>().FromComponentInNewPrefab(_soundController).AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<MusicController>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameState>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameScore>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelUnlocker>().AsSingle();

    }
}

[System.Serializable]
public class GameSettings
{
    [SerializeField] private float gameDuration = 8f;
    [SerializeField] private float minDrawDistance = 0.25f;
    
    [SerializeField] private float oneStarLenght = 10f;
    [SerializeField] private float twoStarsLenght = 10f;
    [SerializeField] private float threeStarLength = 10f;

    [SerializeField] private LayerMask _notDrawLayers;
    
    public float GameDuration => gameDuration;
    public float MinDrawDistance => minDrawDistance;
    public float MaxLineLength => oneStarLenght + twoStarsLenght + threeStarLength;
    public float TwoStarsLenght => twoStarsLenght + threeStarLength;
    public float ThreeStarsLength => threeStarLength;
    public LayerMask NotDrawLayers => _notDrawLayers;

}
