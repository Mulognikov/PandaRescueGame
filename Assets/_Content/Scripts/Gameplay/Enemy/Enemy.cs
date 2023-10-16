using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _hitForce = 10f;
    public float HitForce => _hitForce;
    
    protected GameState _gameState;

    [Inject]
    protected virtual void Construct(GameState gameState)
    {
        _gameState = gameState;
    }

    private void OnEnable()
    {
        _gameState.GameStateChangedEvent += OnGameStateChange;
    }

    private void OnDisable()
    {
        _gameState.GameStateChangedEvent -= OnGameStateChange;
    }

    protected virtual void OnGameStateChange()
    {
        if (_gameState.CurrentGameState == GameState.GameStateEnum.Play) StartGame();
        if (_gameState.CurrentGameState == GameState.GameStateEnum.Win) StopGame();
    }

    protected virtual void StartGame()
    {
        
    }

    protected virtual void StopGame()
    {
        
    }

}
