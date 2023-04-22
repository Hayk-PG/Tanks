using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

//ADDRESSABLE
public class Tile : MonoBehaviour, IDestruct
{
    [SerializeField] 
    private TileProps _tileProps;

    [SerializeField] [Space]
    private AssetReference _assetReferenceParticles;

    private GameObject _mesh;

    [SerializeField] [Space]
    private bool _isSuspended, _isProtected;

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
        foreach (var item in AddressableTile.Loader.TilesMesh)
        {
            if (((GameObject)item.OperationHandle.Result).name == gameObject.name)
            {
                _mesh = Instantiate(((GameObject)item.OperationHandle.Result), transform);

                _mesh.name = gameObject.name;

                if (IsOverlapped)
                {
                    DestroyMesh();

                    return;
                }

                onMeshInstantiated?.Invoke(_mesh);

                return;
            }
        }
    }

    private void DestroyMesh() => Destroy(_mesh);

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

        if (_mesh == null)
            return;

        onDestroyingMesh?.Invoke();

        DestroyMesh();
    }
}
