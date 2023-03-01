using System;
using UnityEngine;

public class BulletSensor : MonoBehaviour, IBulletSensor
{
    [SerializeField]
    protected Transform[] _points;

    [SerializeField] [Space]
    protected float _rayLength;

    protected int _contactCount;

    [SerializeField] [Space]
    private bool _dontCountContacts;

    public Action<RaycastHit> OnHit { get; set; }



    protected virtual void FixedUpdate() => Hit();

    protected virtual void Hit()
    {
        foreach (var point in _points)
        {
            if (IsHit(point, out RaycastHit hit) && IsHit(hit))
                RaiseOnHit(hit);
        }
    }

    protected virtual bool IsHit(Transform point, out RaycastHit _hit)
    {
        bool isHit = Physics.Raycast(point.position, point.TransformDirection(Vector3.forward), out _hit, _rayLength);

        return point != null && isHit;
    }

    protected virtual bool IsHit(RaycastHit hit)
    {
        return !hit.collider.isTrigger && _contactCount < 1;
    }

    protected virtual void RaiseOnHit(RaycastHit hit)
    {
        OnHit?.Invoke(hit);

        if (_dontCountContacts)
            return;

        _contactCount++;
    }
}
