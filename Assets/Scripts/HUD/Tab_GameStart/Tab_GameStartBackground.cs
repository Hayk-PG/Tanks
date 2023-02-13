using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;


//ADDRESSABLE
public class Tab_GameStartBackground : MonoBehaviour
{
    [SerializeField]
    private Image _img;

    [SerializeField] [Space]
    private AssetReferenceSprite _assetReferenceSprite;

    private Sprite _sprt;


    private void Awake() => LoadSpriteAsync();

    private void OnDestroy() => Addressables.Release(_sprt);

    private void LoadSpriteAsync()
    {
        _assetReferenceSprite.LoadAssetAsync().Completed += (asset) =>
        {
            _sprt = asset.Result;
            _img.sprite = _sprt;
        };
    }
}
