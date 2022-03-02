using UnityEngine;

public class BulletParticles : MonoBehaviour
{
    [SerializeField]
    protected ParticleSystem _muzzleFlash, _trail;

    [SerializeField]
    protected Explosion _explosion;

    protected IBulletTrail _iBulletTrail;
    protected IBulletExplosion _iBulletExplosion;

    protected virtual void Awake()
    {
        if(_muzzleFlash != null) _muzzleFlash.transform.parent = null;

        _iBulletTrail = Get<IBulletTrail>.From(gameObject);
        _iBulletExplosion = Get<IBulletExplosion>.From(gameObject);
    }

    protected virtual void OnEnable()
    {
        if (_iBulletTrail != null) _iBulletTrail.OnTrailActivity += OnTrailActivity;
        if (_iBulletExplosion != null) _iBulletExplosion.OnBulletExplosion += OnExplosion;
    }

    protected virtual void OnTrailActivity(bool isActive)
    {
        _trail.gameObject.SetActive(isActive);
    }

    protected virtual void OnExplosion(IScore ownerScore)
    {
        _explosion.OwnerScore = ownerScore;
        _explosion.gameObject.SetActive(true);
        _explosion.transform.parent = null;
    }
}
