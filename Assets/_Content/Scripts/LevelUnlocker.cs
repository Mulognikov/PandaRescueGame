using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUnlocker : IDisposable
{
    private GameStateModel _gameState;
    private GameScore _gameScore;

    public LevelUnlocker(GameStateModel gameState, GameScore gameScore)
    {
        _gameState = gameState;
        _gameScore = gameScore;

        _gameState.GameStateChangedEvent += OnGameStateChanged;
    }
    
    public void Dispose()
    {
        _gameState.GameStateChangedEvent -= OnGameStateChanged;
    }

    public static int GetLevelStars(int levelIndex)
    {
        return PlayerPrefs.GetInt(levelIndex.ToString());
    }

    public static bool GetLevelUnlockStatus(int levelIndex)
    {
        if (levelIndex <= 1) return true;
        return PlayerPrefs.GetInt((levelIndex - 1).ToString()) > 0;
    }

    private void OnGameStateChanged()
    {
        if (_gameState.CurrentGameState == GameStateEnum.Win)
        {
            int currentStars = GetLevelStars(SceneManager.GetActiveScene().buildIndex);
            if (_gameScore.Stars > currentStars)
            {
                UnlockLevel(SceneManager.GetActiveScene().buildIndex, _gameScore.Stars);
            }
        }
    }

    private void UnlockLevel(int levelIndex, int stars)
    {
        PlayerPrefs.SetInt(levelIndex.ToString(), stars);
    }
}
