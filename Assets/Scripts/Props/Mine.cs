using UnityEngine;
using UnityEngine.AddressableAssets;


//ADDRESSABLE
public class Mine : MetalTile
{
    private void OnTriggerEnter(Collider other) => ExecuteOnTriggerEnter(other);

    private void ExecuteOnTriggerEnter(Collider collider)
    {
        print(collider.name);
    }

    protected override void PlaySoundFX()
    {
        
    }

    protected override void OnDestruction() => Addressables.Release(_meshObj);
}
