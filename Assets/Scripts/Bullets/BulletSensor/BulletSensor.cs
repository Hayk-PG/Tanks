using System;
using UnityEngine;

public class BulletSensor : MonoBehaviour, IBulletSensor
{
    [SerializeField] internal Transform _raycastPoint;
    [SerializeField] internal float _rayLength;  
    private int _contactCount;
    public Action<RaycastHit> OnHit { get; set; }



    private void FixedUpdate()
    {
        DrawRay();
        Hit();
    }

    private void DrawRay() => Debug.DrawRay(_raycastPoint.position, _raycastPoint.TransformDirection(Vector3.forward) * _rayLength, Color.red);

    private bool IsHit(out RaycastHit _hit)
    {
        bool isHit = Physics.Raycast(_raycastPoint.position, _raycastPoint.TransformDirection(Vector3.forward), out _hit, _rayLength);

        return _raycastPoint != null && isHit;
    }

    private void Hit()
    {
        if(IsHit(out RaycastHit hit))
        {
            if(!hit.collider.isTrigger && _contactCount < 1)
            {
                OnHit?.Invoke(hit);
                _contactCount++;
            }
        }
    }
}
