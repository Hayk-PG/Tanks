public class BulletWithMultipleHitExplosion : BulletExplosion
{
    protected BulletWithMultipleHitParticles _bulletWithMultipleHitParticles;
    protected int _hitsCount;


    protected override void Awake()
    {
        base.Awake();
        _bulletWithMultipleHitParticles = Get<BulletWithMultipleHitParticles>.From(gameObject);
    }

    protected override void OnExplodeOnCollision(IScore ownerScore, float distance)
    {
        OnBulletExplosion?.Invoke(ownerScore, distance);
        OnCameraShake?.Invoke();

        _hitsCount++;
        if (_hitsCount >= _bulletWithMultipleHitParticles._explosionsCount)
            DestroyBullet();
    }
}
