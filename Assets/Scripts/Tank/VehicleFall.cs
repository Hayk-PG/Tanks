using System;
using UnityEngine;

public class VehicleFall : MonoBehaviour
{
    private WheelColliderController _wheelColliderController;

    private bool _isVehicleFalling;
    private bool _isVehicleFelt;

    public GameObject _smoke;

    internal Action OnVehicleFell;
        

    private void Awake()
    {
        _wheelColliderController = GetComponentInChildren<WheelColliderController>();
    }

    private void Start()
    {
        _isVehicleFalling = !_wheelColliderController.AreWheelsGrounded();
    }

    private void Update()
    {
        if (_isVehicleFalling && _wheelColliderController.AreWheelsGrounded())
        {
            _smoke.SetActive(true);
            OnVehicleFell?.Invoke();
            _isVehicleFalling = false;
        }
    }
}
