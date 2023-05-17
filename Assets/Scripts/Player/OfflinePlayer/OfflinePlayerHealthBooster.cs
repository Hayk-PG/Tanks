
public class OfflinePlayerHealthBooster : PlayerDropBoxObserver
{
    protected int _hp;




    protected override void Awake()
    {
        
    }

    protected override bool IsAllowed(DropBoxItemType dropBoxItemType)
    {
        return dropBoxItemType == DropBoxItemType.HpBoost;
    }

    protected override void Execute(object[] data)
    {
        RetrieveData(data);

        BoostPlayerHealth();
    }

    protected virtual void BoostPlayerHealth() => _playerTankController._healthController.BoostHealth(_hp);
}
