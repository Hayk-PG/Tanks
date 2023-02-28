using UnityEngine;
using UnityEngine.AddressableAssets;

public class BaseBulletParticles : MonoBehaviour
{
    [SerializeField]
    protected BaseBulletController _baseBulletController;

    [SerializeField] 
    protected BaseBulletVelocity _baseBulletVelocity;

    [SerializeField] 
    protected BaseBulletExplosion _baseBulletExplosion;

    [SerializeField] [Space]
    protected AssetReference _assetReferenceTrail, _assetReferenceMuzzleFlash;

    [SerializeField] [Space]
    protected BaseExplosion _explosion;

    [SerializeField] [Space]
    private bool _noMuzzleFlash;



    protected virtual void Awake()
    {
        InstantiateMuzzleFlash(transform.position);
    }

    protected virtual void OnEnable()
    {
        _baseBulletVelocity.onTrail += InstantiateTrail;
        _baseBulletExplosion.onExplode += OnExplode;
    }

    protected virtual void OnDisable()
    {
        _baseBulletVelocity.onTrail -= InstantiateTrail;
        _baseBulletExplosion.onExplode -= OnExplode;
    }

    protected virtual void InstantiateMuzzleFlash(Vector3 position)
    {
        if (!System.String.IsNullOrEmpty(_assetReferenceMuzzleFlash.AssetGUID) && !_noMuzzleFlash)
        {
            _assetReferenceMuzzleFlash.InstantiateAsync(transform).Completed += (asset) =>
            {
                asset.Result.transform.position = position;
                asset.Result.transform.SetParent(null);
            };
        }
    }

    protected virtual void InstantiateTrail()
    {
        if (System.String.IsNullOrEmpty(_assetReferenceTrail.AssetGUID))
            return;

        _assetReferenceTrail.InstantiateAsync(transform);
    }

    protected virtual void OnExplode()
    {
        _explosion.OwnerScore = _baseBulletController.OwnerScore;
        _explosion.Distance = _baseBulletController.Distance;
        _explosion.gameObject.SetActive(true);
        _explosion.transform.parent = null;
    }
}
