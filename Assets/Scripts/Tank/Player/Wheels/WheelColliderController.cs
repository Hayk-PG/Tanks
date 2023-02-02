using UnityEngine;

public class WheelColliderController : MonoBehaviour
{
    private Vector3 _wheelWorldPosition;
    private Quaternion _wheelWorldRotation;
    private Transform _wheelsGameObjectsContainer;
    private struct Wheels
    {
        internal Transform _wheelTransform;
        internal WheelCollider _wheelCollider;
    }
    private Wheels[] _wheels;


    private void Awake()
    {
        InitializeWheels();
    }

    private void InitializeWheels()
    {
        _wheelsGameObjectsContainer = transform.parent?.Find("Wheels");
        int? _wheelsCount = _wheelsGameObjectsContainer?.childCount;

        if (_wheelsCount != null)
        {
            _wheels = new Wheels[(int)_wheelsCount];

            for (int i = 0; i < _wheels.Length; i++)
            {
                _wheels[i]._wheelTransform = _wheelsGameObjectsContainer.GetChild(i);
                _wheels[i]._wheelCollider = transform.GetChild(i).GetComponent<WheelCollider>();
            }
        }
    }

    public void RotateWheels()
    {
        if (_wheels != null)
        {
            for (int i = 0; i < _wheels.Length; i++)
            {
                _wheels[i]._wheelCollider.GetWorldPose(out _wheelWorldPosition, out _wheelWorldRotation);
                _wheels[i]._wheelTransform.position = _wheelWorldPosition;
                _wheels[i]._wheelTransform.rotation = _wheelWorldRotation;
            }
        }
    }

    public void MotorTorque(float torque)
    {
        if (_wheels != null)
        {
            foreach (var wheel in _wheels)
            {
                wheel._wheelCollider.motorTorque = torque;
            }
        }
    }

    public void BrakeTorque(float torque)
    {
        if (_wheels != null)
        {
            foreach (var wheel in _wheels)
            {
                wheel._wheelCollider.brakeTorque = torque;
            }
        }
    }

    public float WheelRPM()
    {
        if (_wheels != null)
        {
            float rpm = 0;

            foreach (var wheel in _wheels)
            {
                rpm = wheel._wheelCollider.rpm;
            }

            return rpm;
        }
        else
        {
            return 0;
        }
    }

    public bool IsGrounded()
    {
        if (_wheels != null)
        {
            int groundedWheels = 0;

            for (int i = 0; i < _wheels.Length; i++)
            {
                if (_wheels[i]._wheelCollider.isGrounded) groundedWheels++;
            }

            return groundedWheels == _wheels.Length ? true : false;
        }
        else
        {
            return false;
        }
    }
}
