using UnityEngine;

public class BulletSensorExplosion : BaseBulletExplosion
{
    [SerializeField] [Space]
    protected BaseBulletController _baseBulletController;



    protected virtual void FixedUpdate()
    {
        DestroyOnZeroAngle();
    }

    protected virtual void DestroyOnZeroAngle()
    {
        if (_baseBulletController.RigidBody.rotation == Quaternion.Euler(0, 0, 0))
        {
            Explode(null);
        }
    }
}
