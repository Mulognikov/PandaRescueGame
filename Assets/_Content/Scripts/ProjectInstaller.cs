using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private UISettings _uiSettings;
    
    [Space]
    [SerializeField] private SelectLevelUI _selectLevelUI;
    [SerializeField] private AudioMixer _mixer;

    public override void InstallBindings()
    {
        Container.Bind<UISettings>().FromInstance(_uiSettings).AsSingle();
        Container.Bind<SelectLevelUI>().FromComponentInNewPrefab(_selectLevelUI).AsSingle().NonLazy();
        Container.Bind<AudioMixer>().FromInstance(_mixer).AsSingle();
        
        Container.Bind<LoadLevel>().FromNewComponentOnNewGameObject().AsSingle();
        Container.Bind<GameStateModel>().To<WritableGameStateModel>().AsSingle();
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
}

[System.Serializable]
public class UISettings
{
    [SerializeField] private float _uiFadeTime;
    [SerializeField] private float _cameraFlyTime;
    [SerializeField] private float _waitUIAfterGameStartTime = 1f;
    [SerializeField] private float waitCameraAfterGameEndTime = 0.5f;
    [SerializeField] private float waitUiAfterGameEndTime = 2f;
    
    [Space]
    [SerializeField] private float _waitMusicStartGame = 0.8f;
    [SerializeField] private float _waitMusicEndGame = 1.1f;
    [SerializeField] private float _musicFadeTime = 1f;

    public float UiFadeTime => _uiFadeTime;
    public float CameraFlyTime => _cameraFlyTime;
    public float WaitUiAfterGameStartTime => _waitUIAfterGameStartTime;
    public float WaitCameraAfterGameEndTime => waitCameraAfterGameEndTime;
    public float WaitUiAfterGameEndTime => waitUiAfterGameEndTime;

    public float WaitMusicStartGame => _waitMusicStartGame;
    public float WaitMusicEndGame => _waitMusicEndGame;
    public float MusicFadeTime => _musicFadeTime;
}
