using System;
using UnityEngine;

public class Tile : MonoBehaviour, IDestruct
{
    [SerializeField] private GameObject _explosion;
    [SerializeField] private Collider _collider;
    [SerializeField] private TileParticles _tileParticles;
    [SerializeField] private TileProps _tileProps;


    [SerializeField] private bool _isSuspended, _isProtected;
    private bool _isStored;

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



    private void OnEnable() => ResetTile();

    public void ResetTile()
    {
        if (_isStored)
        {
            _isStored = false;
            IsProtected = false;
            IsSuspended = false;
            Health = 100;
            OnTileHealth?.Invoke(Health);

            Trigger(false);
            ExplosionActivity(false);

            _tileParticles?.gameObject.SetActive(true);
            _tileParticles?.ResetTileParticles();
            _tileProps?.ActiveProps(TileProps.PropsType.All, false, null);
        }
    }

    public void StoreForLaterUse()
    {
        _isStored = true;
        LevelGenerator.TilesData.TilesDict.Remove(transform.position);
        LevelGenerator.TilesData.StoredInactiveTiles.Find(tile => tile.TileName == name).Tiles?.Add(this);
        transform.SetParent(LevelGenerator.TilesData.IntactiveTilesContainer);
        gameObject.SetActive(false);
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

    private void TileParticlesActivity(int tileParticleIndex)
    {
        if (_tileParticles != null)
        {
            Vector3 bottomTile = transform.position - new Vector3(0, 0.5f, 0);

            if (LevelGenerator.ChangeTiles.HasTile(bottomTile))
            {
                if (LevelGenerator.TilesData.TilesDict[bottomTile].name != Names.LS || LevelGenerator.TilesData.TilesDict[bottomTile].name != Names.RS)
                {
                    _tileParticles.TileParticlesActivity(tileParticleIndex, LevelGenerator.TilesData, bottomTile, true);
                }
            }
        }
    }

    private void Trigger(bool isTrigger) => _collider.isTrigger = isTrigger;

    private void ExplosionActivity(bool isActive)
    {
        _explosion.SetActive(isActive);
        _explosion.transform.parent = isActive ? null : transform;
    }

    private void Destruction(int tileParticleIndex)
    {
        Trigger(true);
        ExplosionActivity(true);
        TileParticlesActivity(tileParticleIndex);
        LevelGenerator.ChangeTiles.UpdateTiles(transform.position);
        StoreForLaterUse();
    }
}
