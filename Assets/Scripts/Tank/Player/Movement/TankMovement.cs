using UnityEngine;

public class TankMovement : MonoBehaviour
{
    [SerializeField] float _normalSpeed, accelerated;
    [SerializeField] float _currentBrake, _maxBrake;

    [SerializeField] Vector3 _normalCenterOfMass;

    [SerializeField] WheelColliderController _wheelColliderController;
    [SerializeField] Rigidbody rigidBody;

    FixedJoystick _joystick;

    public float Direction => _joystick.Horizontal;


    void Awake()
    {
        _joystick = GameObject.Find(Names.MovementJoystick).GetComponent<FixedJoystick>();

        RigidbodyCenterOfMass();
    }

    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        _wheelColliderController.MotorTorque(Direction * _normalSpeed * Time.fixedDeltaTime);
        _wheelColliderController.RotateWheels();

        RigidbodyTransform();
        Brake();
    }

    public void Brake()
    {
        _currentBrake = IsVehicleStopped(Direction) ? _maxBrake : 0;

        _wheelColliderController.BrakeTorque(_currentBrake);
    }

    public void RigidbodyTransform()
    {
        rigidBody.rotation = Quaternion.Euler(rigidBody.transform.eulerAngles.x, 90, 0);
        rigidBody.position = new Vector3(rigidBody.transform.position.x, rigidBody.transform.position.y, 0);
    }

    public void RigidbodyCenterOfMass()
    {
        rigidBody.centerOfMass = _normalCenterOfMass;
    }

    bool IsVehicleStopped(float value)
    {
        return value == 0;
    }
}
