using System;
using UnityEngine;

public class Tile : MonoBehaviour, IDestruct
{
    [SerializeField]
    private GameObject _explosion;

    private Collider _collider;
    private ChangeTiles _changeTiles;
    private TilesData _tilesData;
    private GroundSlam _groundSlam;
    private Sandbags _sandbags;
    private TileParticles _tileParticles;
    private TileProps _tileProps;

    [SerializeField]
    private bool _isSuspended, _isProtected;
    private Vector3 _desiredPosition;
    private Vector3 _tileSize;

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



    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _changeTiles = FindObjectOfType<ChangeTiles>();
        _tilesData = FindObjectOfType<TilesData>();
        _groundSlam = FindObjectOfType<GroundSlam>();
        _tileParticles = Get<TileParticles>.FromChild(gameObject);
        _tileProps = Get<TileProps>.From(gameObject);
        _tileSize = _collider.bounds.size;
    }

    private void OnEnable()
    {
        ResetTile();
        //_changeTiles.OnTilesUpdated += OnTilesUpdated;
    }

    private void OnDisable()
    {       
        //_changeTiles.OnTilesUpdated -= OnTilesUpdated;
    }

    private void Update()
    {
        //MoveTheTileDown();
    }

    public void ResetTile()
    {
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

    public void StoreForLaterUse()
    {
        _tilesData.TilesDict.Remove(transform.position);
        _tilesData.StoredInactiveTiles.Find(tile => tile.TileName == name).Tiles?.Add(this);
        transform.SetParent(_tilesData.IntactiveTilesContainer);
        gameObject.SetActive(false);
    }

    private void MoveTheTileDown()
    {
        if (IsSuspended && transform.position.y > _desiredPosition.y)
        {
            transform.Translate(Vector3.down * 5 * Time.deltaTime);

            if (transform.position.y <= _desiredPosition.y)
            {
                SnapTheTileToTheDesiredPosition();               
            }
        }
    }

    private void SnapTheTileToTheDesiredPosition()
    {
        transform.position = _desiredPosition;
        _groundSlam.OnGroundSlam(new Vector3(transform.position.x, transform.position.y - (_tileSize.y / 2), transform.position.z));
        IsSuspended = false;
        _changeTiles.UpdateTiles(transform.position);
    }

    public void Destruct(int damage, int tileParticleIndex)
    {
        Health -= IsProtected ? damage : damage * 10;
        OnTileHealth?.Invoke(Health);

        if (Health <= 0)
        {
            IsProtected = false;
            _sandbags = Get<Sandbags>.FromChild(gameObject);
            _sandbags?.OnSandbags?.Invoke(false);
            Destruction(tileParticleIndex);
        }
    }

    private void TileParticlesActivity(int tileParticleIndex)
    {
        if (_tileParticles != null)
        {
            Vector3 bottomTile = transform.position - new Vector3(0, 0.5f, 0);

            if (_changeTiles.HasTile(bottomTile))
            {
                if (_tilesData.TilesDict[bottomTile].name != Names.LS || _tilesData.TilesDict[bottomTile].name != Names.RS)
                {
                    _tileParticles.TileParticlesActivity(tileParticleIndex, _tilesData, bottomTile, true);
                }
            }
        }
    }

    private void Trigger(bool isTrigger)
    {
        _collider.isTrigger = isTrigger;
    }

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
        _changeTiles.UpdateTiles(transform.position);
        StoreForLaterUse();
    }

    private void ReplaceTheTile(TilesData TilesGenerator, Vector3 pos, GameObject currentTile)
    {
        if (TilesGenerator.TilesDict.ContainsKey(pos))
        {
            TilesGenerator.TilesDict.Remove(pos);
            TilesGenerator.TilesDict.Add(pos, currentTile);
        }
        else
        {
            TilesGenerator.TilesDict.Add(pos, currentTile);
        }
    }

    private void PrepareToMoveTheTileDown(TilesData TilesGenerator, Vector3 desiredPosition)
    {
        TilesGenerator.TilesDict.Remove(transform.position);
        IsSuspended = true;
        _desiredPosition = desiredPosition;
        ReplaceTheTile(TilesGenerator, _desiredPosition, gameObject);
    }

    private void OnTilesUpdated(TilesData TilesGenerator)
    {
        if(!_changeTiles.HasTile(transform.position - _changeTiles.Vertical))
        {
            for (int i = 2; i < 6; i++)
            {
                int step = i;

                if (_changeTiles.HasTile(transform.position - (_changeTiles.Vertical * step)))
                {
                    if (!Get<Tile>.From(_tilesData.TilesDict[transform.position - (_changeTiles.Vertical * step)]).IsProtected)
                        PrepareToMoveTheTileDown(TilesGenerator, transform.position - (_changeTiles.Vertical * (step - 1)));

                    break;
                }
            }
        }
    }
}
