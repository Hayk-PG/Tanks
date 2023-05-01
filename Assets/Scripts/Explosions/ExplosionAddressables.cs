using UnityEngine;
using UnityEngine.AddressableAssets;


//ADDRESSABLES
public class ExplosionAddressables : MonoBehaviour
{
    [SerializeField] 
    protected AssetReference _assetReferenceParticles;



    protected virtual void Awake() => _assetReferenceParticles.InstantiateAsync(transform).Completed += asset => OnAssetInstantiate(asset.Result);

    protected virtual void OnAssetInstantiate(GameObject asset)
    {
        asset.transform.SetParent(null);

        Destroy(gameObject);
    }
}
