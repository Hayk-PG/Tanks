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
            if (asset.OperationHandle.IsValid())
                continue;

            yield return asset.LoadAssetAsync<GameObject>().IsDone;
        }

        yield return null;

        _validationChecklist.CheckValidation(null, true, null);
    }
}
