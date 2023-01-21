using UnityEngine;
using UnityEngine.AddressableAssets;


//ADDRESSABLE
public class BarrelAddressables : MonoBehaviour
{
    [SerializeField] private AssetReference _assetReferenceMesh;
    [SerializeField] private AssetReference _assetReferenceTrail;
    [SerializeField] private AssetReference _assetReferenceExplosion;

    [Space] [SerializeField] private Barrel _barrel;


    private void Awake() => _assetReferenceMesh.InstantiateAsync(transform);

    private void OnEnable()
    {
        _barrel.onLaunch += OnLaunch;
        _barrel.onExplode += OnExplode;
    }

    private void OnDisable()
    {
        _barrel.onLaunch -= OnLaunch;
        _barrel.onExplode -= OnExplode;
    }

    private void OnLaunch(Vector3 position) => _assetReferenceTrail.InstantiateAsync(transform);

    private void OnExplode(Vector3 position) => _assetReferenceExplosion.InstantiateAsync().Completed += (asset) => { asset.Result.transform.position = position; };
}
