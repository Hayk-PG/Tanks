
public class RailgunExplosion : BaseBulletExplosion
{
    protected override void Start() => Invoke("DestroyOnTimeLimit", 3);
}
