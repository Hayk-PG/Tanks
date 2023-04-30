using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;



//ADDRESSABLE
public class AddressableTile : MonoBehaviour
{
    public static AddressableTile Loader { get; private set; }

    [SerializeField]
    private List<AssetReference> _assetReferenceTilesMesh;

    public List<AssetReference> TilesMesh => _assetReferenceTilesMesh;

    [SerializeField] [Space]
    private InitAddressablesValidationChecklist _validationChecklist;

    public bool IsValid { get; private set; }




    private void Awake()
    {
        if(Loader == null)
        {
            Loader = this;

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
        foreach (var asset in _assetReferenceTilesMesh)
        {
            bool isValid = asset.OperationHandle.IsValid();

            if (isValid)
                continue;

            yield return StartCoroutine(LoadAssets(asset));
        }

        _validationChecklist.CheckValidation(null, IsValid = true, null);
    }

    private IEnumerator LoadAssets(AssetReference assetReference)
    {
        bool isAssetLoaded = false;

        assetReference.LoadAssetAsync<GameObject>().Completed += asset => { isAssetLoaded = true; };

        yield return new WaitUntil(() => isAssetLoaded == true);
    }
}
