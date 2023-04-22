using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;


//ADDRESSABLE
public abstract class TabBaseImgLoader<T> : MonoBehaviour
{
    protected T[] _observers;

    [SerializeField]
    protected AssetReferenceSprite[] _assetReferenceSprites;

    [SerializeField] [Space]
    protected Image[] _images;

    protected Sprite[] _loadedSprites;

    [SerializeField] [Space]
    protected bool _isLoaderInstance;

    protected int _assetsCount, _loadedAssetsCount;






    protected virtual void Awake() => Initialize();

    protected virtual void Start() => LoadAssets();

    protected virtual void OnDestroy() => ReleaseAssets();

    protected virtual void Initialize()
    {
        if (!_isLoaderInstance)
            return;

        _observers = FindObjectOfType<MenuTabs>().GetComponentsInChildren<T>();

        _assetsCount = _assetReferenceSprites.Length;

        _loadedSprites = new Sprite[_assetsCount];
    }

    protected virtual void LoadAssets()
    {
        if (!_isLoaderInstance)
            return;

        if (AssetsLoaded())
            return;

        GlobalFunctions.Loop<AssetReferenceSprite>.Foreach(_assetReferenceSprites, asset => asset.LoadAssetAsync().Completed += OnLoadAssetCompleted);
    }

    protected virtual void OnLoadAssetCompleted(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<Sprite> asset)
    {
        _loadedSprites[_loadedAssetsCount] = asset.Result;

        _loadedAssetsCount++;

        if (_loadedAssetsCount < _assetsCount)
            return;

        AssignObserversSprites(_loadedSprites);
    }

    protected abstract void AssignObserversSprites(Sprite[] sprites);

    protected virtual void ReleaseAssets()
    {
        if (_isLoaderInstance && AssetsLoaded())
            GlobalFunctions.Loop<AssetReferenceSprite>.Foreach(_assetReferenceSprites, asset => { asset.ReleaseAsset(); });
    }

    protected virtual bool AssetsLoaded()
    {
        bool assetsLoaded = false;

        GlobalFunctions.Loop<AssetReferenceSprite>.Foreach(_assetReferenceSprites, asset => { assetsLoaded = asset.OperationHandle.IsValid(); });

        return assetsLoaded;
    }

    public virtual void AssignSprites(Sprite[] sprites)
    {
        for (int i = 0; i < _images.Length; i++)
        {
            _images[i].sprite = sprites[i];
            _images[i].gameObject.SetActive(true);
        }
    }
}
