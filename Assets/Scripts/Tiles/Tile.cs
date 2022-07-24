using UnityEngine;

public class Tile : MonoBehaviour, IDestruct
{
    private Collider _collider;
    private ChangeTiles _changeTiles;
    private GroundSlam _groundSlam;
    private Sandbags _sandbags;
    private TileParticles _tileParticles;

    [SerializeField]
    private GameObject _explosion;

    private bool _isSuspended;
    private Vector3 _desiredPosition;
    private Vector3 _tileSize;

    public bool IsProtected { get; set; }
    public float Health { get; set; } = 100;



    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _changeTiles = FindObjectOfType<ChangeTiles>();
        _groundSlam = FindObjectOfType<GroundSlam>();
        _tileParticles = Get<TileParticles>.From(gameObject);
        _tileSize = _collider.bounds.size;
    }

    private void OnEnable()
    {
        _changeTiles.OnTilesUpdated += OnTilesUpdated;
    }

    private void OnDisable()
    {
        _changeTiles.OnTilesUpdated -= OnTilesUpdated;
    }

    private void Update()
    {
        MoveTheTileDown();
    }

    private void MoveTheTileDown()
    {
        if (_isSuspended && transform.position.y > _desiredPosition.y)
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
        _isSuspended = false;
        _changeTiles.UpdateTiles();
    }

    public void Destruct(int damage, int tileParticleIndex)
    {
        if (!IsProtected)
        {
            Destruction(tileParticleIndex);
        }
        else
        {
            Health -= damage;

            if (Health <= 0)
            {
                IsProtected = false;
                _sandbags = Get<Sandbags>.FromChild(gameObject);
                _sandbags?.OnSandbags?.Invoke(false);
                Destruction(tileParticleIndex);
            }
        }
    }

    private void Destruction(int tileParticleIndex)
    {
        _collider.isTrigger = true;
        _explosion.SetActive(true);
        _explosion.transform.parent = null;
        _changeTiles.UpdateTiles();
        _tileParticles.TileParticlesActivity(tileParticleIndex, true);
        Destroy(gameObject);
    }

    private void OnTilesUpdated(TilesData TilesGenerator)
    {
        DestroyUncachedTile(TilesGenerator);

        if(!IsThereATileOnGivenPosition(transform.position - _changeTiles.Vertical))
        {
            for (int i = 2; i < 6; i++)
            {
                int step = i;

                if (IsThereATileOnGivenPosition(transform.position - (_changeTiles.Vertical * step)))
                {
                    PrepareToMoveTheTileDown(TilesGenerator, transform.position - (_changeTiles.Vertical * (step - 1)));
                    break;
                }
            }
        }
    }

    private bool IsThereATileOnGivenPosition(Vector3 pos)
    {
        return _changeTiles.HasTile(pos);
    }

    private void PrepareToMoveTheTileDown(TilesData TilesGenerator, Vector3 desiredPosition)
    {
        TilesGenerator.TilesDict.Remove(transform.position);
        _isSuspended = true;
        _desiredPosition = desiredPosition;
        ReplaceTheTile(TilesGenerator, _desiredPosition, gameObject);
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

    private void DestroyUncachedTile(TilesData TilesGenerator)
    {
        if(TilesGenerator.TilesDict.ContainsKey(transform.position) && TilesGenerator.TilesDict[transform.position] != gameObject)
        {
            Destroy(gameObject);
        }
    }
}
