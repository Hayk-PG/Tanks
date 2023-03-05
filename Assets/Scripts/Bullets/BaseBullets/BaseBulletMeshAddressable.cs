using UnityEngine;
using UnityEngine.AddressableAssets;


//ADDRESSABLE
public class BaseBulletMeshAddressable : MonoBehaviour
{
    [SerializeField]
    private AssetReference _assetReferenceMesh;


    private void Awake() => _assetReferenceMesh.InstantiateAsync(transform);
}
