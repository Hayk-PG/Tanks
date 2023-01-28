using UnityEngine;

public class BulletParticlesDirect : BulletParticles
{
    private IBulletExplosionDirect _iBulletExplosionDirect;




    protected override void GetIBulletExplosion() => _iBulletExplosionDirect = Get<IBulletExplosionDirect>.From(gameObject);

    protected override void ListenIBulletExplosion(bool isSubscribing)
    {
        if (_iBulletExplosionDirect == null)
            return;

        if(isSubscribing)
            _iBulletExplosionDirect.onBulletExplosion += OnExplosion;
        else
            _iBulletExplosionDirect.onBulletExplosion -= OnExplosion;
    }

    private void OnExplosion(Collider collider, IScore ownerScore, float distance)
    {
        _explosion.Collider = collider;
        _explosion.OwnerScore = ownerScore;      
        _explosion.Distance = distance;
        _explosion.gameObject.SetActive(true);
        _explosion.transform.parent = null;
    }
}
