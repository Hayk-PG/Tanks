using System;
using UnityEngine.Events;

public class BulletExplosion : GetBulletController, IBulletExplosion
{
    private CameraShake _cameraShake;

    public Action<IScore> OnBulletExplosion { get; set; }
    public UnityEvent OnCameraShake;


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
        OnCameraShake?.Invoke();

        DestroyBullet();
    }

    public void CameraShake()
    {
        _cameraShake.Shake();
    }

    public void CameraBigShake()
    {
        _cameraShake.BigShake();
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
