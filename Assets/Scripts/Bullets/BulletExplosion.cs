using System;
using UnityEngine;

public class BulletExplosion : GetBulletController, IBulletExplosion
{
    protected LavaSplash _lavaSplash;

    public Action<IScore, float> OnBulletExplosion { get; set; }
    public Action<IScore, float, Vector3?> OnFlareBulletExplosion { get; set; }



    protected override void Awake()
    {
        base.Awake();
        _lavaSplash = FindObjectOfType<LavaSplash>();
    }

    protected virtual void OnEnable()
    {
        if (_iBulletCollision != null) 
            _iBulletCollision.OnExplodeOnCollision += OnExplodeOnCollision;

        if (_iBulletLimit != null)
            _iBulletLimit.OnDestroyTimeLimit += delegate { OnExplodeOnCollision(_iBulletId.OwnerScore, 0); };

        if (_iBulletLimit != null) 
            _iBulletLimit.OnExplodeOnLimit += OnExplodeOnLimit;
    }

    protected virtual void OnDisable()
    {
        if (_iBulletCollision != null)
            _iBulletCollision.OnExplodeOnCollision -= OnExplodeOnCollision;

        if (_iBulletLimit != null)
            _iBulletLimit.OnDestroyTimeLimit -= delegate { OnExplodeOnCollision(_iBulletId.OwnerScore, 0); };

        if (_iBulletLimit != null)
            _iBulletLimit.OnExplodeOnLimit -= OnExplodeOnLimit;
    }

    protected virtual void OnExplodeOnCollision(IScore ownerScore, float distance)
    {
        OnBulletExplosion?.Invoke(ownerScore, distance);
        DestroyBullet();
    }

    protected virtual void OnExplodeOnLimit(bool isTrue)
    {
        if (isTrue)
        {
            _lavaSplash.ActivateSmallSplash(transform.position);
            DestroyBullet();
        }
    }

    protected virtual void SetTurnToTransition() => _iTurnController.TurnController.SetNextTurn(TurnState.Transition);

    public virtual void DestroyBullet()
    {
        SetTurnToTransition();
        Destroy(gameObject);
    }
}
