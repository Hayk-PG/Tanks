
public class OfflinePlayerTankSpawner : BasePlayerTankSpawner<OfflinePlayerTankController>
{
    protected override void CacheSpawnedTank(TankController tank)
    {
        _tankController.CacheTank(tank);
    }
}
