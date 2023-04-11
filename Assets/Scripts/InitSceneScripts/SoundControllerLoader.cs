using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;



//ADDRESSABLE
public class SoundControllerLoader : MonoBehaviour
{
    public static SoundControllerLoader Instance { get; private set; }

    [SerializeField] 
    private AssetReference _assetReferenceSoundController;

    [SerializeField] [Space]
    private InitAddressablesValidationChecklist _validationChecklist;

    public bool IsValid { get; private set; }




    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() => StartCoroutine(RunIteration());

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

    private void ConfirmValidation()
    {
        _validationChecklist.CheckValidation(IsValid = true, null, null);
    }
}
