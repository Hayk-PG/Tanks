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
            bool isDone = asset.LoadAssetAsync<GameObject>().IsDone;
            bool isValid = asset.OperationHandle.IsValid();

            if (isValid)
                continue;

            yield return new WaitUntil(() => isDone && isValid);
        }

        _validationChecklist.CheckValidation(null, IsValid = true, null);
    }
}
