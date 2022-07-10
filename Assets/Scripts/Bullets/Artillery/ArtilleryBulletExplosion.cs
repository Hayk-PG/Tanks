using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryBulletExplosion : BulletExplosion
{
    private ArtilleryBulletController _artillertBulletController;

    protected override void Awake()
    {
        base.Awake();

        _artillertBulletController = Get<ArtilleryBulletController>.From(gameObject);
    }

    protected override void DestroyBullet()
    {
        base.DestroyBullet();
    }

    protected override void SetTurnToTransition()
    {
        if (_artillertBulletController.IsLastShellOfBarrage)
            base.SetTurnToTransition();
    }
}
