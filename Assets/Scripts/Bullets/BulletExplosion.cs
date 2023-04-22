using System;
using UnityEngine;

public class BulletExplosion : MonoBehaviour, IBulletExplosion
{    
    protected IBulletCollision _iBulletCollision;
    protected IBulletLimit _iBulletLimit;
    protected IBulletID _iBulletId;
    protected ITurnController _iTurnController;
    protected LavaSplash _lavaSplash;

    public Action<IScore, float> OnBulletExplosion { get; set; }
    public Action<IScore, float, Vector3?> OnFlareBulletExplosion { get; set; }



    protected virtual void Awake()
    {
        GetIBulletCollision();
        _iBulletLimit = Get<IBulletLimit>.From(gameObject);
        _iBulletId = Get<IBulletID>.From(gameObject);
        _iTurnController = Get<ITurnController>.From(gameObject);
        _lavaSplash = FindObjectOfType<LavaSplash>();
    }

    protected virtual void OnEnable()
    {
        ListenIBulletCollision(true);
        ListenIBulletLimit(true);
    }

    protected virtual void OnDisable()
    {
        ListenIBulletCollision(false);
        ListenIBulletLimit(false);
    }

    protected virtual void GetIBulletCollision() => _iBulletCollision = Get<IBulletCollision>.From(gameObject);

    protected virtual void ListenIBulletCollision(bool isSubscribing)
    {
        if (_iBulletCollision == null)
            return;

        if (isSubscribing)
            _iBulletCollision.OnExplodeOnCollision += OnExplodeOnCollision;
        else
            _iBulletCollision.OnExplodeOnCollision -= OnExplodeOnCollision;
    }

    protected virtual void ListenIBulletLimit(bool isSubscribing)
    {
        if (_iBulletLimit == null)
            return;

        if (isSubscribing)
        {
            _iBulletLimit.OnDestroyTimeLimit += delegate { OnExplodeOnCollision(_iBulletId.OwnerScore, 0); };
            _iBulletLimit.OnExplodeOnLimit += OnExplodeOnLimit;
        }
        else
        {
            _iBulletLimit.OnDestroyTimeLimit -= delegate { OnExplodeOnCollision(_iBulletId.OwnerScore, 0); };
            _iBulletLimit.OnExplodeOnLimit -= OnExplodeOnLimit;
        }
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
