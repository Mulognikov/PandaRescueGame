using UnityEngine;
using Zenject;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private float _hitForce = 10f;
    public float HitForce => _hitForce;
    
    protected GameStateModel _gameState;
    private ParticleSystem _hitParticlePrefab;
    protected ParticleSystem _hitParticle;

    [Inject]
    protected virtual void Construct(GameStateModel gameState, ParticleSystem hitParticle)
    {
        _gameState = gameState;
        _hitParticlePrefab = hitParticle;
    }

    protected virtual void Awake()
    {
        _hitParticle = Instantiate(_hitParticlePrefab);
    }

    private void OnEnable()
    {
        _gameState.GameStateChangedEvent += OnGameStateChange;
    }

    private void OnDisable()
    {
        _gameState.GameStateChangedEvent -= OnGameStateChange;
    }
    
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Survivor survivor))
        {
            SurvivorHit(survivor, collision.contacts[0].point);
        }
        
        if (collision.gameObject.TryGetComponent(out Rigidbody2D rigidbody))
        {
            RigidbodyHit(rigidbody);
        }
    }

    protected virtual void OnGameStateChange()
    {
        if (_gameState.CurrentGameState == GameStateEnum.Play) StartGame();
        if (_gameState.CurrentGameState == GameStateEnum.Win) StopGame();
    }

    protected virtual void StartGame() { }

    protected virtual void StopGame() { }

    protected virtual void SurvivorHit(Survivor survivor, Vector3 hitPosition)
    {
        _hitParticle.transform.position = hitPosition;
        _hitParticle.Play();
    }

    protected virtual void RigidbodyHit(Rigidbody2D rigidbody)
    {
        rigidbody.AddForce((rigidbody.transform.position - transform.position) * HitForce, ForceMode2D.Impulse);
    }

}
