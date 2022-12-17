using System;
using UnityEngine;

public class BulletSensor : MonoBehaviour, IBulletSensor
{
    [SerializeField] internal Transform[] _points;
    [SerializeField] internal float _rayLength;  
    private int _contactCount;
    public Action<RaycastHit> OnHit { get; set; }



    private void FixedUpdate()
    {
        Hit();
    }

    private bool IsHit(Transform point, out RaycastHit _hit)
    {
        bool isHit = Physics.Raycast(point.position, point.TransformDirection(Vector3.forward), out _hit, _rayLength);

        return point != null && isHit;
    }

    private void Hit()
    {
        foreach (var point in _points)
        {
            if (IsHit(point, out RaycastHit hit))
            {
                if (!hit.collider.isTrigger && _contactCount < 1)
                {
                    OnHit?.Invoke(hit);
                    _contactCount++;
                }
            }
        }
    }
}
