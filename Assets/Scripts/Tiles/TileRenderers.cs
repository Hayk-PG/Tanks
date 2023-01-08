using UnityEngine;
using UnityEngine.AddressableAssets;

public class TileRenderers : MonoBehaviour
{
    [SerializeField] private AssetReference _assetReferenceRenderers;


    private void Start()
    {
        if (!System.String.IsNullOrEmpty(_assetReferenceRenderers.AssetGUID))
        {
            _assetReferenceRenderers.InstantiateAsync(transform);
        }
    }
}
