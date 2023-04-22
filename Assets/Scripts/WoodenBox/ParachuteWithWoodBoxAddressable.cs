using UnityEngine;
using UnityEngine.AddressableAssets;


//ADDRESSABLE
public class ParachuteWithWoodBoxAddressable : MonoBehaviour
{
    [SerializeField]
    private AssetReference _assetReferenceParachute, _assetReferenceWoodBox;

    [SerializeField] [Space]
    private Transform _woodBoxTransform;

    [SerializeField] [Space]
    private ParachuteWithWoodBoxController _parachuteWithWoodBoxController;

    private GameObject _parachuteObj, _woodBoxObj;



    private void Awake()
    {
        InstantiateParachuteAsync();
        InstantiateWoodBoxAsync();
    }

    private void OnDestroy()
    {
        Addressables.ReleaseInstance(_parachuteObj);
        Addressables.ReleaseInstance(_woodBoxObj);
    }

    private void InstantiateParachuteAsync()
    {
        _assetReferenceParachute.InstantiateAsync(transform).Completed += (asset) =>
        {
            _parachuteObj = asset.Result;
            _parachuteWithWoodBoxController.ParachuteObj = _parachuteObj;
        };
    }

    private void InstantiateWoodBoxAsync()
    {
        _assetReferenceWoodBox.InstantiateAsync(_woodBoxTransform).Completed += (asset) =>
        {
            _woodBoxObj = asset.Result;
        };
    }
}
