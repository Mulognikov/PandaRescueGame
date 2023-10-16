using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Line : MonoBehaviour
{
    [SerializeField] private LayerMask notDrawLayers;
    
    private LineRenderer _lineRenderer;
    private EdgeCollider2D _collider;
    private Rigidbody2D _rigidbody;
    private GameState _gameState;
    private Camera _camera;
    private GameSettings _gameSettings;
    private NavMeshObstacle _obstaclePrefab;
    
    private List<Vector2> _points = new();
    private NavMeshObstacle[] _obstaclePool;
    private bool _disableDraw = false;
    private Vector2 _massCenter = Vector2.zero;

    public int DrawLeftPoints => _gameSettings.MaxDrawPoints - _points.Count;
    
    
    [Inject]
    public void Construct(GameState gameState, Camera camera, GameSettings gameSettings, NavMeshObstacle _obstacle)
    {
        _gameState = gameState;
        _camera = camera;
        _gameSettings = gameSettings;
        _obstaclePrefab = _obstacle;
    }
    
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _collider = GetComponent<EdgeCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.simulated = false;
        
        _gameState.GameStateChangedEvent += SimulateLine;
        
        CreateObstaclePool();
    }


    private void Update()
    {
        if (_gameState.CurrentGameState == GameState.GameStateEnum.Draw)
        {
            DrawLine();
        }
    }

    private void DrawLine()
    {
        if (Input.GetMouseButton(0))
        {
            AddPoint();
        }

        if (Input.GetMouseButtonUp(0))
        {
            DrawEnd();
        }
    }

    private void AddPoint() 
    {
        Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        
        if (!CanAppend(mousePos)) return;
        if (_points.Count > _gameSettings.MaxDrawPoints) return;

        _points.Add(mousePos);
 
        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1,mousePos);
 
        _collider.points = _points.ToArray();
        _massCenter += mousePos;

        if (_points.Count > 3 && _points.Count < _gameSettings.MaxDrawPoints)
        {
            Vector2 lastDirection = _points[^2] - _points[^3];
            float angle = Vector2.SignedAngle(Vector2.right, lastDirection);
            float distance = Vector2.Distance(_points[^2], _points[^3]);

            NavMeshObstacle currentObstacle = _obstaclePool[_points.Count];
        
            currentObstacle.transform.position = _points[^2];
            currentObstacle.transform.eulerAngles = Vector3.forward * angle;
            currentObstacle.size = new Vector3(distance, _collider.edgeRadius * 2, _collider.edgeRadius * 2);
            currentObstacle.center = new Vector3(-distance/ 2, 0, 0);
            currentObstacle.enabled = true;
        }
    }

    private bool CanAppend(Vector2 pos) 
    {
        if (_lineRenderer.positionCount == 0) return true;

        Vector2 lastPoint = _lineRenderer.GetPosition(_lineRenderer.positionCount - 1);
        float distance = Vector2.Distance(lastPoint, pos);
        
        if (Physics2D.Linecast(lastPoint, pos, notDrawLayers))
        {
            _disableDraw = true;
            return false;
        }

        if (!_disableDraw) return distance > _gameSettings.MinDrawDistance;
        
        _disableDraw = false;
        return distance > _gameSettings.MinDrawDistance && distance < _gameSettings.MinDrawDistance * 2f;

    }

    private void DrawEnd()
    {
        if (_points.Count < 2)
        {
            _points.Clear();
            _lineRenderer.positionCount = 0;
            return;
        }
        
        _rigidbody.centerOfMass = _massCenter / _collider.points.Length;
        _gameState.DrawEnd();
    }

    private void SimulateLine()
    {
        _rigidbody.simulated = _gameState.CurrentGameState is GameState.GameStateEnum.Play or GameState.GameStateEnum.Lose;
    }

    private void CreateObstaclePool()
    {
        int obstaclesCount = _gameSettings.MaxDrawPoints + 1;
        _obstaclePool = new NavMeshObstacle[obstaclesCount];
        
        for (int i = 0; i < obstaclesCount; i++)
        {
            _obstaclePool[i] = Instantiate(_obstaclePrefab, Vector3.zero, Quaternion.identity);
            _obstaclePool[i].transform.parent = transform;
            _obstaclePool[i].enabled = false;
        }
    }
}
