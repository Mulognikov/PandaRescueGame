using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine.Audio;

public class MusicController : IDisposable
{
    private AudioMixer _mixer;
    private GameStateModel _gameState;
    private UISettings _uiSettings;
    private GameplayUI _gameplayUI;

    private const string _menuVolumeKey = "Menu";
    private const string _gameVolumeKey = "Game";
    private const string _winVolumeKey = "Win";
    
    public MusicController(AudioMixer mixer, GameStateModel gameState, UISettings uiSettings, GameplayUI gameplayUI)
    {
        _mixer = mixer;
        _gameState = gameState;
        _uiSettings = uiSettings;
        _gameplayUI = gameplayUI;
        
        Init();
    }

    public void Dispose()
    {
        _gameState.GameStateChangedEvent -= OnGameStateChanged;
    }

    private void Init()
    {
        _gameState.GameStateChangedEvent += OnGameStateChanged;
    }

    private void OnGameStateChanged()
    {
        if (_gameState.CurrentGameState == GameStateEnum.Draw)
        {
            GameSound();
        }
        
        if (_gameState.CurrentGameState == GameStateEnum.Win)
        {
            WinSound();
        }
        
        if (_gameState.CurrentGameState == GameStateEnum.Lose)
        {
            MenuSound();
        }
    }

    private async void MenuSound()
    {
        if (!_gameplayUI.ShowMenu)
        {
            await Task.Delay((int)(_uiSettings.WaitMusicEndGame * 1000));
        }
        
        _mixer.DOSetFloat(_menuVolumeKey, 0f, _uiSettings.MusicFadeTime).SetEase(Ease.OutExpo);
        _mixer.DOSetFloat(_gameVolumeKey, -80f, _uiSettings.MusicFadeTime).SetEase(Ease.InExpo);
        _mixer.DOSetFloat(_winVolumeKey, -80f, _uiSettings.MusicFadeTime).SetEase(Ease.InExpo);
    }
    
    private async void GameSound()
    {
        await Task.Delay((int)(_uiSettings.WaitMusicStartGame * 1000));

        _mixer.DOSetFloat(_menuVolumeKey, -80f, _uiSettings.MusicFadeTime).SetEase(Ease.InExpo);
        _mixer.DOSetFloat(_gameVolumeKey, 0f, _uiSettings.MusicFadeTime).SetEase(Ease.OutExpo);
        _mixer.DOSetFloat(_winVolumeKey, -80f, _uiSettings.MusicFadeTime).SetEase(Ease.InExpo);
    }
    
    private async void WinSound()
    {
        await Task.Delay((int)(_uiSettings.WaitMusicEndGame * 1000));
        
        _mixer.DOSetFloat(_menuVolumeKey, -80f, _uiSettings.MusicFadeTime).SetEase(Ease.InExpo);
        _mixer.DOSetFloat(_gameVolumeKey, -80f, _uiSettings.MusicFadeTime).SetEase(Ease.InExpo);
        _mixer.DOSetFloat(_winVolumeKey, 0f, _uiSettings.MusicFadeTime).SetEase(Ease.OutExpo);
    }
}
