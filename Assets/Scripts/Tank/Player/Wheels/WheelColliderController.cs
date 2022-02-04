using UnityEngine;

public class WheelColliderController : MonoBehaviour
{
    [SerializeField] WheelCollider[] _wheelCollider;
    [SerializeField] Transform[] _wheelTransforms;

    Vector3 _wheelWorldPosition;
    Quaternion _wheelWorldRotation;


    public void RotateWheels()
    {
        for (int i = 0; i < _wheelCollider.Length; i++)
        {
            _wheelCollider[i].GetWorldPose(out _wheelWorldPosition, out _wheelWorldRotation);
            _wheelTransforms[i].transform.position = _wheelWorldPosition;
            _wheelTransforms[i].transform.rotation = _wheelWorldRotation;
        }
    }

    public void MotorTorque(float torque)
    {
        foreach (var wheel in _wheelCollider)
        {
            wheel.motorTorque = torque;
        }
    }

    public void BrakeTorque(float torque)
    {
        foreach (var wheel in _wheelCollider)
        {
            wheel.brakeTorque = torque;
        }
    }
}
