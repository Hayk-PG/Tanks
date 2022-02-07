using System;
using UnityEngine;

public class VehicleFall : MonoBehaviour
{
    private WheelColliderController _wheelColliderController;

    private bool _isVehicleFalling;
    private bool _isVehicleFelt;

    [SerializeField]
    private GameObject _smoke;
        

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
            _isVehicleFalling = false;
        }
    }
}
