using UnityEngine;

[System.Serializable] public struct TileModifyGUIElement
{
    public GameObject _guiElement;
    public Vector3 _corespondentPosition;
}

public class TileModifyGUI : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    private Canvas _canvas;
    private TilesData _tilesData;
    private ChangeTiles _changeTiles;
    private LevelGenerator _levelGenerator;
    private Tab_TileModify _tabTileModify;
    private Vector3 _currentTilePosition;

    public TileModifyGUIElement _tileModifyGUIElement;


    private void Awake()
    {
        _canvas = FindObjectOfType<Canvas>();
        _tilesData = FindObjectOfType<TilesData>();
        _changeTiles = FindObjectOfType<ChangeTiles>();
        _levelGenerator = FindObjectOfType<LevelGenerator>();
        _tabTileModify = FindObjectOfType<Tab_TileModify>();

        _canvas.worldCamera = Camera.main;
        _currentTilePosition = transform.parent.transform.position;
    }

    public void EnableGUI()
    {
        Vector3 top = _currentTilePosition + _tileModifyGUIElement._corespondentPosition;
        bool hasTileOnTopOfIt = _changeTiles.HasTile(top) && !_tilesData.TilesDict.ContainsKey(top);
       
        if (!hasTileOnTopOfIt)
        {
            _tileModifyGUIElement._guiElement.SetActive(true);
        }
        else
        {
            _tileModifyGUIElement._guiElement.SetActive(false);
        }
    }

    public void DisableGUI()
    {
        _tileModifyGUIElement._guiElement.SetActive(false);
    }

    public void OnClick()
    {
        if (_tabTileModify.CanModifyTiles)
        {
            Vector3 newTilePosition = _currentTilePosition + _tileModifyGUIElement._corespondentPosition;
            GameObject tile = Instantiate(_prefab, newTilePosition, Quaternion.identity, _levelGenerator.transform);

            if (_tilesData.TilesDict.ContainsKey(newTilePosition))
            {
                Destroy(_tilesData.TilesDict[newTilePosition]);
                _tilesData.TilesDict.Remove(newTilePosition);
            }

            _tilesData.TilesDict.Add(newTilePosition, tile);
            _tileModifyGUIElement._guiElement.SetActive(false);
            _changeTiles.UpdateTiles(newTilePosition);
            _tabTileModify.SubtractScore();
        }
    }
}
