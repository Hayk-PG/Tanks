using UnityEngine;

[System.Serializable] public struct TileModifyGUIElement
{
    public GameObject _guiElement;
    public Vector3 _corespondentPosition;
}

public class TileModifyGUI : MonoBehaviour
{
    [SerializeField] 
    private TileModifyGUIElement _tileModifyGUIElement;
    private Canvas _canvas;
    private GlobalTileController _globalTileController;
    private TilesData _tilesData;
    private ChangeTiles _changeTiles;
    private Tab_TileModify _tabTileModify;   
    private delegate void CustomDelegate();

    private Vector3 CurrentTilePosition
    {
        get => transform.parent.position;
    }
    private CustomDelegate OnClickAction { get; set; }

    

    private void Awake()
    {
        _canvas = FindObjectOfType<Canvas>();
        _globalTileController = FindObjectOfType<GlobalTileController>();
        _tilesData = FindObjectOfType<TilesData>();
        _changeTiles = FindObjectOfType<ChangeTiles>();
        _tabTileModify = FindObjectOfType<Tab_TileModify>();
        _canvas.worldCamera = Camera.main;
    }

    private void DefineOnClickAction(Tab_TileModify.TileModifyType tileModifyType)
    {
        switch (tileModifyType)
        {
            case Tab_TileModify.TileModifyType.NewTile:
                OnClickAction = delegate
                {
                    Vector3 newTilePosition = CurrentTilePosition + _tileModifyGUIElement._corespondentPosition;
                    _globalTileController.Modify(newTilePosition);
                };
                break;

            case Tab_TileModify.TileModifyType.ArmoredCube:
                OnClickAction = delegate
                {
                    _globalTileController.Modify(CurrentTilePosition, Tab_TileModify.TileModifyType.ArmoredCube);
                };
                break;

            case Tab_TileModify.TileModifyType.ArmoredTile:
                OnClickAction = delegate
                {
                    _globalTileController.Modify(CurrentTilePosition, Tab_TileModify.TileModifyType.ArmoredTile);
                };
                break;
        }
    }

    private bool CanGUIElementBeActive(Tab_TileModify.TileModifyType tileModifyType)
    {
        Vector3 top = CurrentTilePosition + _tileModifyGUIElement._corespondentPosition;
        return tileModifyType == Tab_TileModify.TileModifyType.NewTile ?
                                 !_changeTiles.HasTile(top) && !_tilesData.TilesDict.ContainsKey(top) :
                                 tileModifyType == Tab_TileModify.TileModifyType.ArmoredCube ||
                                 tileModifyType == Tab_TileModify.TileModifyType.ArmoredTile ?
                                 !_changeTiles.HasTile(top) && !_tilesData.TilesDict.ContainsKey(top) &&
                                 !_tilesData.TilesDict[CurrentTilePosition].GetComponent<Tile>().IsProtected : false;
    }

    public void EnableGUI(Tab_TileModify.TileModifyType tileModifyType)
    {
        DefineOnClickAction(tileModifyType);

        if (CanGUIElementBeActive(tileModifyType))
            _tileModifyGUIElement._guiElement.SetActive(true);
        else
            _tileModifyGUIElement._guiElement.SetActive(false);
    }

    public void DisableGUI()
    {
        _tileModifyGUIElement._guiElement.SetActive(false);
    }

    public void OnClick()
    {
        if (_tabTileModify.CanModifyTiles)
        {
            _tileModifyGUIElement._guiElement.SetActive(false);
            _tabTileModify.SubtractScore();
            OnClickAction?.Invoke();                    
        }
    }
}
