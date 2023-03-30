
public class OfflinePlayerHealthBooster : PlayerDropBoxObserver
{
    private int _hp;

    protected override bool IsAllowed(DropBoxItemType dropBoxItemType)
    {
        return dropBoxItemType == DropBoxItemType.HpBoost;
    }

    protected override void Execute(object[] data)
    {
        _price = (int)data[0];
        _hp = (int)data[1];

        _playerTankController._healthController.BoostHealth(_hp);

        DeductScores(_price);
    }
}
