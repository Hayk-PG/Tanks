using UnityEngine;
using UnityEngine.AddressableAssets;


//ADDRESSABLES
public class ExplosionAddressables : MonoBehaviour
{
    [SerializeField] private AssetReference _assetReferenceParticles;



    private void Awake()
    {
        _assetReferenceParticles.InstantiateAsync(transform).Completed += (asset) => 
        {
            asset.Result.transform.SetParent(null);
            DestroyGameObject();
        };
    }

    private void DestroyGameObject() => Destroy(gameObject);
}
