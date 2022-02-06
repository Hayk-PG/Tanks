using UnityEngine;

public class TankMovement : BaseTankMovement
{
    private FixedJoystick _joystick;


    protected override void Awake()
    {
        base.Awake();

        _joystick = GameObject.Find(Names.HorizontalJoystick).GetComponent<FixedJoystick>();

        RigidbodyCenterOfMass();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        MotorTorque();
        Brake();
        RigidbodyTransform();

        if (_playerTurn.IsMyTurn)
        {
            Raycasts();
            UpdateSpeedAndPush();
            Direction = _joystick.Horizontal;
        }
        else
        {
            Direction = 0;
        }
    }

    private void MotorTorque()
    {
        _wheelColliderController.MotorTorque(Direction * Speed * Time.fixedDeltaTime);
        _wheelColliderController.RotateWheels();
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

    private void RigidbodyTransform()
    {
        _rigidBody.transform.eulerAngles = new Vector3(_rigidBody.transform.eulerAngles.x, InitialRotationYAxis, 0);
        _rigidBody.position = new Vector3(_rigidBody.transform.position.x, _rigidBody.transform.position.y, 0);
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
}
