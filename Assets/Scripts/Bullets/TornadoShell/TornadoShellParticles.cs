using UnityEngine;

public class TornadoShellParticles : FlareBulletParticles
{
    private IBulletID _iBulletId;
    private IBulletSensor _iBulletSensor;
    

    protected override void Awake()
    {
        base.Awake();
        _iBulletId = Get<IBulletID>.From(gameObject);
        _iBulletSensor = Get<IBulletSensor>.FromChild(gameObject);   
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_iBulletSensor != null)
            _iBulletSensor.OnHit += delegate(RaycastHit hit) { OnCollision(hit, _iBulletId.OwnerScore, _iBulletId.Distance); };
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (_iBulletSensor != null)
            _iBulletSensor.OnHit -= delegate (RaycastHit hit) { OnCollision(hit, _iBulletId.OwnerScore, _iBulletId.Distance); };
    }

    protected override void OnCollision(RaycastHit hit, IScore ownerScore, float distance)
    {
        if (_iBulletId != null)
        {
            ActivateExplosion(hit);
            _explosion.OwnerScore = ownerScore;
            _iBulletId.TurnController.SetNextTurn(TurnState.Transition);
            Destroy(gameObject);
        }
    }

    protected override void OnExplosion(IScore ownerScore, float distance)
    {
        
    }
}
