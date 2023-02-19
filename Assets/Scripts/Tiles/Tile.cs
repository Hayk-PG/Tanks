using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

//ADDRESSABLE
public class Tile : MonoBehaviour, IDestruct
{
    [SerializeField] 
    private TileProps _tileProps;

    [SerializeField] [Space]
    private AssetReference _assetReferenceMesh, _assetReferenceParticles;

    [SerializeField] [Space]
    private bool _isSuspended, _isProtected;

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
    public bool IsOverlapped { get; private set; }
    public float Health { get; set; } = 100;

    public Action<float> OnTileHealth { get; set; }

    public event Action<GameObject> onMeshInstantiated;

    public event Action onDestroyingMesh;

    public event Action onDestruction;





    private void Start() => InstantiateMesh();

    private void InstantiateMesh()
    {
        if (!System.String.IsNullOrEmpty(_assetReferenceMesh.AssetGUID))
        {
            _assetReferenceMesh.InstantiateAsync(transform).Completed += asset =>
            {
                _cachedMesh = asset.Result;

                if (IsOverlapped)
                {
                    DestroyMesh();

                    return;
                }

                onMeshInstantiated?.Invoke(asset.Result);
            };
        }
    }

    private void DestroyMesh()
    {
        if (_cachedMesh != null)
        {
            Addressables.Release(_cachedMesh);

            Destroy(_cachedMesh);
        }
    }

    private void Destruction()
    {
        LevelGenerator.ChangeTiles.UpdateTiles(transform.position);
        LevelGenerator.TilesData.TilesDict.Remove(transform.position);

        _assetReferenceParticles.InstantiateAsync().Completed += (asset) =>
        {
            asset.Result.transform.position = transform.position;

            onDestruction?.Invoke();

            DestroyMesh();

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

    public void DetectingOverlap()
    {
        IsOverlapped = true;

        if (_cachedMesh == null)
            return;

        onDestroyingMesh?.Invoke();

        DestroyMesh();
    }
}
