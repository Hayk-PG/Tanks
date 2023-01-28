using UnityEngine;

public class BulletSensorExplosion : BulletExplosion
{
    private IBulletSensor _iBulletSensor;
    private IBulletVelocity<BulletController.VelocityData> _iBulletVelocity;



    protected override void Awake()
    {
        base.Awake();
        _iBulletSensor = Get<IBulletSensor>.FromChild(gameObject);
        _iBulletVelocity = Get<IBulletVelocity<BulletController.VelocityData>>.From(gameObject);
    }

    protected override void OnEnable()
    {
        if (_iBulletSensor != null)
            _iBulletSensor.OnHit += Hit;

        if (_iBulletLimit != null)
        {
            _iBulletLimit.OnExplodeOnLimit += OnExplodeOnLimit;
            _iBulletLimit.OnDestroyTimeLimit += delegate { Hit(default); };
        }

        if(_iBulletVelocity != null)
        {
            _iBulletVelocity.OnBulletVelocity += delegate (BulletController.VelocityData velocityData)
            {
                if (velocityData._rigidBody.rotation == Quaternion.Euler(0, 0, 0))
                    Hit(default);
            };
        }
    }

    protected override void OnDisable()
    {
        if (_iBulletSensor != null)
            _iBulletSensor.OnHit -= Hit;

        if (_iBulletLimit != null)
        {
            _iBulletLimit.OnExplodeOnLimit -= OnExplodeOnLimit;
            _iBulletLimit.OnDestroyTimeLimit -= delegate { Hit(default); };
        }

        if (_iBulletVelocity != null)
        {
            _iBulletVelocity.OnBulletVelocity -= delegate (BulletController.VelocityData velocityData)
            {
                if (velocityData._rigidBody.rotation == Quaternion.Euler(0, 0, 0))
                    Hit(default);
            };
        }
    }

    protected virtual void Hit(RaycastHit hit)
    {
        OnExplodeOnCollision(_iBulletId.OwnerScore, _iBulletId.Distance);
    }
}
