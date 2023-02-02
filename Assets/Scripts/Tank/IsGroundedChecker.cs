using System;
using UnityEngine;

public class IsGroundedChecker : MonoBehaviour
{
    private WheelColliderController _wheelColliderController;
    private bool _isVehicleFalling;

    internal Action OnGrounded { get; set; }
        

    private void Awake()
    {
        _wheelColliderController = Get<WheelColliderController>.FromChild(transform.parent.gameObject);
    }

    private void Start()
    {
        _isVehicleFalling = !_wheelColliderController.IsGrounded();
    }

    private void Update()
    {
        if (_isVehicleFalling && _wheelColliderController.IsGrounded())
        {
            OnGrounded?.Invoke();
            SecondarySoundController.PlaySound(2, 0);

            _isVehicleFalling = false;
        }
    }
}
