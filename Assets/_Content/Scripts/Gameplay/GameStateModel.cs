using System;
using UnityEngine;

public class GameStateModel
{
    public event Action GameStateChangedEvent;
    
    private float _timeLeft;
    private GameStateEnum _currentGameState;

    public float TimeLeft
    {
        get => _timeLeft;
        protected set
        {
            _timeLeft = value;
            if (_timeLeft < 0) _timeLeft = 0;
        }
    }

    public GameStateEnum CurrentGameState
    {
        get => _currentGameState;
        protected set
        {
            _currentGameState = value;
            Debug.Log(value);
            GameStateChangedEvent?.Invoke();
        }
    }
}

public class WritableGameStateModel : GameStateModel
{
    public void SetState(GameStateEnum state)
    {
        CurrentGameState = state;
    }

    public void SetTimeLeft(float timeLeft)
    {
        TimeLeft = timeLeft;
    }
}

public enum GameStateEnum
{
    Draw,
    Play,
    Lose,
    Win
}
