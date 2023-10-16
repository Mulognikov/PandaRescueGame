using System;
using UnityEngine;
using Zenject;

public class GameState: ITickable
{
    public enum GameStateEnum
    {
        Draw,
        Play,
        Lose,
        Win
    }
    
    public event Action GameStateChangedEvent;
    
    private float _timeLeft;
    private GameStateEnum _currentGameState;
    private GameSettings _gameSettings;

    public float TimeLeft => _timeLeft;
    
    public GameStateEnum CurrentGameState
    {
        get => _currentGameState;
        private set
        {
            _currentGameState = value;
            Debug.Log(value);
            GameStateChangedEvent?.Invoke();
        }
    }

    public GameState(GameSettings settings)
    {
        _gameSettings = settings;
        _timeLeft = _gameSettings.GameDuration;
        CurrentGameState = GameStateEnum.Draw;
    }

    public void DrawEnd()
    {
        if (CurrentGameState == GameStateEnum.Draw) CurrentGameState = GameStateEnum.Play;
    }

    public void DogeHit()
    {
        if (CurrentGameState == GameStateEnum.Play) CurrentGameState = GameStateEnum.Lose;
    }

    public void Tick()
    {
        if (CurrentGameState == GameStateEnum.Play)
        {
            UpdateTimeLeft();
        }
    }

    private void UpdateTimeLeft()
    {
        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0)
        {
            _timeLeft = 0;
            CurrentGameState = GameStateEnum.Win;
        }
    }
}
