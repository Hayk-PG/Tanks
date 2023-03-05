using UnityEngine;

public class RocketExplosion : BaseBulletExplosion
{
    [SerializeField] [Space]
    protected RocketController _rocketController;



    protected override void Start()
    {
        Invoke("DestroyOnTimeLimit", 20);
    }

    public override void DestroyBullet()
    {
        _rocketController.SwitchRocketControllerTab(false);

        base.DestroyBullet();
    }
}
