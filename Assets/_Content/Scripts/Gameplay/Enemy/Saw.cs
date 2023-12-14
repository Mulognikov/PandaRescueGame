using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Zenject;

public class Saw : Enemy, IBeeKiller
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private LineRenderer _line;

    [Space]
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private List<Vector3> _movePoints;
    [SerializeField] private bool _loop;

    private SoundController _soundController;
    
    private const float _rotationSpeed = 25f;
    private const float _sawStartStopTime = 1f;
    private float _currentRotationSpeed = 0f;
    private int _targetPoint;
    private bool _reverse;
    private TweenerCore<Vector3, Vector3, VectorOptions> _tween;

    [Inject]
    private void Construct(SoundController soundController)
    {
        _soundController = soundController;
    }

    private void Update()
    {
        _sprite.transform.Rotate(Vector3.forward, _currentRotationSpeed);
    }

    private void OnValidate()
    {
        Validate();
    }

    private void Validate()
    {
        if (_movePoints.Count == 0)
        {
            _movePoints.Insert(0, transform.position);
        }

        if (_movePoints.Count > 0 && _movePoints[0] != transform.position)
        {
            _movePoints[0] = transform.position;
        }

        _line.positionCount = _movePoints.Count;
        _line.SetPositions(_movePoints.ToArray());
        _line.loop = _loop;
    }

    protected override void StartGame()
    {
        Validate();
        DOTween.To(() => _currentRotationSpeed, x => _currentRotationSpeed = x, _rotationSpeed, _sawStartStopTime);

        if (_movePoints.Count < 2) return;

        _targetPoint = 1;
        MoveNext();
        _soundController.SawStart();
    }

    protected override void StopGame()
    {
        _soundController.SawStop();
        _tween.Kill();
        DOTween.To(()=> _currentRotationSpeed, x=> _currentRotationSpeed = x, 0, _sawStartStopTime);
    }
    

    private void MoveNext()
    {
        float moveTime = Vector3.Distance(transform.position, _movePoints[_targetPoint]) / _moveSpeed;
        _tween = transform.DOMove(_movePoints[_targetPoint], moveTime).SetEase(Ease.Linear).OnComplete(MoveNext).SetUpdate(UpdateType.Fixed);
        
        if (_reverse)
        {
            _targetPoint--;
            
            if (_targetPoint == -1)
            {
                _reverse = false;
                _targetPoint = 1;
            }
        }
        else
        {
            _targetPoint++;
            
            if (_targetPoint == _movePoints.Count)
            {
                _reverse = true;
                _targetPoint = _movePoints.Count - 2;
                
                if (_loop)
                {
                    _targetPoint = 0;
                    _reverse = false;
                }
            }
        }
    }
}
