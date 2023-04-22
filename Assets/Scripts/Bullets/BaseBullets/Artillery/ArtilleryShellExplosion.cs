using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryShellExplosion : BaseBulletExplosion
{
    [SerializeField] [Space]
    protected BaseBulletController _baseBulletControll;


    public override void DestroyBullet()
    {
        if (!_baseBulletControll.IsLastShellOfBarrage)
        {
            Destroy(gameObject);
        }
        else
        {
            base.DestroyBullet();
        }
    }
}
