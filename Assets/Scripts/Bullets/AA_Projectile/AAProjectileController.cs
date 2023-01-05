public class AAProjectileController : BulletSensorController
{
    protected override void Start()
    {
        StartCoroutine(DestroyOnTimeLimit(4));
    }
}
