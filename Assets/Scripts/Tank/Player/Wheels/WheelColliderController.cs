﻿using UnityEngine;

public class WheelColliderController : MonoBehaviour
{
    [SerializeField]
    private WheelCollider[] _wheelCollider;
    [SerializeField]
    private Transform[] _wheelTransforms;

    private Vector3 _wheelWorldPosition;
    private Quaternion _wheelWorldRotation;


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

    public float WheelRPM()
    {
        float rpm = 0;

        foreach (var wheel in _wheelCollider)
        {
            rpm = wheel.rpm;
        }

        return rpm;
    }

    public bool AreWheelsGrounded()
    {
        int groundedWheels = 0;

        for (int i = 0; i < _wheelCollider.Length; i++)
        {
            if (_wheelCollider[i].isGrounded) groundedWheels++;
        }

        return groundedWheels == _wheelCollider.Length ? true : false;
    }
}
