using System;
using UnityEngine;

public class BulletRaycastReceiver2 : BulletRaycastReceiver
{
    public Action<RaycastHit, IScore, float> OnCollision { get; set; }
    public Action<IScore, float> OnExplodeOnCollision { get; set; }


    protected override void OnFrontRaycastHit(RaycastHit hit)
    {
        OnCollision?.Invoke(hit, _bulletControllerWithRaycast.OwnerScore, _bulletControllerWithRaycast.Distance);
        OnExplodeOnCollision?.Invoke(_bulletControllerWithRaycast.OwnerScore, _bulletControllerWithRaycast.Distance);
    }
}
