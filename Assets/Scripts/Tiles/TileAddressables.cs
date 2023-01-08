using UnityEngine;
using UnityEngine.AddressableAssets;

public class TileAddressables : MonoBehaviour
{
    [SerializeField] private AssetReference _assetReferenceRenderers;


    private void Start()
    {
        if (!System.String.IsNullOrEmpty(_assetReferenceRenderers.AssetGUID))
        {
            _assetReferenceRenderers.InstantiateAsync(transform).Completed += (asset) =>
            {

            };
        }
    }
}
