using System;
using UnityEngine;
using UnityEngine.Events;

public class BulletExplosion : GetBulletController, IBulletExplosion
{
    protected CameraShake _cameraShake;
    protected LavaSplash _lavaSplash;

    public Action<IScore, float> OnBulletExplosion { get; set; }
    public Action OnBulletExplosionWithoutHitting { get; set; }
    public Action<IScore, float, Vector3?> OnFlareBulletExplosion { get; set; }

    public UnityEvent OnCameraShake;


    protected override void Awake()
    {
        base.Awake();

        _cameraShake = FindObjectOfType<CameraShake>();
        _lavaSplash = FindObjectOfType<LavaSplash>();
    }

    protected virtual void OnEnable()
    {
        if (_iBulletCollision != null) _iBulletCollision.OnExplodeOnCollision = OnExplodeOnCollision;
        if (_iBulletLimit != null) _iBulletLimit.OnExplodeOnLimit = OnExplodeOnLimit;
    }

    protected virtual void OnExplodeOnCollision(IScore ownerScore, float distance)
    {
        OnBulletExplosion?.Invoke(ownerScore, distance);
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

    protected virtual void OnExplodeOnLimit(bool isTrue)
    {
        if (isTrue)
        {
            //OnBulletExplosionWithoutHitting?.Invoke();
            _lavaSplash.ActivateSmallSplash(transform.position);
            DestroyBullet();
        }
    }

    protected virtual void SetTurnToTransition()
    {
        _iTurnController.TurnController.SetNextTurn(TurnState.Transition);
    }

    protected virtual void DestroyBullet()
    {
        SetTurnToTransition();
        Destroy(gameObject);
    }
}
