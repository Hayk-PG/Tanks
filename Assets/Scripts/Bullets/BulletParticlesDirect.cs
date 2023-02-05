using UnityEngine;

public class BulletParticlesDirect : BaseBulletParticles
{
    [SerializeField]
    [Space]
    protected BulletExplosionDirect _bulletExplosionDirect;


    protected override void OnEnable()
    {
        _baseBulletVelocity.onTrail += base.InstantiateTrail;
        _bulletExplosionDirect.onBulletExplosion += OnExplosion;
    }

    protected override void OnDisable()
    {
        _baseBulletVelocity.onTrail -= base.InstantiateTrail;
        _bulletExplosionDirect.onBulletExplosion -= OnExplosion;
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
