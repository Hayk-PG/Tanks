using UnityEngine;

public class AAProjectileExplosion : BulletSensorExplosion
{
    [SerializeField] [Space]
    protected AAProjectileVelocity _aAProjectileVelocity;



    protected override void Start()
    {
        Invoke("DestroyOnTimeLimit", 4);
    }

    protected override void OnEnable()
    {
        _aAProjectileVelocity.onVerticalLimit += DestroyOnVerticalLimit;
        _aAProjectileVelocity.onTargetDetected += delegate { Explode(null); };
    }

    protected override void OnDisable()
    {
        _aAProjectileVelocity.onVerticalLimit -= DestroyOnVerticalLimit;
        _aAProjectileVelocity.onTargetDetected -= delegate { Explode(null); };
    }

    protected override void SetTurnToTransition()
    {
        
    }
}
