using UnityEngine;

public class BulletSensorCollision : BulletCollision
{
    private IBulletSensor _iBulletSensor;
    private IBulletID _iBulletId;




    protected override void Awake()
    {
        base.Awake();
        _iBulletSensor = Get<IBulletSensor>.FromChild(gameObject);
        _iBulletId = Get<IBulletID>.From(gameObject);
    }

    protected override void OnEnable() => _iBulletSensor.OnHit += Hit;

    protected override void OnDisable() => _iBulletSensor.OnHit -= Hit;

    protected virtual void Hit(RaycastHit hit)
    {
        OnCollision(hit.collider, _iBulletId.OwnerScore, _iBulletId.Distance);
    }
}
