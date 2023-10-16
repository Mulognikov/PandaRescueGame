using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Bee : Enemy
{
    [SerializeField] private float _directionChangeTime = 0.15f;
    [SerializeField] private float _distanceToStartChangingDirection = 1.5f;

    private SpriteRenderer _spriteRenderer;
    private NavMeshAgent _agent;
    private Rigidbody2D _rigidbody;
    private Transform _pursuedDoge;
    
    private int _randDirectionMin = -1;
    private int _randDirectionMax = 2;
    private int _changeDirectionChance = 30;

    private float _timeLeftToDirectionChange;
    private Vector3 lastFixedPosition;

    private Coroutine _beeMoveCoroutine;
    
    [Inject]
    private void Construct(Survivor[] doges)
    {
        _pursuedDoge = doges[UnityEngine.Random.Range(0, doges.Length)].transform;
    }
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.updatePosition = false;
    }

    private void FixedUpdate()
    {
        FlipSpriteToMoveDirection();
        
        if (_gameState.CurrentGameState == GameState.GameStateEnum.Play)
        {
            Move();
        }

        if (_gameState.CurrentGameState == GameState.GameStateEnum.Lose)
        {
            Move();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Survivor doge))
        {
            _agent.destination = transform.position - doge.transform.position;
            _timeLeftToDirectionChange = _directionChangeTime;
        }
        
        if (collision.gameObject.TryGetComponent(out Rigidbody2D rigidbody))
        {
            rigidbody.AddForce((rigidbody.transform.position - transform.position) * HitForce, ForceMode2D.Impulse);
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

        if (_agent.hasPath)
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

    public void StartMove()
    {
        if (_gameState.CurrentGameState == GameState.GameStateEnum.Play)
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
