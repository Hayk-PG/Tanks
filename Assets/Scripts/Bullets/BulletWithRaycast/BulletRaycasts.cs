using System;
using UnityEngine;


[Serializable] public struct RaycastProperties
{
    [SerializeField] internal Transform _raycastPoint;
    [SerializeField] internal float _rayLength;
    internal RaycastHit _hit;

    public bool IsHit => _raycastPoint != null && Physics.Raycast(_raycastPoint.position, _raycastPoint.TransformDirection(Vector3.forward), out _hit, _rayLength);
    public Action <RaycastHit> OnHit { get; set; }
}


public class BulletRaycasts : MonoBehaviour
{
    public RaycastProperties _front;
  
    private int _contactCount;


    private void Update()
    {
        if (_front.IsHit && _contactCount < 1)
        {
            _front.OnHit?.Invoke(_front._hit);
            _contactCount++;
        }
    }
}
