
public class OfflinePlayerShoot : OfflinePlayerBase, IShoot
{
    public void LaunchRocket(int id)
    {
        _offlinePlayerTankController._shootController.LaunchRocket(id);
    }

    public void Shoot(float force)
    {
        _offlinePlayerTankController._shootController.ShootBullet(force);
    }
}
