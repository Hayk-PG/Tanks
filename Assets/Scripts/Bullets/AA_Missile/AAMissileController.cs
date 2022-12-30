public class AAMissileController : BulletSensorController
{
    protected override void Start()
    {
        StartCoroutine(DestroyOnTimeLimit(4));
    }
}
