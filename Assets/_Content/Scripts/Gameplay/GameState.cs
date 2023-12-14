using System;
using UnityEngine;
using Zenject;

public class GameState: ITickable, IDisposable, IInitializable
{
    private WritableGameStateModel _stateModel;
    private GameSettings _gameSettings;
    private Survivor[] _survivors;
    private Line _line;
    private GameplayUI _gameplayUI;

    public GameState(GameStateModel writableStateModel ,GameSettings settings, Survivor[] survivors, Line line, GameplayUI gameplayUI)
    {
        _stateModel = (WritableGameStateModel)writableStateModel;
        _gameSettings = settings;
        _survivors = survivors;
        _line = line;
        _gameplayUI = gameplayUI;

        foreach (var s in _survivors)
        {
            s.SurvivorHitEvent += SurvivorHit;
        }

        _line.DrawEndEvent += DrawEnd;
        _gameplayUI.MenuButtonClickEvent += GameStop;
    }
    
    public void Dispose()
    {
        foreach (var s in _survivors)
        {
            s.SurvivorHitEvent -= SurvivorHit;
        }
        
        _line.DrawEndEvent -= DrawEnd;
        _gameplayUI.MenuButtonClickEvent -= GameStop;
    }
    
    public void Initialize()
    {
        _stateModel.SetTimeLeft(_gameSettings.GameDuration);
        _stateModel.SetState(GameStateEnum.Draw);
    }
    
    public void Tick()
    {
        if (_stateModel.CurrentGameState == GameStateEnum.Play)
        {
            UpdateTimeLeft();
        }
    }

    private void DrawEnd()
    {
        if (_stateModel.CurrentGameState == GameStateEnum.Draw) _stateModel.SetState(GameStateEnum.Play);
    }

    private void SurvivorHit()
    {
        if (_stateModel.CurrentGameState == GameStateEnum.Play) _stateModel.SetState(GameStateEnum.Lose);
    }

    private void GameStop()
    {
        _stateModel.SetState(GameStateEnum.Lose);
    }

    private void UpdateTimeLeft()
    {
        _stateModel.SetTimeLeft(_stateModel.TimeLeft - Time.deltaTime);
        
        if (_stateModel.TimeLeft <= 0)
        {
            _stateModel.SetState(GameStateEnum.Win);
        }
    }
}
