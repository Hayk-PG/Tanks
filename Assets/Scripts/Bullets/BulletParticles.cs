using UnityEngine;

public class BulletParticles : MonoBehaviour
{
    protected IBulletTrail _iBulletTrail;
    protected IBulletExplosion _iBulletExplosion;

    [SerializeField] protected ParticleSystem _muzzleFlash;
    [SerializeField] protected GameObject _trail;
    [SerializeField] protected Explosion _explosion;
    [Header("Optional non collided explosion")]
    [SerializeField] protected GameObject _optionalExplosion;

    

    protected virtual void Awake()
    {
        if(_muzzleFlash != null) 
            _muzzleFlash.transform.parent = null;

        _iBulletTrail = Get<IBulletTrail>.From(gameObject);
        _iBulletExplosion = Get<IBulletExplosion>.From(gameObject);
    }

    protected virtual void OnEnable()
    {
        if (_iBulletTrail != null)
            _iBulletTrail.OnTrailActivity += OnTrailActivity;

        if (_iBulletExplosion != null)
            _iBulletExplosion.OnBulletExplosion += OnExplosion;
    }

    protected virtual void OnDisable()
    {
        if (_iBulletTrail != null)
            _iBulletTrail.OnTrailActivity -= OnTrailActivity;

        if (_iBulletExplosion != null)
            _iBulletExplosion.OnBulletExplosion -= OnExplosion;
    }

    protected virtual void OnTrailActivity(bool isActive)
    {
        if (_trail != null)
            _trail.SetActive(isActive);
    }

    protected virtual void OnExplosion(IScore ownerScore, float distance)
    {
        _explosion.OwnerScore = ownerScore;
        _explosion.Distance = distance;
        _explosion.gameObject.SetActive(true);
        _explosion.transform.parent = null;
    }
}
