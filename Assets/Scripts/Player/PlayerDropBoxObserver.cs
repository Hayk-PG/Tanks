using UnityEngine;

public abstract class PlayerDropBoxObserver : MonoBehaviour
{
    [SerializeField]
    protected BasePlayerTankController<BasePlayer> _playerTankController;

    protected bool _isSubscribed;

    protected abstract int Price { get; set; }
    protected abstract int Quantity { get; set; }

    protected abstract bool IsAllowed { get; set; }

    protected abstract TurnState PlayerTurnState { get; set; }



    protected virtual void OnEnable() => DropBoxSelectionHandler.onItemSelect += OnItemSelect;

    protected virtual void OnDisable() => DropBoxSelectionHandler.onItemSelect -= OnItemSelect;

    protected abstract void OnItemSelect(DropBoxItemType dropBoxItemType, object[] data);

    protected virtual void ManageTurnControllerSubscription(bool isSubscribing)
    {
        if (isSubscribing && _isSubscribed)
            return;

        if (isSubscribing)
            GameSceneObjectsReferences.TurnController.OnTurnChanged += OnTurnController;
        else
            GameSceneObjectsReferences.TurnController.OnTurnChanged += OnTurnController;

        _isSubscribed = isSubscribing;
    }

    protected abstract void Execute(object[] data);

    protected abstract void OnTurnController(TurnState turnState);
}
