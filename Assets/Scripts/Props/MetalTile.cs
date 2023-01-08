using UnityEngine;
using UnityEngine.AddressableAssets;


//ADDRESSABLE
public class MetalTile : MonoBehaviour
{
    private Tile _tile;
    private GameManager _gameManager;

    [SerializeField] protected AssetReference _assetReferenceMesh;
    [SerializeField] protected AssetReference _assetReferencePropsGUI;


    protected virtual void Awake()
    {
        _tile = Get<Tile>.From(transform.parent.gameObject);
        PlaySoundFX();
    }

    private void OnEnable() => _tile.onDestruction += OnDestruction;

    private void OnDisable() => _tile.onDestruction -= OnDestruction;

    protected virtual void Start() => InstantiateAddressables();

    protected virtual void PlaySoundFX()
    {
        _gameManager = FindObjectOfType<GameManager>();

        if (_gameManager.IsGameStarted)
            SecondarySoundController.PlaySound(1, 2);
    }

    protected virtual void InstantiateAddressables()
    {
        InstantiateMesh();
        InstantiatePropsGUI();
    }

    protected virtual void InstantiateMesh() => _assetReferenceMesh.InstantiateAsync(transform);
    protected virtual void InstantiatePropsGUI()=> _assetReferencePropsGUI.InstantiateAsync(transform);

    protected virtual void OnDestruction()
    {
        _assetReferenceMesh.ReleaseAsset();
        _assetReferencePropsGUI.ReleaseAsset();
    }
}
