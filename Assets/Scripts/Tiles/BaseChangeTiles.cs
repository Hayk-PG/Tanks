using UnityEngine;

public class BaseChangeTiles : MonoBehaviour
{
    public enum TileCreationMode { Instantiate, Pool}
    public TileCreationMode _tileCreationMode;

    protected Tile _newTile;
    protected TilesData _tileData;
    protected MapPoints _mapPoints;
    protected GlobalTileController _globalTileController;
    protected Vector3 _thisTilePos;

    public Vector3 Vertical { get => new Vector3(0, _tileData.Size, 0); }
    public Vector3 Horizontal { get => new Vector3(_tileData.Size, 0, 0); }


    protected virtual void Awake()
    {
        _tileData = Get<TilesData>.From(gameObject);
        _mapPoints = Get<MapPoints>.From(gameObject);
        _globalTileController = FindObjectOfType<GlobalTileController>();
    }

    protected virtual void OnEnable()
    {
        _globalTileController.OnCreateNewTile += OnGlobalTileController;
    }

    protected virtual void OnDisable()
    {
        _globalTileController.OnCreateNewTile -= OnGlobalTileController;
    }

    public bool HasTile(Vector3 pos)
    {
        return _tileData.TilesDict.ContainsKey(pos) && _tileData.TilesDict[pos].gameObject != null ? true :
               _tileData.TilesDict.ContainsKey(pos) && _tileData.TilesDict[pos].gameObject == null ? false :
               !_tileData.TilesDict.ContainsKey(pos) ? false : false;
    }

    protected void UpdateTile(Vector3 pos, Tile tile)
    {
        if(_tileData.TilesDict[pos].name != tile.name)
        {
            OnTileUpdates(pos, tile);
        }
    }

    private void InstantiateNewTile(Vector3 pos, Tile tile)
    {
        Destroy(_tileData.TilesDict[pos]);
        _tileData.TilesDict.Remove(pos);
        Tile newTile = Instantiate(tile, pos, Quaternion.identity);
        newTile.name = tile.name;
        _tileData.TilesDict.Add(pos, newTile.gameObject);
        newTile.transform.SetParent(transform);
    }

    private void PoolNewTile(Vector3 pos, Tile newTile)
    {       
        _newTile = newTile;
        _newTile.transform.SetParent(transform);
        _newTile.transform.position = pos;
        _newTile.gameObject.SetActive(true);
        _tileData.TilesDict.Add(pos, _newTile.gameObject);
    }

    private void OnTileUpdates(Vector3 pos, Tile tile)
    {
        if (!_tileData.TilesDict[pos].GetComponent<Tile>().IsProtected)
        {
            if (_tileCreationMode == TileCreationMode.Instantiate)
            {
                InstantiateNewTile(pos, tile);
            }
            else
            {
                Get<Tile>.From(_tileData.TilesDict[pos])?.StoreForLaterUse();
                PoolNewTile(pos, _tileData.StoredInactiveTiles.Find(storedTile => storedTile.TileName == tile.name).Tiles[0]);
                _tileData.StoredInactiveTiles.Find(storedTile => storedTile.TileName == tile.name).Tiles.RemoveAt(0);
            }
        }
    }

    protected void SetTile(Vector3 pos, Tile tile)
    {
        if(tile != null)
        {
            UpdateTile(pos, tile);
        }
    }

    protected virtual void OnGlobalTileController(Vector3 newTilePosition)
    {
        if(LevelGenerator.ModifiableTilesLoader.Container != null && LevelGenerator.ModifiableTilesLoader.Container.transform.childCount > 0)
        {
            Tile tile = Get<Tile>.From(LevelGenerator.ModifiableTilesLoader.Container.transform.GetChild(0).gameObject);
            PoolNewTile(newTilePosition, tile);
            LevelGenerator.ModifiableTilesLoader.TrackTilesCount();
        }
    }
}
