using UnityEngine;

[System.Serializable] public struct TileModifyGUIElement
{
    public GameObject _guiElement;
    public Vector3 _corespondentPosition;
}

public class TileModifyGUI : MonoBehaviour
{
    private Canvas _canvas;
    private GlobalTileController _globalTileController;
    private TilesData _tilesData;
    private ChangeTiles _changeTiles;
    private Tab_TileModify _tabTileModify;
    private Vector3 _currentTilePosition;

    public TileModifyGUIElement _tileModifyGUIElement;


    private void Awake()
    {
        _canvas = FindObjectOfType<Canvas>();
        _globalTileController = FindObjectOfType<GlobalTileController>();
        _tilesData = FindObjectOfType<TilesData>();
        _changeTiles = FindObjectOfType<ChangeTiles>();
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
            _globalTileController.Modify(newTilePosition);
            _tileModifyGUIElement._guiElement.SetActive(false);
            _tabTileModify.SubtractScore();
        }
    }
}
