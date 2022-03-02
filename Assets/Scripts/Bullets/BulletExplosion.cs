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
        if (_iBulletCollision != null) _iBulletCollision.OnExplodeOnCollision = OnExplodeOnCollision;
        if (_iBulletLimit != null) _iBulletLimit.OnExplodeOnLimit = OnExplodeOnLimit;
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
        _iTurnController.TurnController.SetNextTurn(TurnState.Transition);
        Destroy(gameObject);
    }
}
