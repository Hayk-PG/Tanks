
public class OfflinePlayerXpBooster : PlayerDropBoxObserver
{
    protected int _xpMultiplier;



    protected override bool IsAllowed(DropBoxItemType dropBoxItemType)
    {
        return dropBoxItemType == DropBoxItemType.XpDoubleBoost || dropBoxItemType == DropBoxItemType.XpTripleBoost;
    }

    protected override void Execute(object[] data)
    {
        _price = (int)data[0];
        _quantity = (int)data[1] + GameSceneObjectsReferences.TurnController.TurnCyclesCount;
        _xpMultiplier = (int)data[2];

        _playerTurnState = _playerTankController.OwnTank.gameObject.name == Names.Tank_FirstPlayer ? TurnState.Player1 : TurnState.Player2;

        _altData[0] = _quantity;

        SetPlayerScoreMultiplier(_xpMultiplier);

        ManageTurnControllerSubscription(true);

        BuffDebuffHandler.RaiseEvent(_xpMultiplier <= 2 ? BuffDebuffType.Xp2 : BuffDebuffType.Xp3, _playerTurnState, _altData);
    }

    protected virtual void SetPlayerScoreMultiplier(int xpMultiplier) => _playerTankController._scoreController.SetScoreMultiplier(xpMultiplier);

    protected override void OnTurnController(TurnState turnState)
    {
        if (GameSceneObjectsReferences.TurnController.TurnCyclesCount >= _quantity)
        {
            ManageTurnControllerSubscription(false);

            SetPlayerScoreMultiplier(1);

            return;
        }
    }
}
