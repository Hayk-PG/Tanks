﻿using System;
using UnityEngine;

[Serializable]
public class BaseTankMovement : MonoBehaviour
{
    [Header("Movement parameters")] [Range(0, 1000)] 
    public float _normalSpeed;
    [Range(0, 1000)]
    public float _maxBrake;
    [Range(0, 3000)]
    public float _accelerated;
    [Range(0, 100)]
    public int _damageFactor;
    public Vector3 _normalCenterOfMass;
    protected float _currentBrake;
    protected float _currentSpeed;

    protected Rigidbody _rigidBody;
    protected PlayerTurn _playerTurn;
    protected HealthController _healthController;
    protected WheelColliderController _wheelColliderController;   
    protected Raycasts _rayCasts;
    protected VehicleRigidbodyPosition _vehicleRigidbodyPosition;
    protected Stun _stun;

    protected bool _isOnRightSlope, _isOnLeftSlope, _isStunned;
    protected string[] _slopesNames;
    protected Vector3 _vectorRight;
    protected Vector3 _vectorLeft;

    public virtual int DamageFactor
    {
        get
        {
            return (_healthController.Health + _damageFactor) >= 100 ? 100 : _healthController.Health + _damageFactor;
        }
    }
    public virtual float Speed
    {
        get => _currentSpeed;
        set => _currentSpeed = value / 100 * DamageFactor;
    }
    public virtual float Direction { get; set; }
    protected float RotationXAxis { get; set; }
    protected float InitialRotationYAxis { get; set; }   
    protected float _speedBlocker;

    internal Action<float> OnVehicleMove { get; set; }
    public Action<float> OnDirectionValue { get; set; }
    public Action<Rigidbody> OnRigidbodyPosition { get; set; }
    public Action<float, bool> OnFuel { get; set; }


    protected virtual void Awake()
    {
        _rigidBody = Get<Rigidbody>.From(gameObject);
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _healthController = Get<HealthController>.From(gameObject);
        _wheelColliderController = Get<WheelColliderController>.FromChild(gameObject);
        _rayCasts = Get<Raycasts>.FromChild(gameObject);
        _vehicleRigidbodyPosition = Get<VehicleRigidbodyPosition>.From(gameObject);
        _stun = Get<Stun>.FromChild(gameObject);

        Initialize();
    }

    protected virtual void OnEnable()
    {
        _vehicleRigidbodyPosition.OnAllowingPlayerToMoveOnlyFromLeftToRight += OnAllowingPlayerToMoveOnlyFromLeftToRight;

        if (_stun != null)
            _stun.OnStunEffect += OnStunEffect;
    }

    protected virtual void OnDisable()
    {
        _vehicleRigidbodyPosition.OnAllowingPlayerToMoveOnlyFromLeftToRight -= OnAllowingPlayerToMoveOnlyFromLeftToRight;

        if (_stun != null)
            _stun.OnStunEffect -= OnStunEffect;
    }

    void Initialize()
    {
        InitialRotationYAxis = _rigidBody.position.x < 0 ? 90 : -90;

        string right = InitialRotationYAxis > 0 ? "RS" : "LS";
        string left = InitialRotationYAxis > 0 ? "LS" : "RS";
        _slopesNames = new string[2] { right, left };

        _vectorRight = InitialRotationYAxis > 0 ? Vector3.right : Vector3.left;
        _vectorLeft = InitialRotationYAxis > 0 ? Vector3.left : Vector3.right;
    }

    protected virtual void OnAllowingPlayerToMoveOnlyFromLeftToRight(bool? mustMoveFromLeftToRightOnly)
    {

    }

    protected virtual void OnStunEffect(bool isStunned)
    {
        _isStunned = isStunned;
    }
}
