using UnityEngine;

public class BulletParticles : MonoBehaviour
{
    [SerializeField]
    protected ParticleSystem _muzzleFlash;

    [SerializeField]
    protected GameObject _trail;

    [SerializeField]
    protected Explosion _explosion;

    [Header("Optional non collided explosion")]
    [SerializeField]
    protected GameObject _optionalExplosion;

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
        if (_iBulletExplosion != null) _iBulletExplosion.OnBulletExplosionWithoutHitting += OnBulletExplosionWithoutHitting;
    }

    protected virtual void OnTrailActivity(bool isActive)
    {
        if(_trail != null) _trail.SetActive(isActive);
    }

    protected virtual void OnExplosion(IScore ownerScore, float distance)
    {
        _explosion.OwnerScore = ownerScore;
        _explosion.Distance = distance;
        _explosion.gameObject.SetActive(true);
        _explosion.transform.parent = null;
    }

    protected virtual void OnBulletExplosionWithoutHitting()
    {
        if(_optionalExplosion != null)
        {
            _optionalExplosion.SetActive(true);
            _optionalExplosion.transform.parent = null;
        }
    }
}
