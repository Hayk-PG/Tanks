public class OfflinePlayerShoot : OfflinePlayerBase, IShoot
{
    public void Shoot(float force)
    {
        _offlinePlayerTankController._shootController.ShootBullet(force);
    }
}
