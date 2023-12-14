using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Survivor : MonoBehaviour
{
    public event Action SurvivorHitEvent;
    [SerializeField] private Sprite _deadSprite;
    
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private GameStateModel _gameState;
    private SoundController _soundController;

    [Inject]
    private void Construct(GameStateModel gameState, SoundController soundController)
    {
        _gameState = gameState;
        _soundController = soundController;
    }
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _gameState.GameStateChangedEvent += OnGameStateChanged;
    }

    private void OnDisable()
    {
        _gameState.GameStateChangedEvent -= OnGameStateChanged;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out Enemy _)) return;
        
        SurvivorHit();
        _soundController.SurvivorHitSound();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out Enemy enemy)) return;
        if (enemy is Lava) _soundController.LavaSplashSound();
        
        SurvivorHit();
    }

    private void SurvivorHit()
    {
        _spriteRenderer.sprite = _deadSprite;
        SurvivorHitEvent?.Invoke();
    }

    private void OnGameStateChanged()
    {
        if (_gameState.CurrentGameState == GameStateEnum.Play)
        {
            _rigidbody.gravityScale = 1;
        }

        if (_gameState.CurrentGameState == GameStateEnum.Win)
        {
            _rigidbody.simulated = false;
        }
    }
}
