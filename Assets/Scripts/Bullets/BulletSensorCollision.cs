using UnityEngine;

public class BulletSensorCollision : BulletCollision
{
    protected override void Awake()
    {
        base.Awake();
        _iBulletSensor = Get<IBulletSensor>.FromChild(gameObject);
    }

    protected override void OnEnable() => _iBulletSensor.OnHit += Hit;

    protected override void OnDisable() => _iBulletSensor.OnHit -= Hit;

    protected virtual void Hit(RaycastHit hit)
    {
        OnCollision(hit.collider, _iBulletId.OwnerScore, _iBulletId.Distance);
    }
}
