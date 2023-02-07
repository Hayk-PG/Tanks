using UnityEngine;
using System;

public class BaseBulletExplosion : MonoBehaviour, IBulletExplosion
{
    [SerializeField]
    protected BaseBulletVelocity _baseBulletVelocity;

    [SerializeField] [Space]
    protected BaseBulletCollision _baseBulletCollision;

    [SerializeField] [Space]
    protected bool _dontDestroyOnTimeLimit;

    public event Action onExplode;



    protected virtual void Start()
    {
        Invoke("DestroyOnTimeLimit", 8);
    }

    protected virtual void OnEnable()
    {
        _baseBulletVelocity.onVerticalLimit += DestroyOnVerticalLimit;
        _baseBulletCollision.onCollision += Explode;
    }

    protected virtual void OnDisable()
    {
        _baseBulletVelocity.onVerticalLimit -= DestroyOnVerticalLimit;
        _baseBulletCollision.onCollision -= Explode;
    }

    protected virtual void DestroyOnVerticalLimit()
    {
        GameSceneObjectsReferences.LavaSplash.ActivateSmallSplash(transform.position);
        DestroyBullet();
    }

    protected virtual void DestroyOnTimeLimit()
    {
        if (!_dontDestroyOnTimeLimit)
        {
            RaiseOnExplode(null);
            DestroyBullet();
        }
    }

    protected virtual void Explode(Collider collider)
    {
        RaiseOnExplode(collider);
        DestroyBullet();
    }

    protected virtual void RaiseOnExplode(Collider collider)
    {
        onExplode?.Invoke();
    }

    public virtual void DestroyBullet()
    {
        SetTurnToTransition();
        Destroy(gameObject);
    }

    protected virtual void SetTurnToTransition()
    {
        GameSceneObjectsReferences.TurnController.SetNextTurn(TurnState.Transition);
    }
}
