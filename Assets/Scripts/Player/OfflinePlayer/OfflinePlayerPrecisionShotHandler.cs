
public class OfflinePlayerPrecisionShotHandler : PlayerDropBoxObserver, IBuffDebuffUIElementController
{
    protected override bool IsAllowed(DropBoxItemType dropBoxItemType)
    {
        return dropBoxItemType == DropBoxItemType.Accuracy;
    }

    protected override void Execute(object[] data)
    {
        base.Execute(data);

        SetPrecisionShotActive(true);
    }

    protected override void RaiseBuffDebuffEvent(BuffDebuffType buffDebuffType = BuffDebuffType.None, IBuffDebuffUIElementController buffDebuffUIElementController = null)
    {
        base.RaiseBuffDebuffEvent(BuffDebuffType.Accuracy, this);
    }

    protected override void OnTurnController(TurnState turnState)
    {
        base.OnTurnController(turnState);

        SetPrecisionShotActive(false);
    }

    protected virtual void SetPrecisionShotActive(bool isPrecisionShotActive)
    {
        _playerTankController._shootController.SetPrecisionShotActive(isPrecisionShotActive);
    }
}
