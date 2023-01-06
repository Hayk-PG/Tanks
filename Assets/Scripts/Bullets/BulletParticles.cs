using UnityEngine;
using UnityEngine.AddressableAssets;

//ADDRESSABLE
public class BulletParticles : MonoBehaviour
{
    protected IBulletTrail _iBulletTrail;
    protected IBulletExplosion _iBulletExplosion;

    [SerializeField] protected AssetReference _assetReferenceTrail;

    [SerializeField] protected ParticleSystem _muzzleFlash;
    [SerializeField] protected Explosion _explosion;
    [Header("Optional non collided explosion")]
    [SerializeField] protected GameObject _optionalExplosion;

    protected bool _isTrailInstantiated;

    protected GameObject Trail { get; set; }
    

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

    protected virtual void OnDestroy() => ReleaseAddressables();

    protected virtual void OnTrailActivity(bool isActive)
    {
        if (System.String.IsNullOrEmpty(_assetReferenceTrail.AssetGUID))
            return;

        InstantiateAndCacheTrail();
    }

    protected void InstantiateAndCacheTrail()
    {
        if (!_isTrailInstantiated)
        {
            Addressables.InstantiateAsync(_assetReferenceTrail, transform).Completed += (asset) =>
            {
                Trail = asset.Result;
                _isTrailInstantiated = true;
            };
        }
    }

    protected virtual void OnExplosion(IScore ownerScore, float distance)
    {
        _explosion.OwnerScore = ownerScore;
        _explosion.Distance = distance;
        _explosion.gameObject.SetActive(true);
        _explosion.transform.parent = null;
    }

    protected virtual void ReleaseAddressables()
    {
        if (Trail != null)
            Addressables.ReleaseInstance(Trail);
    }
}
