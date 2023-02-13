using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;


//ADDRESSABLE
public class TabBaseBackgroundImg : MonoBehaviour
{
    [SerializeField]
    private Image _img;

    [SerializeField] [Space]
    private AssetReferenceSprite _assetReferenceSprite;


    private void Awake() => LoadSpriteAsync();

    private void LoadSpriteAsync()
    {
        _assetReferenceSprite.LoadAssetAsync().Completed += (asset) =>
        {
            _img.sprite = asset.Result;
        };
    }
}
