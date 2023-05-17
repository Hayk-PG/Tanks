using UnityEngine;

public abstract class PlayerDropBoxObserver : MonoBehaviour, IBuffDebuffUIElementController
{
    [SerializeField]
    protected BasePlayerTankController<BasePlayer> _playerTankController;

    protected TurnState _playerTurnState;

    protected bool _isSubscribed;

    protected int _price, _quantity;

    protected object[] _altData = new object[10];

    public BuffDebuffUIElement BuffDebuffUIElement { get; set; }





    protected virtual void Awake() => GetPlayerTurnState();

    protected virtual void OnEnable() => DropBoxSelectionHandler.onItemSelect += OnItemSelect;

    protected virtual void OnDisable() => DropBoxSelectionHandler.onItemSelect -= OnItemSelect;

    protected virtual void GetPlayerTurnState() => _playerTurnState = _playerTankController.OwnTank.gameObject.name == Names.Tank_FirstPlayer ? TurnState.Player1 : TurnState.Player2;

    protected virtual void OnItemSelect(DropBoxItemType dropBoxItemType, object[] data)
    {
        if (IsAllowed(dropBoxItemType))
        {
            Execute(data);

            DeductScores();
        }
    }

    protected abstract bool IsAllowed(DropBoxItemType dropBoxItemType);

    protected virtual void Execute(object[] data)
    {
        RetrieveData(data);

        ManageTurnControllerSubscription(true);

        RaiseBuffDebuffEvent();
    }

    protected virtual void RetrieveData(object[] data)
    {
        AssignPrice(data);

        AssignQuantity(data);

        AssignAltData();
    }

    /// <summary>
    /// Caches the cost of the selected 'DropBox' item.
    /// </summary>
    /// <param name="data">An array of data, with the price value at index 0 by default.</param>
    protected virtual void AssignPrice(object[] data) => _price = (int)data[0];

    /// <summary>
    /// Caches the quantity of the selected 'DropBox' item.
    /// </summary>
    /// <param name="data">An array of data, with the quantity value at index 1 by default.</param>
    protected virtual void AssignQuantity(object[] data) => _quantity = (int)data[1];

    protected virtual void AssignAltData() => _altData[0] = _quantity;

    protected virtual void DeductScores() => _playerTankController._scoreController.GetScore(_price, null, null, Vector3.zero);

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

    /// <summary>
    /// Activates 'BuffDebuff' icon
    /// </summary>
    /// <param name="buffDebuffType"></param>
    /// <param name="buffDebuffUIElementController">Uses to cache the 'BuffDebuff'</param>
    protected virtual void RaiseBuffDebuffEvent(BuffDebuffType buffDebuffType = BuffDebuffType.None, IBuffDebuffUIElementController buffDebuffUIElementController = null)
    {
        BuffDebuffHandler.RaiseEvent(buffDebuffType, _playerTurnState, buffDebuffUIElementController, _altData);
    }

    protected virtual void OnTurnController(TurnState turnState)
    {
        float turnCycleCount = GameSceneObjectsReferences.TurnController.TurnCyclesCount;
        float duration = _quantity * 2;

        SetBuffDebuffIconFillAmount(turnState, turnCycleCount, duration);

        bool isSufficientTurnCycles = turnCycleCount >= duration;

        if (!isSufficientTurnCycles)
            return;

        ManageTurnControllerSubscription(false);

        DeactivateBuffDebuff();
    }

    protected virtual void SetBuffDebuffIconFillAmount(TurnState turnState, float turnCycleCount, float duration)
    {
        if (turnState == _playerTurnState)
            BuffDebuffUIElement?.ControlImageFill(Mathf.InverseLerp(0, duration, turnCycleCount));
    }

    protected virtual void DeactivateBuffDebuff() => BuffDebuffUIElement?.Deactivate();

    public virtual void AssignBuffDebuffUIElement(BuffDebuffUIElement buffDebuffUIElement) => BuffDebuffUIElement = buffDebuffUIElement;
}
