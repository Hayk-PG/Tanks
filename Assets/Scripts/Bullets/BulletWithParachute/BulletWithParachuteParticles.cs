using UnityEngine;

public class BulletWithParachuteParticles : BulletParticles
{
    [SerializeField]
    private GameObject _parachuteOpenExplosion;

    private BulletWithParachuteVelocity _bulletWithParachuteVelocity;


    protected override void Awake()
    {
        base.Awake();

        _bulletWithParachuteVelocity = Get<BulletWithParachuteVelocity>.From(gameObject);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _bulletWithParachuteVelocity.OnParachuteOpen += OnParachuteOpen;
    }

    private void OnParachuteOpen()
    {
        _parachuteOpenExplosion.SetActive(true);
    }
}
