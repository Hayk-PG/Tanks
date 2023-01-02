using UnityEngine;

[System.Serializable] public struct TileModifyGUIElement
{
    public GameObject _guiElement;
    public Vector3 _corespondentPosition;
}

public class TileModifyGUI : MonoBehaviour
{
    [SerializeField] private TileModifyGUIElement _tileModifyGUIElement;
    [SerializeField] private TileModifyGUIElement _bridgeModifyGUIElement;

    private Canvas _canvas;
    private GlobalTileController _globalTileController;
    private TilesData _tilesData;
    private ChangeTiles _changeTiles;
    private TileModifyManager _tileModifyManager;   
    private delegate void CustomDelegate();

    private Vector3 CurrentTilePosition
    {
        get => transform.parent.position;
    }
    private CustomDelegate OnClickAction { get; set; }

    

    private void GetObjects()
    {
        if (_tilesData != null)
            return;

        _canvas = FindObjectOfType<Canvas>();
        _canvas.worldCamera = Camera.main;
        _globalTileController = FindObjectOfType<GlobalTileController>();
        _tilesData = FindObjectOfType<TilesData>();
        _changeTiles = FindObjectOfType<ChangeTiles>();
        _tileModifyManager = FindObjectOfType<TileModifyManager>();
    }

    private void DefineOnClickAction(TileModifyManager.TileModifyType tileModifyType)
    {
        switch (tileModifyType)
        {
            case TileModifyManager.TileModifyType.BuildBasicTiles:
                OnClickAction = delegate
                {
                    Vector3 newTilePosition = CurrentTilePosition + _tileModifyGUIElement._corespondentPosition;
                    _globalTileController.Modify(newTilePosition);
                };
                break;

            case TileModifyManager.TileModifyType.BuildConcreteTiles:
                OnClickAction = delegate
                {
                    _globalTileController.Modify(CurrentTilePosition, TileModifyManager.TileModifyType.BuildConcreteTiles);
                };
                break;

            case TileModifyManager.TileModifyType.UpgradeToConcreteTiles:
                OnClickAction = delegate
                {
                    _globalTileController.Modify(CurrentTilePosition, TileModifyManager.TileModifyType.UpgradeToConcreteTiles);
                };
                break;
            case TileModifyManager.TileModifyType.ExtendBasicTiles:
                OnClickAction = delegate
                {
                    int random = Random.Range(0, 2);
                    bool hasTileOnRight = _changeTiles.HasTile(CurrentTilePosition + _bridgeModifyGUIElement._corespondentPosition) && _tilesData.TilesDict.ContainsKey(CurrentTilePosition + _bridgeModifyGUIElement._corespondentPosition);
                    bool hasTileOnLeft = _changeTiles.HasTile(CurrentTilePosition - _bridgeModifyGUIElement._corespondentPosition) && _tilesData.TilesDict.ContainsKey(CurrentTilePosition - _bridgeModifyGUIElement._corespondentPosition);

                    Vector3? position = null;

                    if (!hasTileOnRight && hasTileOnLeft)
                    {
                        position = CurrentTilePosition + _bridgeModifyGUIElement._corespondentPosition;
                    }

                    if(hasTileOnRight && !hasTileOnLeft)
                    {
                        position = CurrentTilePosition - _bridgeModifyGUIElement._corespondentPosition;
                    }

                    if (!hasTileOnRight && !hasTileOnLeft)
                    {
                        position = random < 1 ? CurrentTilePosition + _bridgeModifyGUIElement._corespondentPosition : CurrentTilePosition - _bridgeModifyGUIElement._corespondentPosition;
                    }

                    if (position != null)
                    {
                        _globalTileController.Modify(position.Value);
                    }
                };
                break;
        }
    }

    private bool CanGUIElementBeActive(TileModifyManager.TileModifyType tileModifyType)
    {
        if (_tileModifyGUIElement._guiElement != null)
        {
            Vector3 top = CurrentTilePosition + _tileModifyGUIElement._corespondentPosition;

            return tileModifyType == TileModifyManager.TileModifyType.BuildBasicTiles ? IsAbleToBuildBasicTiles(top) :
                   tileModifyType == TileModifyManager.TileModifyType.BuildConcreteTiles ? IsAbleToBuildConcreteTiles(top) :
                   tileModifyType == TileModifyManager.TileModifyType.UpgradeToConcreteTiles ? IsAbleToUpgradeToConcreteTiles(top) :
                   tileModifyType == TileModifyManager.TileModifyType.ExtendBasicTiles ? IsAbleToExtendBasicTiles() :
                   false;
        }
        else
            return false;
    }

    private bool CanBridgeGUIElementBeActive(TileModifyManager.TileModifyType tileModifyType)
    {
        return false;
    }

    private bool IsAbleToBuildBasicTiles(Vector3 position)
    {
        return !_changeTiles.HasTile(position) && !_tilesData.TilesDict.ContainsKey(position);
    }

    private bool IsAbleToBuildConcreteTiles(Vector3 position)
    {
        return IsAbleToBuildBasicTiles(position) && !_tilesData.TilesDict[CurrentTilePosition].GetComponent<Tile>().IsProtected;
    }

    private bool IsAbleToUpgradeToConcreteTiles(Vector3 position)
    {
        return IsAbleToBuildConcreteTiles(position);
    }

    private bool IsAbleToExtendBasicTiles()
    {
        float tileSize = _bridgeModifyGUIElement._corespondentPosition.x;
        Vector3 right = new Vector3(tileSize, 0, 0);
        Vector3 left = new Vector3(-tileSize, 0, 0);
        Vector3 bottom = new Vector3(0, -tileSize, 0);
        Vector3 up = new Vector3(0, tileSize, 0);

        bool noTileOnRight = !_changeTiles.HasTile(CurrentTilePosition + right) && !_tilesData.TilesDict.ContainsKey(CurrentTilePosition + right);
        bool noTileOnLeft = !_changeTiles.HasTile(CurrentTilePosition + left) && !_tilesData.TilesDict.ContainsKey(CurrentTilePosition + left);
        bool noTileOnRightBottom = !_changeTiles.HasTile(CurrentTilePosition + bottom + right) && !_tilesData.TilesDict.ContainsKey(CurrentTilePosition + bottom + right);
        bool noTileOnLefttBottom = !_changeTiles.HasTile(CurrentTilePosition + bottom + left) && !_tilesData.TilesDict.ContainsKey(CurrentTilePosition + bottom + left);
        bool noTileOnRightUp = !_changeTiles.HasTile(CurrentTilePosition + up + right) && !_tilesData.TilesDict.ContainsKey(CurrentTilePosition + up + right);
        bool noTileOnLefttUp = !_changeTiles.HasTile(CurrentTilePosition + up + left) && !_tilesData.TilesDict.ContainsKey(CurrentTilePosition + up + left);

        return noTileOnRight && noTileOnRightBottom && noTileOnRightUp ? true :
               noTileOnLeft && noTileOnLefttBottom && noTileOnLefttUp ? true :
               false;
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

    public void EnableGUI(TileModifyManager.TileModifyType tileModifyType)
    {
        GetObjects();
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
        if (_tileModifyManager.CanModifyTiles)
        {
            DisableGUI();
            _tileModifyManager.SubtractScore();
            OnClickAction?.Invoke();                    
        }
    }
}
