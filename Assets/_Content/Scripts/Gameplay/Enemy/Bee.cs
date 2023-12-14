using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Bee : Enemy
{
    public static event Action BeeSpawnEvent;
    public static event Action BeeDestroyEvent;
    
    [SerializeField] private float _directionChangeTime = 0.15f;
    [SerializeField] private float _distanceToStartChangingDirection = 1.5f;

    private SpriteRenderer _spriteRenderer;
    private NavMeshAgent _agent;
    private Rigidbody2D _rigidbody;
    private Transform _pursuedDoge;
    private SoundController _soundController;
    
    private int _randDirectionMin = -1;
    private int _randDirectionMax = 2;
    private int _changeDirectionChance = 30;

    private float _timeLeftToDirectionChange;
    private Vector3 _lastFixedPosition;

    private Coroutine _beeMoveCoroutine;
    
    [Inject]
    private void Construct(Survivor[] survivors, SoundController soundController)
    {
        _pursuedDoge = survivors[UnityEngine.Random.Range(0, survivors.Length)].transform;
        _soundController = soundController;
    }
    
    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.updatePosition = false;
    }

    private void Start()
    {
        BeeSpawnEvent?.Invoke();
    }

    private void OnDestroy()
    {
        BeeDestroyEvent?.Invoke();
    }

    private void FixedUpdate()
    {
        FlipSpriteToMoveDirection();
        
        if (_gameState.CurrentGameState == GameStateEnum.Play)
        {
            Move();
        }

        if (_gameState.CurrentGameState == GameStateEnum.Lose)
        {
            Move();
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        
        if (collision.gameObject.TryGetComponent(out IBeeKiller _))
        {
            _soundController.BeeHitSound();
            _hitParticle.transform.position = transform.position;
            _hitParticle.Play();
            Destroy(gameObject);
        }
    }

    private void FlipSpriteToMoveDirection()
    {
        Vector3 direction = _agent.destination - transform.position;
        float dot = Vector3.Dot(direction, Vector3.left);

        _spriteRenderer.flipX = dot < 0;
    }

    private void Move()
    {
        _timeLeftToDirectionChange -= Time.fixedDeltaTime;

        if (_agent.hasPath && _agent.pathStatus != NavMeshPathStatus.PathInvalid)
        {
            _rigidbody.velocity = _agent.velocity;
        }
        else
        {
            _rigidbody.velocity = (_pursuedDoge.position - transform.position).normalized * _agent.speed;
        }
        
        _agent.nextPosition = transform.position;
        
    }

    protected override void StopGame()
    {
        _rigidbody.velocity = Vector2.zero;
        StopCoroutine(_beeMoveCoroutine);
    }

    protected override void SurvivorHit(Survivor survivor, Vector3 hitPosition)
    {
        base.SurvivorHit(survivor, hitPosition);
        _agent.destination = transform.position - survivor.transform.position;
        _timeLeftToDirectionChange = _directionChangeTime;
    }

    public void StartMove()
    {
        if (_gameState.CurrentGameState == GameStateEnum.Play)
        {
            _beeMoveCoroutine = StartCoroutine(BeeMoveCoroutine());
        }
    }

    private IEnumerator BeeMoveCoroutine()
    {
        _timeLeftToDirectionChange = _directionChangeTime;
        
        Vector3 newDestination = new Vector3(UnityEngine.Random.Range(_randDirectionMin, _randDirectionMax), UnityEngine.Random.Range(_randDirectionMin, _randDirectionMax));
        _agent.destination = transform.position + newDestination;
        
        while (true)
        {
            yield return new WaitWhile(() => _timeLeftToDirectionChange > 0);
            _timeLeftToDirectionChange = _directionChangeTime;
            
            if (UnityEngine.Random.Range(0, 100) < _changeDirectionChance && Vector3.Distance(_pursuedDoge.position, transform.position) < _distanceToStartChangingDirection)
            {
                newDestination = new Vector3(UnityEngine.Random.Range(_randDirectionMin, _randDirectionMax), UnityEngine.Random.Range(_randDirectionMin, _randDirectionMax));
                _agent.destination = transform.position + newDestination;
            }
            else
            {
                _agent.destination = _pursuedDoge.position;
            }
        }
    }
}
