using UnityEngine;
using UnityEngine.AddressableAssets;


//ADDRESSABLE
public class LavaBubblesSpawner : MonoBehaviour
{
    [SerializeField] 
    private AssetReference _assetReferenceBubbles;


    private void Start() => _assetReferenceBubbles.InstantiateAsync(transform);
}
