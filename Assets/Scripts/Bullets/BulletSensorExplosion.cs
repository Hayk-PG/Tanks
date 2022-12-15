using UnityEngine;

public class BulletSensorExplosion : BulletExplosion
{
    protected override void OnEnable()
    {
        if (_iBulletLimit != null)
            _iBulletLimit.OnExplodeOnLimit += OnExplodeOnLimit;

        if (_iBulletSensor != null)
            _iBulletSensor.OnHit += Hit;
    }

    protected override void OnDisable()
    {
        if (_iBulletLimit != null)
            _iBulletLimit.OnExplodeOnLimit -= OnExplodeOnLimit;

        if (_iBulletSensor != null)
            _iBulletSensor.OnHit -= Hit;
    }

    private void Hit(RaycastHit hit)
    {
        OnExplodeOnCollision(_iBulletId.OwnerScore, _iBulletId.Distance);
    }
}
