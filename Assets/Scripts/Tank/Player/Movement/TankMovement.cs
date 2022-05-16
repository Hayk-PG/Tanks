using UnityEngine;

public class TankMovement : BaseTankMovement
{
    private TankController _tankController;
    private Fuel _fuel;
    private bool _isEnoughFuel = true;


    protected override void Awake()
    {
        base.Awake();      
        RigidbodyCenterOfMass();
        _tankController = Get<TankController>.From(gameObject);
    }

    private void OnEnable()
    {
        _tankController.OnHorizontalJoystick += OnHorizontalJoystick;
        if (_fuel != null) _fuel.OnFuelValue -= OnFuelValue;
    }

    private void OnDisable()
    {
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
        _wheelColliderController.MotorTorque(Direction * Speed * Time.fixedDeltaTime);
        _wheelColliderController.RotateWheels();

        OnVehicleMove?.Invoke(_wheelColliderController.WheelRPM());
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
        _rigidBody.transform.eulerAngles = new Vector3(_rigidBody.transform.eulerAngles.x, InitialRotationYAxis, 0);
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
}
