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
    [SerializeField]
    private TileModifyGUIElement _bridgeModifyGUIElement;

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
            case Tab_TileModify.TileModifyType.Bridge:
                OnClickAction = delegate
                {
                    Vector3 side = CurrentTilePosition + _bridgeModifyGUIElement._corespondentPosition;
                    _globalTileController.Modify(side);
                };
                break;
        }
    }

    private bool CanGUIElementBeActive(Tab_TileModify.TileModifyType tileModifyType)
    {
        if (_tileModifyGUIElement._guiElement != null)
        {
            Vector3 top = CurrentTilePosition + _tileModifyGUIElement._corespondentPosition;
            return tileModifyType == Tab_TileModify.TileModifyType.NewTile ?
                                     !_changeTiles.HasTile(top) && !_tilesData.TilesDict.ContainsKey(top) :
                                     tileModifyType == Tab_TileModify.TileModifyType.ArmoredCube ||
                                     tileModifyType == Tab_TileModify.TileModifyType.ArmoredTile ?
                                     !_changeTiles.HasTile(top) && !_tilesData.TilesDict.ContainsKey(top) &&
                                     !_tilesData.TilesDict[CurrentTilePosition].GetComponent<Tile>().IsProtected : false;
        }
        else
            return false;
    }

    private bool CanBridgeGUIElementBeActive(Tab_TileModify.TileModifyType tileModifyType)
    {
        if (_bridgeModifyGUIElement._guiElement != null && tileModifyType == Tab_TileModify.TileModifyType.Bridge)
        {
            Vector3 side = CurrentTilePosition + _bridgeModifyGUIElement._corespondentPosition;
            Vector3 bottom = new Vector3(0, -Mathf.Abs(_bridgeModifyGUIElement._corespondentPosition.x), 0);
            return !_changeTiles.HasTile(side) && !_changeTiles.HasTile(side + bottom) ? true : false;
        }
        else return false;
    }

    private void TileModifyGUIElementActivity(bool isActive)
    {
        if (_tileModifyGUIElement._guiElement != null)
            _tileModifyGUIElement._guiElement.SetActive(isActive);
    }

    private void BridgeModifyGUIElementActivity(bool isActive)
    {
        if(_bridgeModifyGUIElement._guiElement != null)
            _bridgeModifyGUIElement._guiElement.SetActive(isActive);
    }

    public void EnableGUI(Tab_TileModify.TileModifyType tileModifyType)
    {
        DefineOnClickAction(tileModifyType);

        TileModifyGUIElementActivity(CanGUIElementBeActive(tileModifyType) ? true : false);
        BridgeModifyGUIElementActivity(CanBridgeGUIElementBeActive(tileModifyType) ? true : false);
    }

    public void DisableGUI()
    {
        TileModifyGUIElementActivity(false);
        BridgeModifyGUIElementActivity(false);
    }

    public void OnClick()
    {
        if (_tabTileModify.CanModifyTiles)
        {
            TileModifyGUIElementActivity(false);
            BridgeModifyGUIElementActivity(false);

            _tabTileModify.SubtractScore();
            OnClickAction?.Invoke();                    
        }
    }
}
