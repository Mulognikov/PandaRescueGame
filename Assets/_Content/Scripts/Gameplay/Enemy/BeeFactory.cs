using System.Collections;
using UnityEngine;
using Zenject;

public class BeeFactory : MonoBehaviour
{
    [SerializeField] private int _beesCount = 10;
    [SerializeField] private float _spawnDelay = 0.1f;

    private GameStateModel _gameState;
    private DiContainer _diContainer;
    private Bee _beePrefab;
    private Bee[] _beesPool;

    [Inject]
    private void Construct(GameStateModel gameState, DiContainer diContainer, Bee bee)
    {
        _gameState = gameState;
        _diContainer = diContainer;
        _beePrefab = bee;
    }
    
    private void Awake()
    {  
        Create();
    }

    private void OnEnable()
    {
        _gameState.GameStateChangedEvent += OnGameStateChanged;
    }

    private void OnDisable()
    {
        _gameState.GameStateChangedEvent -= OnGameStateChanged;
    }

    private void Create()
    {
        _beesPool = new Bee[_beesCount];

        for (int i = 0; i < _beesCount; i++)
        {
            _beesPool[i] = _diContainer.InstantiatePrefab(_beePrefab, transform).GetComponent<Bee>();
            _beesPool[i].gameObject.SetActive(false);
        }
    }
    
    private void OnGameStateChanged()
    {
        if (_gameState.CurrentGameState == GameStateEnum.Play)
        {
            StartCoroutine(SpawnBeesCoroutine());
        }
    }

    private IEnumerator SpawnBeesCoroutine()
    {
        for (int i = 0; i < _beesCount; i++)
        {
            _beesPool[i].gameObject.SetActive(true);
            _beesPool[i].StartMove();
            yield return new WaitForSeconds(_spawnDelay);
        }
    }
}
