using System;
using UnityEngine;


public class BulletExplosionDirect : BaseBulletExplosion
{
    [SerializeField]
    protected BaseBulletController _baseBulletController;

    [SerializeField]
    protected BulletCollisionDirect _bulletCollisionDirect;

    public event Action<Collider, IScore, float> onBulletExplosion;



    protected override void RaiseOnExplode(Collider collider)
    {
        onBulletExplosion?.Invoke(collider, _baseBulletController.OwnerScore, _baseBulletController.Distance);
    }
}
