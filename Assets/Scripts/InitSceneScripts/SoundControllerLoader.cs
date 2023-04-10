using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;



//ADDRESSABLE
public class SoundControllerLoader : MonoBehaviour
{
    [SerializeField] 
    private AssetReference _assetReferenceSoundController;

    [SerializeField] [Space]
    private InitAddressablesValidationChecklist _validationChecklist;




    private void Awake() => StartCoroutine(RunIteration());

    private IEnumerator RunIteration()
    {
        if(_assetReferenceSoundController.InstantiateAsync().IsValid())
        {
            ConfirmValidation();

            yield break;
        }

        yield return _assetReferenceSoundController.InstantiateAsync().IsDone;

        ConfirmValidation();
    }

    private void ConfirmValidation() => _validationChecklist.CheckValidation(true, null, null);
}
