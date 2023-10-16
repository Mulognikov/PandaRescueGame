using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Survivor : MonoBehaviour
{
    [SerializeField] private Sprite _deadSprite;
    
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private GameState _gameState;

    [Inject]
    private void Construct(GameState gameState)
    {
        _gameState = gameState;
    }
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        _gameState.GameStateChangedEvent += OnGameStateChanged;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out Enemy enemy)) return;
        
        _rigidbody.AddForce((transform.position - collision.transform.position) * enemy.HitForce, ForceMode2D.Impulse);
        DogeHit();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out Enemy enemy)) return;
        
        DogeHit();
    }

    private void DogeHit()
    {
        _spriteRenderer.sprite = _deadSprite;
        _gameState.DogeHit();
    }

    private void OnGameStateChanged()
    {
        if (_gameState.CurrentGameState == GameState.GameStateEnum.Play)
        {
            _rigidbody.gravityScale = 1;
        }

        if (_gameState.CurrentGameState == GameState.GameStateEnum.Win)
        {
            _rigidbody.simulated = false;
        }
    }
}
