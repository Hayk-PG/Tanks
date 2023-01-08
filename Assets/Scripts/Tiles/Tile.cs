using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

//ADDRESSABLE
public class Tile : MonoBehaviour, IDestruct
{
    [SerializeField] private TileProps _tileProps;
    [SerializeField] private AssetReference _assetReferenceMesh;
    [SerializeField] private AssetReference _assetReferenceParticles;
    [SerializeField] private bool _isSuspended, _isProtected;

    private GameObject _cachedMesh;

    public bool IsSuspended
    {
        get => _isSuspended;
        set => _isSuspended = value;
    }
    public bool IsProtected
    {
        get => _isProtected;
        set => _isProtected = value;
    }
    public float Health { get; set; } = 100;
    public Action<float> OnTileHealth { get; set; }



    private void Start() => InstantiateMesh();

    private void InstantiateMesh()
    {
        if (!System.String.IsNullOrEmpty(_assetReferenceMesh.AssetGUID))
            _assetReferenceMesh.InstantiateAsync(transform).Completed += (asset) => { _cachedMesh = asset.Result; };
    }

    private void Destruction()
    {
        LevelGenerator.ChangeTiles.UpdateTiles(transform.position);
        LevelGenerator.TilesData.TilesDict.Remove(transform.position);
        _assetReferenceParticles.InstantiateAsync().Completed += (asset) =>
        {
            asset.Result.transform.position = transform.position;
            _assetReferenceMesh.ReleaseInstance(_cachedMesh);
            Destroy(gameObject);
        };
    }

    public void Destruct(int damage, int tileParticleIndex)
    {
        Health -= IsProtected ? damage : damage * 10;
        OnTileHealth?.Invoke(Health);

        if (Health <= 0)
        {
            IsProtected = false;
            Destruction();
        }
    }
}
