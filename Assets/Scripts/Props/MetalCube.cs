using UnityEngine;
using UnityEngine.AddressableAssets;


//ADDRESSABLE
public class MetalCube : MetalTile
{
    [SerializeField] [Space]
    protected AssetReference _assetReferenceParticles;



    protected override void PlaySoundFX()
    {
        
    }

    protected override void InstantiateAddressables()
    {
        base.InstantiateAddressables();
        InstantiateParticles();
    }

    private void InstantiateParticles()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z);

        _assetReferenceParticles.InstantiateAsync(position, Quaternion.identity, null);
    }
}
