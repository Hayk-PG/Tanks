using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;


//ADDRESSABLE
public class Tab_PopUpBackground : MonoBehaviour
{
    [SerializeField]
    private Image _imgBackground;

    [SerializeField] [Space]
    private AssetReferenceSprite _assetReferenceSprite;

    private Sprite _sprt;


    private void Awake() => LoadBackgroundSpriteAsync();

    private void OnDestroy() => Release();

    private void LoadBackgroundSpriteAsync()
    {
        _assetReferenceSprite.LoadAssetAsync().Completed += asset =>
        {
            _sprt = asset.Result;

            _imgBackground.sprite = _sprt;
        };
    }

    private void Release()
    {
        Addressables.Release(_sprt);
    }
}
