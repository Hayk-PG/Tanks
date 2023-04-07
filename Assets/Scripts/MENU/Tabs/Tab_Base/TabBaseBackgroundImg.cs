using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;


//ADDRESSABLE
public class TabBaseBackgroundImg : MonoBehaviour
{
    [SerializeField]
    protected Image _img;

    [SerializeField] [Space]
    protected AssetReferenceSprite _assetReferenceSprite;

    protected Sprite _sprt;


    protected virtual void Awake() => LoadSpriteAsync();

    protected virtual void OnDestroy() => Addressables.Release(_sprt);

    protected virtual void LoadSpriteAsync() => _assetReferenceSprite.LoadAssetAsync().Completed += asset => AssignImageSprite(asset.Result);

    protected virtual void AssignImageSprite(Sprite sprite)
    {
        _sprt = sprite;

        _img.sprite = sprite;

        _img.color = Color.white;
    }
}
