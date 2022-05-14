﻿using System;
using UnityEngine;

public class AITankMovement : BaseTankMovement
{      
    private bool IsDestinationReachedForwards => Direction > 0 && transform.position.x <= _destination.x;
    private bool IsDestinationReachedBackwards => Direction < 0 && transform.position.x > _destination.x;

    private Vector3 _destination;

    private AIActionPlanner _aiActionPlanner;

    public event Action Shoot;

    private float _stuckTime;


    protected override void Awake()
    {
        base.Awake();
        _aiActionPlanner = GetComponent<AIActionPlanner>();
        RigidbodyCenterOfMass();
    }

    private void Start()
    {
        OnLocalDelegatesSubscription();
    }

    private void OnEnable()
    {
        _aiActionPlanner.OnActionPlanner += OnGetDestination;
    }
   
    private void OnDisable()
    {
        _aiActionPlanner.OnActionPlanner -= OnGetDestination;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnGetDestination(Vector3 arg1, int arg2)
    {
        _destination = arg1;
        Direction = arg2;

        if(arg2 == 0) Shoot?.Invoke();

        ResetStuckTime();
    }

    public void Move()
    {
        _wheelColliderController.MotorTorque(Direction * Speed * Time.fixedDeltaTime);
        _wheelColliderController.RotateWheels();

        OnVehicleMove?.Invoke(_wheelColliderController.WheelRPM());
        OnRigidbodyPosition?.Invoke(_rigidBody);

        Brake();
        RigidbodyEulerAngles();
        OnDestinationReached();
        OnStuck();

        if (_playerTurn.IsMyTurn)
        {
            Raycasts();
            UpdateSpeedAndPush();
        }
        else
        {
            ResetDirection();
        }
    }

    private delegate void Stuck();
    private Stuck _OnResets;
    private Stuck _OnStuck;
    private Stuck _OnStuckTimeEnded;
    private Stuck _OnDestinationReached;

    private delegate bool Checker();
    private Checker _isDestinationReached;
    private Checker _isVehicleStuck;
    private Checker _isStuckTimeEnded;

    private void OnLocalDelegatesSubscription()
    {
        _OnResets += ResetDirection;
        _OnResets += ResetStuckTime;
        _OnStuck = delegate { Conditions<bool>.Compare(_isVehicleStuck(), () => _stuckTime += Time.deltaTime, null); };
        _OnStuckTimeEnded = delegate { _OnResets?.Invoke(); Shoot?.Invoke(); };
        _OnDestinationReached = delegate { ResetDirection(); Shoot?.Invoke(); };

        _isDestinationReached = delegate { return IsDestinationReachedForwards || IsDestinationReachedBackwards; };
        _isVehicleStuck = delegate { return _rigidBody.velocity.x >= -0.3 && _rigidBody.velocity.x <= 0.3f; };
        _isStuckTimeEnded = delegate { return _stuckTime >= 3; };
    }

    private void OnDestinationReached()
    {
        Conditions<bool>.Compare(_isDestinationReached() && Direction != 0, () => _OnDestinationReached?.Invoke(), null);
    }
   
    private void OnStuck()
    {
        Conditions<bool>.Compare(!_isDestinationReached() && Direction != 0, ()=> _OnStuck?.Invoke(), null);
        Conditions<bool>.Compare(_isStuckTimeEnded(), () => _OnStuckTimeEnded?.Invoke(), null);
    }

    private void ResetStuckTime() => _stuckTime = 0;

    private void ResetDirection() => Direction = 0;

    public void UpdateSpeedAndPush()
    {
        _isOnRightSlope = Direction > 0.5f && _rayCasts.FrontHit.collider?.name == _slopesNames[0];
        _isOnLeftSlope = Direction < -0.5f && _rayCasts.BackHit.collider?.name == _slopesNames[1];

        if (_isOnRightSlope) RigidbodyPush(_vectorRight, 2500);
        if (_isOnLeftSlope) RigidbodyPush(_vectorLeft, 2500);

        if (_isOnRightSlope || _isOnLeftSlope) Speed = _accelerated;
        else Speed = _normalSpeed;
    }

    public void Brake()
    {
        _currentBrake = IsVehicleStopped(Direction) ? _maxBrake : 0;

        _wheelColliderController.BrakeTorque(_currentBrake);
    }

    public void RigidbodyEulerAngles()
    {
        _rigidBody.transform.eulerAngles = new Vector3(_rigidBody.transform.eulerAngles.x, InitialRotationYAxis, 0);
    }

    public void RigidbodyCenterOfMass()
    {
        _rigidBody.centerOfMass = _normalCenterOfMass;
    }

    public void RigidbodyPush(Vector3 direction, float force)
    {
        _rigidBody.AddForce(direction * force * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    public void Raycasts()
    {
        _rayCasts.CastRays(_vectorRight, _vectorLeft);
    }

    private bool IsVehicleStopped(float value) 
    {
        return value == 0;
    }
}
