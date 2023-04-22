using UnityEngine;
using UnityEngine.AddressableAssets;


//ADDRESSABLE
public class BomberAddressable : MonoBehaviour
{
    [SerializeField]
    private AssetReference _assetReferenceMesh;



    public void LoadMeshes()
    {
        if (_assetReferenceMesh.IsValid())
            return;

        _assetReferenceMesh.InstantiateAsync(transform).Completed += asset => { asset.Result.transform.SetAsFirstSibling(); };
    }
}
