using UnityEngine;

public abstract class PlayerDropBoxObserver : MonoBehaviour
{
    [SerializeField]
    protected BasePlayerTankController<BasePlayer> _playerTankController;

    protected bool _isSubscribed;

    protected int _price, _quantity;

    protected object[] _altData = new object[10];

    protected TurnState _playerTurnState;



    protected virtual void OnEnable() => DropBoxSelectionHandler.onItemSelect += OnItemSelect;

    protected virtual void OnDisable() => DropBoxSelectionHandler.onItemSelect -= OnItemSelect;

    protected abstract void Execute(object[] data);

    protected virtual void OnItemSelect(DropBoxItemType dropBoxItemType, object[] data)
    {
        if (IsAllowed(dropBoxItemType))
            Execute(data);
    }

    protected virtual bool IsAllowed(DropBoxItemType dropBoxItemType)
    {
        return false;
    }

    protected virtual void DeductScores(int price) => _playerTankController._scoreController.GetScore(price, null, null, Vector3.zero);

    protected virtual void ManageTurnControllerSubscription(bool isSubscribing)
    {
        if (isSubscribing && _isSubscribed)
            return;

        if (isSubscribing)
            GameSceneObjectsReferences.TurnController.OnTurnChanged += OnTurnController;
        else
            GameSceneObjectsReferences.TurnController.OnTurnChanged -= OnTurnController;

        _isSubscribed = isSubscribing;
    }

    protected virtual void OnTurnController(TurnState turnState)
    {

    }
}
