using UnityEngine;

public abstract class PlayerDropBoxObserver : MonoBehaviour
{
    [SerializeField]
    protected BasePlayerTankController<BasePlayer> _playerTankController;

    protected bool _isAllowed;

    protected int _activeTurnsCount;



    protected virtual void OnEnable()
    {
        DropBoxSelectionHandler.onItemSelect += OnItemSelect;

        GameSceneObjectsReferences.TurnController.OnTurnChanged += OnTurnController;
    }

    protected virtual void OnDisable()
    {
        DropBoxSelectionHandler.onItemSelect -= OnItemSelect;

        GameSceneObjectsReferences.TurnController.OnTurnChanged += OnTurnController;
    }

    protected abstract void OnItemSelect(DropBoxSelectionHandler.DropBoxItemType dropBoxItemType, object[] data);

    protected abstract void OnTurnController(TurnState turnState);
}
