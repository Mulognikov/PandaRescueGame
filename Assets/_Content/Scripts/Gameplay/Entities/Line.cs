using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Zenject;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Line : MonoBehaviour
{
    public event Action DrawEndEvent;
    
    [SerializeField] private LayerMask _notDrawLayers;
    [SerializeField] private LineRenderer _wrongLine;
    
    private LineRenderer _lineRenderer;
    private EdgeCollider2D _collider;
    private Rigidbody2D _rigidbody;
    private GameStateModel _gameState;
    private Camera _camera;
    private GameSettings _gameSettings;
    private NavMeshObstacle _obstaclePrefab;
    
    private List<Vector2> _points = new();
    private NavMeshObstacle[] _obstaclePool;
    private bool _disableDraw = false;
    private Vector2 _massCenter = Vector2.zero;

    public float CurrentLineLength { get; private set; }


    [Inject]
    public void Construct(GameStateModel gameState, Camera camera, GameSettings gameSettings, NavMeshObstacle _obstacle)
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
        
        CreateObstaclePool();
    }

    private void OnEnable()
    {
        _gameState.GameStateChangedEvent += SimulateLine;
    }

    private void OnDisable()
    {
        _gameState.GameStateChangedEvent -= SimulateLine;
    }


    private void Update()
    {
        if (_gameState.CurrentGameState == GameStateEnum.Draw)
        {
            DrawLine();
        }

        if (_gameState.CurrentGameState == GameStateEnum.Play)
        {
            DisableEndLineObstaclesWhenOnGround();
        }
    }

    private void DrawLine()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
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

        _points.Add(mousePos);
 
        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1,mousePos);
 
        _collider.points = _points.ToArray();
        _massCenter += mousePos;

        if (_points.Count > 1)
        {
            CurrentLineLength += Vector2.Distance(_points[^1], _points[^2]);
        }

        if (_points.Count > 1)
        {
            Vector2 lastDirection = _points[^1] - _points[^2];
            float angle = Vector2.SignedAngle(Vector2.right, lastDirection);
            float distance = Vector2.Distance(_points[^1], _points[^2]);

            NavMeshObstacle currentObstacle = _obstaclePool[_points.Count];
        
            currentObstacle.transform.position = _points[^1];
            currentObstacle.transform.eulerAngles = Vector3.forward * angle;
            currentObstacle.size = new Vector3(distance, _collider.edgeRadius * 2, _collider.edgeRadius * 2);
            currentObstacle.center = new Vector3(-distance/ 2, 0, 0);
            currentObstacle.enabled = true;
        }
    }

    private bool CanAppend(Vector2 pos)
    {
        if (CurrentLineLength >= _gameSettings.MaxLineLength) return false;
        if (_lineRenderer.positionCount == 0) return true;

        Vector2 lastPoint = _lineRenderer.GetPosition(_lineRenderer.positionCount - 1);
        float distance = Vector2.Distance(lastPoint, pos);

        if (Physics2D.Linecast(lastPoint, pos, _notDrawLayers))
        {
            _disableDraw = true;
            ShowWrongLine(lastPoint, pos);
            return false;
        }

        if (_disableDraw)
        {
            ShowWrongLine(lastPoint, pos);
        }
        else
        {
            return distance > _gameSettings.MinDrawDistance;
        }
        
        if (distance > _gameSettings.MinDrawDistance && distance < 1f)
        {
            _wrongLine.gameObject.SetActive(false);
            _disableDraw = false;
            return true;
        }

        return false;
    }

    private void ShowWrongLine(Vector2 start, Vector2 end)
    {
        if (_points.Count < 2) return;
        
        _wrongLine.transform.position = start;
        _wrongLine.SetPosition(0, start);
        _wrongLine.SetPosition(1, end);
        _wrongLine.gameObject.SetActive(true);
    }

    private void DrawEnd()
    {
        if (_points.Count < 2)
        {
            _points.Clear();
            _lineRenderer.positionCount = 0;
            return;
        }
        
        _wrongLine.gameObject.SetActive(false);
        _rigidbody.centerOfMass = _massCenter / _collider.points.Length;
        DrawEndEvent?.Invoke();
    }

    private void SimulateLine()
    {
        _rigidbody.simulated = _gameState.CurrentGameState is GameStateEnum.Play or GameStateEnum.Lose;
    }

    private void DisableEndLineObstaclesWhenOnGround()
    {
        bool start = !(Physics2D.CircleCastAll(_obstaclePool[2].transform.position, 0.25f, Vector2.zero, 0, 1).Length > 0);
        bool end = !(Physics2D.CircleCastAll(_obstaclePool[_points.Count].transform.position, 0.25f, Vector2.zero, 0, 1).Length > 0);

		_obstaclePool[2].enabled = start;
		_obstaclePool[3].enabled = start;

		_obstaclePool[_points.Count].enabled = end;
		_obstaclePool[_points.Count - 1].enabled = end;
	}

    private void CreateObstaclePool()
    {
        int obstaclesCount = (int)(_gameSettings.MaxLineLength / _gameSettings.MinDrawDistance) + 1;
        _obstaclePool = new NavMeshObstacle[obstaclesCount];
        
        for (int i = 0; i < obstaclesCount; i++)
        {
            _obstaclePool[i] = Instantiate(_obstaclePrefab, Vector3.zero, Quaternion.identity);
            _obstaclePool[i].transform.parent = transform;
            _obstaclePool[i].enabled = false;
        }
    }
}
