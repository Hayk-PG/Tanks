using System;
using UnityEngine;

public class AITankMovement : BaseTankMovement
{      
    private bool IsDestinationReachedForwards => Direction > 0 && transform.position.x <= _destination.x;
    private bool IsDestinationReachedBackwards => Direction < 0 && transform.position.x > _destination.x;

    private Vector3 _destination;

    private AIActionPlanner _aiActionPlanner;

    public event Action Shoot;


    protected override void Awake()
    {
        base.Awake();

        _aiActionPlanner = GetComponent<AIActionPlanner>();

        RigidbodyCenterOfMass();
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

        if(arg2 == 0)
        {
            Shoot?.Invoke();
        }
    }

    public void Move()
    {
        _wheelColliderController.MotorTorque(Direction * Speed * Time.fixedDeltaTime);
        _wheelColliderController.RotateWheels();

        OnVehicleMove?.Invoke(_wheelColliderController.WheelRPM());

        Brake();
        RigidbodyTransform();
        OnDestinationReached();

        if (_playerTurn.IsMyTurn)
        {
            Raycasts();
            UpdateSpeedAndPush();
        }
        else
        {
            Direction = 0;
        }
    }
    private void OnDestinationReached()
    {
        if(IsDestinationReachedForwards || IsDestinationReachedBackwards)
        {
            if (Direction != 0)
            {
                Direction = 0;
                Shoot?.Invoke();
            }
        }
    }

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

    public void RigidbodyTransform()
    {
        _rigidBody.transform.eulerAngles = new Vector3(_rigidBody.transform.eulerAngles.x, InitialRotationYAxis, 0);
        _rigidBody.position = new Vector3(_rigidBody.transform.position.x, _rigidBody.transform.position.y, 0);
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
