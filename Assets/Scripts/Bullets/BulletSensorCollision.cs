using UnityEngine;

public class BulletSensorCollision : BaseBulletCollision
{
    [SerializeField] [Space]
    protected BulletSensor _bulletSensor;


    protected virtual void OnEnable()
    {
        _bulletSensor.OnHit += OnHit;
    }

    protected virtual void OnDisable()
    {
        _bulletSensor.OnHit -= OnHit;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        
    }

    protected virtual void OnHit(RaycastHit raycastHit)
    {
        if (!_isCollided)
        {
            RaiseOnCollision(raycastHit.collider);
            OnCollision(raycastHit.collider);
            _isCollided = true;
        }
    }
}
