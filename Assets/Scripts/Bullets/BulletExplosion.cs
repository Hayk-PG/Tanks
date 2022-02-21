using System;

public class BulletExplosion : GetBulletController, IBulletExplosion
{
    private CameraShake _cameraShake;

    public Action<IScore> OnBulletExplosion { get; set; }

    protected override void Awake()
    {
        base.Awake();

        _cameraShake = FindObjectOfType<CameraShake>();
    }

    private void OnEnable()
    {
        if (_bulletController != null) _bulletController.OnExplodeOnCollision = OnExplodeOnCollision;
        if (_bulletController != null) _bulletController.OnExplodeOnLimit = OnExplodeOnLimit;
    }

    private void OnExplodeOnCollision(IScore ownerScore)
    {
        OnBulletExplosion?.Invoke(ownerScore);       
        _cameraShake.Shake();

        DestroyBullet();
    }

    private void OnExplodeOnLimit(bool isTrue)
    {
        if(isTrue) DestroyBullet();
    }

    private void DestroyBullet()
    {
        _bulletController._turnController.SetNextTurn(TurnState.Transition);
        Destroy(gameObject);
    }
}
