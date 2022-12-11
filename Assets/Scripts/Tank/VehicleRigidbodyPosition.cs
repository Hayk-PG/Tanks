using System;
using UnityEngine;

public class VehicleRigidbodyPosition : MonoBehaviour
{
    private BaseTankMovement _baseTankMovement;
    private TankController _tankController;
    private PlayerTurn _playerTurn;
    private MapPoints _mapPoints;
    private GameManager _gameManager;

    public delegate bool Checker(Rigidbody rigidBody);
    public Checker IsPositionOutsideOfMinBoundaries { get; private set; }
    public Checker IsPositionOutsideOfMaxBoundaries { get; private set; }
    public Checker IsPositionInsideOfBoundaries { get; private set; }

    private float _xPositionMinLimit, _xPositionMaxLimit;
    private float _newPositionMinLimit, _newPositionMaxLimit;
    private bool _isNewPositionLimitSet;

    public float MinX
    {
        get => _xPositionMinLimit;
        private set => _xPositionMinLimit = value;
    }
    public float MaxX
    {
        get => _xPositionMaxLimit;
        private set => _xPositionMaxLimit = value;
    }

    public Action<bool?> OnAllowingPlayerToMoveOnlyFromLeftToRight { get; set; }


    private void Awake()
    {
        _baseTankMovement = Get<BaseTankMovement>.From(gameObject);
        _tankController = Get<TankController>.From(gameObject);
        _playerTurn = Get<PlayerTurn>.From(gameObject);      
        _mapPoints = FindObjectOfType<MapPoints>();
        _gameManager = FindObjectOfType<GameManager>();

        _xPositionMinLimit = _mapPoints.HorizontalMin;
        _xPositionMaxLimit = _mapPoints.HorizontalMax;

        IsPositionOutsideOfMinBoundaries = delegate (Rigidbody rb) { return rb.position.x <= _xPositionMinLimit; };
        IsPositionOutsideOfMaxBoundaries = delegate (Rigidbody rb) { return rb.position.x >= _xPositionMaxLimit; };
        IsPositionInsideOfBoundaries = delegate (Rigidbody rb) { return rb.position.x > _xPositionMinLimit && rb.position.x < _xPositionMaxLimit; };
    }

    private void OnEnable()
    {
        if (_baseTankMovement != null)
            _baseTankMovement.OnRigidbodyPosition += OnRigidbodyPosition;
    }

    private void OnDisable()
    {
        if (_baseTankMovement != null)
            _baseTankMovement.OnRigidbodyPosition -= OnRigidbodyPosition; ;
    }

    private void OnRigidbodyPosition(Rigidbody rigidBody)
    {
        Conditions<bool>.Compare(_gameManager.Tank1 != null && _gameManager.Tank2 != null, ()=> UpdateLimits(rigidBody), null);
        Conditions<bool>.Compare(IsPositionOutsideOfMinBoundaries(rigidBody), () => XPositionLesser(rigidBody), null);
        Conditions<bool>.Compare(IsPositionOutsideOfMaxBoundaries(rigidBody), () => XPositionGreater(rigidBody), null);
        Conditions<bool>.Compare(IsPositionInsideOfBoundaries(rigidBody), () => AtAllowedPosition(rigidBody), null);
    }

    private void UpdateLimits(Rigidbody rigidBody)
    {
        Tank1Limits(rigidBody, 3);
        Tank2Limits(rigidBody, 3);
    }

    private void Tank1Limits(Rigidbody rigidBody, float value)
    {
        if (_gameManager.Tank1 == this._tankController)
        {
            if (_playerTurn.IsMyTurn) MaxX = _isNewPositionLimitSet ? _newPositionMaxLimit : _mapPoints.HorizontalMax;
            if (!_playerTurn.IsMyTurn)
            {
                if (rigidBody.position.x >= _gameManager.Tank2.transform.position.x - value) _newPositionMaxLimit = rigidBody.position.x;
                if (rigidBody.position.x < _gameManager.Tank2.transform.position.x - value) _newPositionMaxLimit = _gameManager.Tank2.transform.position.x - value;

                _isNewPositionLimitSet = true;
            }
        }
    }

    private void Tank2Limits(Rigidbody rigidBody, float value)
    {
        if (_gameManager.Tank2 == this._tankController)
        {
            if (_playerTurn.IsMyTurn) MinX = _isNewPositionLimitSet ? _newPositionMinLimit : _mapPoints.HorizontalMin;
            if (!_playerTurn.IsMyTurn)
            {
                if (rigidBody.position.x <= _gameManager.Tank1.transform.position.x + value) _newPositionMinLimit = rigidBody.position.x;
                if (rigidBody.position.x > _gameManager.Tank1.transform.position.x + value) _newPositionMinLimit = _gameManager.Tank1.transform.position.x + value;

                _isNewPositionLimitSet = true;
            }
        }
    }

    private void XPositionLesser(Rigidbody rigidBody)
    {
        rigidBody.position = new Vector3(_xPositionMinLimit, rigidBody.position.y, 0);
        rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
        OnAllowingPlayerToMoveOnlyFromLeftToRight?.Invoke(true);
    }

    private void XPositionGreater(Rigidbody rigidBody)
    {
        rigidBody.position = new Vector3(_xPositionMaxLimit, rigidBody.position.y, 0);
        rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
        OnAllowingPlayerToMoveOnlyFromLeftToRight?.Invoke(false);
    }

    private void AtAllowedPosition(Rigidbody rigidBody)
    {
        rigidBody.position = new Vector3(rigidBody.position.x, rigidBody.position.y, 0);
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, 0);
        OnAllowingPlayerToMoveOnlyFromLeftToRight?.Invoke(null);
    }
}
