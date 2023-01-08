using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Tile : MonoBehaviour, IDestruct
{
    [SerializeField] private Collider _collider;
    [SerializeField] private TileProps _tileProps;
    [SerializeField] private AssetReference _assetReferenceParticles;
    [SerializeField] private bool _isSuspended, _isProtected;

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



    public void StoreForLaterUse()
    {
        LevelGenerator.TilesData.TilesDict.Remove(transform.position);
        LevelGenerator.TilesData.StoredInactiveTiles.Find(tile => tile.TileName == name).Tiles?.Add(this);
        transform.SetParent(LevelGenerator.TilesData.IntactiveTilesContainer);
        gameObject.SetActive(false);
    }

    public void ReUse()
    {
        IsProtected = false;
        IsSuspended = false;
        Health = 100;
        OnTileHealth?.Invoke(Health);
        LevelGenerator.TilesData.TilesDict.Add(transform.position, gameObject);

        Trigger(false);
        _tileProps?.ActiveProps(TileProps.PropsType.All, false, null);
    }

    public void Destruct(int damage, int tileParticleIndex)
    {
        Health -= IsProtected ? damage : damage * 10;
        OnTileHealth?.Invoke(Health);

        if (Health <= 0)
        {
            IsProtected = false;
            Destruction(tileParticleIndex);
        }
    }

    private void Trigger(bool isTrigger) => _collider.isTrigger = isTrigger;

    private void ExplosionActivity()
    {
        _assetReferenceParticles.InstantiateAsync().Completed += (asset) =>
        {
            asset.Result.transform.position = transform.position;
        };
    }

    private void Destruction(int tileParticleIndex)
    {
        Trigger(true);
        ExplosionActivity();
        LevelGenerator.ChangeTiles.UpdateTiles(transform.position);
        StoreForLaterUse();
    }
}
