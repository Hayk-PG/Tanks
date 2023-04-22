using UnityEngine;

public class BaseChangeTiles : MonoBehaviour
{
    [SerializeField]
    protected TilesData _tileData;

    [SerializeField] [Space]
    protected MapPoints _mapPoints;

    [SerializeField] [Space]
    protected GlobalTileController _globalTileController;

    protected Tile _newTile;    
    
    protected Vector3 _thisTilePos;

    public Vector3 Vertical { get => new Vector3(0, _tileData.Size, 0); }
    public Vector3 Horizontal { get => new Vector3(_tileData.Size, 0, 0); }

  

    protected virtual void OnEnable()
    {
        _globalTileController.OnCreateNewTile += OnGlobalTileController;
    }

    protected virtual void OnDisable()
    {
        _globalTileController.OnCreateNewTile -= OnGlobalTileController;
    }

    protected void SetTile(Vector3 pos, Tile tile)
    {
        if (tile != null)
            UpdateTile(pos, tile);
    }

    protected void UpdateTile(Vector3 pos, Tile tile)
    {
        if (_tileData.TilesDict[pos].name != tile.name)
            OnTileUpdates(pos, tile);
    }

    private void OnTileUpdates(Vector3 pos, Tile tile)
    {
        if (!_tileData.TilesDict[pos].GetComponent<Tile>().IsProtected)
            InstantiateNewTile(pos, tile);
    }

    private void InstantiateNewTile(Vector3 pos, Tile tile)
    {
        Destroy(_tileData.TilesDict[pos]);

        _tileData.TilesDict.Remove(pos);

        Tile newTile = Instantiate(tile, pos, Quaternion.identity, transform);
        newTile.name = tile.name;

        _tileData.TilesDict.Add(pos, newTile.gameObject);
    }

    protected virtual void OnGlobalTileController(Vector3 newTilePosition) => ModifyTiles(newTilePosition);

    private void ModifyTiles(Vector3 newTilePosition)
    {
        Tile newTile = Instantiate(LevelGenerator.TilesData.TilesPrefabs[0], newTilePosition, Quaternion.identity, transform);
        newTile.name = LevelGenerator.TilesData.TilesPrefabs[0].name;
        _tileData.TilesDict.Add(newTilePosition, newTile.gameObject);
    }

    public bool HasTile(Vector3 pos)
    {
        return _tileData.TilesDict.ContainsKey(pos) && _tileData.TilesDict[pos].gameObject != null ? true :
               _tileData.TilesDict.ContainsKey(pos) && _tileData.TilesDict[pos].gameObject == null ? false :
               !_tileData.TilesDict.ContainsKey(pos) ? false : false;
    }
}
