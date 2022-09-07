public class ArtilleryBulletExplosion : BulletExplosion
{
    private BulletController _bulletController;


    protected override void Awake()
    {
        base.Awake();

        _bulletController = Get<BulletController>.From(gameObject);
    }

    protected override void DestroyBullet()
    {
        base.DestroyBullet();
    }

    protected override void SetTurnToTransition()
    {
        if (_bulletController.IsLastShellOfBarrage)
            base.SetTurnToTransition();
    }
}
