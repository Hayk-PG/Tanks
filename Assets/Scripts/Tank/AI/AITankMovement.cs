using UnityEngine;

public class AITankMovement : BaseTankMovement
{
    [Range(-1,1)]
    [SerializeField] int direction;

    public override float Direction => direction;


    protected override void Awake()
    {
        base.Awake();

        RigidbodyCenterOfMass();
    }

    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        _wheelColliderController.MotorTorque(Direction * Speed * Time.fixedDeltaTime);
        _wheelColliderController.RotateWheels();

        Raycasts();
        UpdateSpeedAndPush();
        Brake();
        RigidbodyTransform();
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

    bool IsVehicleStopped(float value)
    {
        return value == 0;
    }
}
