public class AAMissileExplosion : BulletSensorExplosion
{
    private IAATargetDetector<BulletController.VelocityData> _iAATargetDetector;


    protected override void Awake()
    {
        base.Awake();
        _iAATargetDetector = Get<IAATargetDetector<BulletController.VelocityData>>.From(gameObject);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_iAATargetDetector != null)
            _iAATargetDetector.OnTargetDetected += delegate { Hit(default); };
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (_iAATargetDetector != null)
            _iAATargetDetector.OnTargetDetected -= delegate { Hit(default); };
    }

    public override void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
