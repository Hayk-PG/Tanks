using UnityEngine;

public class TankMovement : BaseTankMovement
{
    private TankController _tankController;
    private Fuel _fuel;
    private bool _isEnoughFuel = true;
    private bool _isSandbagsTriggered;
    public float SpeedInSandbags
    {
        get
        {
            if (_isSandbagsTriggered)
            {
                if (_playerTurn.MyTurn == TurnState.Player1 && Direction > 0 || _playerTurn.MyTurn == TurnState.Player2 && Direction < 0)
                    return 0;
                else
                    return 1;
            }
            else
                return 1;
        } 
    }


    protected override void Awake()
    {
        base.Awake();      
        RigidbodyCenterOfMass();
        _tankController = Get<TankController>.From(gameObject);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _tankController.OnHorizontalJoystick += OnHorizontalJoystick;
        if (_fuel != null) _fuel.OnFuelValue -= OnFuelValue;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _tankController.OnHorizontalJoystick -= OnHorizontalJoystick;
    }

    private void OnHorizontalJoystick(float horizontal)
    {
        if (_playerTurn.IsMyTurn && _isEnoughFuel)
            Direction = name == Names.Tank_FirstPlayer ? horizontal : -horizontal;
        else
            Direction = 0;
    }

    private void FixedUpdate()
    {
        Move();      
    }

    public void Move()
    {
        MotorTorque();
        Brake();
        RigidbodyEulerAngles();

        if (_playerTurn.IsMyTurn)
        {
            Raycasts();
            UpdateSpeedAndPush();
        }
    }

    private void MotorTorque()
    {
        _wheelColliderController.MotorTorque(Direction * Speed * SpeedInSandbags * _speedBlocker * Time.fixedDeltaTime);
        _wheelColliderController.RotateWheels();

        OnVehicleMove?.Invoke(_wheelColliderController.WheelRPM());
        OnDirectionValue?.Invoke(Direction);
        OnFuel?.Invoke(_wheelColliderController.WheelRPM(), _playerTurn.IsMyTurn);
        OnRigidbodyPosition?.Invoke(_rigidBody);       
    }

    private void UpdateSpeedAndPush()
    {
        _isOnRightSlope = Direction > 0.5f && _rayCasts.FrontHit.collider?.name == _slopesNames[0];
        _isOnLeftSlope = Direction < -0.5f && _rayCasts.BackHit.collider?.name == _slopesNames[1];

        if (_isOnRightSlope) RigidbodyPush(_vectorRight, 2500);
        if (_isOnLeftSlope) RigidbodyPush(_vectorLeft, 2500);

        if (_isOnRightSlope || _isOnLeftSlope) Speed = _accelerated;
        else Speed = _normalSpeed;
    }

    private void Brake()
    {
        _currentBrake = IsVehicleStopped(Direction) ? _maxBrake : 0;

        _wheelColliderController.BrakeTorque(_currentBrake);
    }

    private void RigidbodyEulerAngles()
    {
        RotationXAxis = Converter.AngleConverter(_rigidBody.transform.eulerAngles.x) >= 65 || Converter.AngleConverter(_rigidBody.transform.eulerAngles.x) <= -65 ?
                        0 : _rigidBody.transform.eulerAngles.x;

        _rigidBody.transform.eulerAngles = new Vector3(RotationXAxis, InitialRotationYAxis, 0);
    }

    private void RigidbodyCenterOfMass()
    {
        _rigidBody.centerOfMass = _normalCenterOfMass;
    }

    private void RigidbodyPush(Vector3 direction, float force)
    {
        _rigidBody.AddForce(direction * force * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    private void Raycasts()
    {
        _rayCasts.CastRays(_vectorRight, _vectorLeft);
    }

    private bool IsVehicleStopped(float value)
    {
        return value == 0;
    }

    internal void SubscribeToFuelEvents(Fuel fuel)
    {
        _fuel = fuel;
        _fuel.OnFuelValue += OnFuelValue;
    }

    private void OnFuelValue(bool isEnoughFuel)
    {
        _isEnoughFuel = isEnoughFuel;
    }

    public void OnEnteredSandbagsTrigger(bool isEntered)
    {
        _isSandbagsTriggered = isEntered;
    }

    protected override void OnAllowingPlayerToMoveOnlyFromLeftToRight(bool? mustMoveFromLeftToRightOnly)
    {
        if (mustMoveFromLeftToRightOnly.HasValue && mustMoveFromLeftToRightOnly.Value == true && _playerTurn.MyTurn == TurnState.Player2)
        {
            if (Direction > 0)
                _speedBlocker = 0;
            else
                _speedBlocker = 1;
        }

        if (mustMoveFromLeftToRightOnly.HasValue && mustMoveFromLeftToRightOnly.Value == false && _playerTurn.MyTurn == TurnState.Player1)
        {
            if (Direction > 0)
                _speedBlocker = 0;
            else
                _speedBlocker = 1;
        }

        if (!mustMoveFromLeftToRightOnly.HasValue)
            _speedBlocker = 1;
    }
}
