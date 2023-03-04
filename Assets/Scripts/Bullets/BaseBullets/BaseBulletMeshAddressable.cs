using UnityEngine;
using UnityEngine.AddressableAssets;


//ADDRESSABLE
public class BaseBulletMeshAddressable : MonoBehaviour
{
    [SerializeField]
    private AssetReference _assetReferenceMesh;

    private GameObject _mesh;


    private void Awake()
    {
        _assetReferenceMesh.InstantiateAsync(transform).Completed += asset => 
        {
            _mesh = asset.Result;
        };
    }

    private void OnDestroy()
    {
        if (_mesh == null)
            return;

        Addressables.Release(_mesh);
    }
}
