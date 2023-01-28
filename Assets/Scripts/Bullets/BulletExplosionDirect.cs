using System;
using UnityEngine;


public class BulletExplosionDirect : BulletExplosion, IBulletExplosionDirect
{
    private IBulletCollisionDirect _iBulletCollisionDirect;

    public event Action<Collider, IScore, float> onBulletExplosion;




    protected override void GetIBulletCollision()
    {
        _iBulletCollisionDirect = Get<IBulletCollisionDirect>.From(gameObject);
    }

    protected override void ListenIBulletCollision(bool isSubscribing)
    {
        if (_iBulletCollisionDirect == null)
            return;

        if(isSubscribing)
            _iBulletCollisionDirect.onHit += OnExplodeOnCollision;
        else
            _iBulletCollisionDirect.onHit -= OnExplodeOnCollision;
    }

    private void OnExplodeOnCollision(Collider collider, IScore iScore, float distance)
    {
        onBulletExplosion?.Invoke(collider, iScore, distance);
        DestroyBullet();
    }
}
