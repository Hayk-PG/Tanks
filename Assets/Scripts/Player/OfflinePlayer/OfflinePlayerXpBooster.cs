
public class OfflinePlayerXpBooster : PlayerDropBoxObserver
{
    protected int _xpMultiplier;



    protected override bool IsAllowed(DropBoxItemType dropBoxItemType)
    {
        return dropBoxItemType == DropBoxItemType.XpDoubleBoost || dropBoxItemType == DropBoxItemType.XpTripleBoost;
    }

    protected override void Execute(object[] data)
    {
        base.Execute(data);

        SetPlayerScoreMultiplier(_xpMultiplier);
    }

    protected override void RetrieveData(object[] data)
    {
        base.RetrieveData(data);

        AssignXpMultiplier(data);
    }

    protected override void RaiseBuffDebuffEvent(BuffDebuffType buffDebuffType = BuffDebuffType.None, IBuffDebuffUIElementController buffDebuffUIElementController = null)
    {
        base.RaiseBuffDebuffEvent(_xpMultiplier <= 2 ? BuffDebuffType.Xp2 : BuffDebuffType.Xp3, buffDebuffUIElementController);
    }

    protected override void OnTurnController(TurnState turnState)
    {
        base.OnTurnController(turnState);

        SetPlayerScoreMultiplier(1);
    }

    protected virtual void AssignXpMultiplier(object[] data) => _xpMultiplier = (int)data[2];

    protected virtual void SetPlayerScoreMultiplier(int xpMultiplier) => _playerTankController._scoreController.SetScoreMultiplier(xpMultiplier);
}
