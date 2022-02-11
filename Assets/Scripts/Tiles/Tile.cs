using UnityEngine;

public class Tile : MonoBehaviour, IDamage
{
    private Collider _collider;
    private ChangeTiles _changeTiles;
    private CameraShake _cameraShake;

    [SerializeField]
    private GameObject _explosion;

    private bool _isSuspended;
    private Vector3 _desiredPosition;


    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _changeTiles = FindObjectOfType<ChangeTiles>();
        _cameraShake = FindObjectOfType<CameraShake>();
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
            transform.Translate(Vector3.down * 2 * Time.deltaTime);

            if (transform.position.y <= _desiredPosition.y)
            {
                SnapTheTileToTheDesiredPosition();
                _cameraShake.Shake();
            }
        }
    }

    private void SnapTheTileToTheDesiredPosition()
    {
        transform.position = _desiredPosition;       
        _isSuspended = false;
        _changeTiles.UpdateTiles();
    }

    public void Damage(float damage)
    {
        _collider.isTrigger = true;
        _explosion.SetActive(true);
        _explosion.transform.parent = null;       
        _changeTiles.UpdateTiles();
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
