
public class PhotonPlayerTankSpawner : BasePlayerTankSpawner<PhotonPlayerTankController>
{
    protected override void CacheSpawnedTank(TankController tank)
    {
        _tankController.CacheTank(tank);
    }
}
