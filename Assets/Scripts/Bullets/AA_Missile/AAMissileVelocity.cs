using System;
using UnityEngine;

public class AAMissileVelocity : BulletVelocity, IAATargetDetector<BulletController.VelocityData>
{
    private bool _isTrailActive;

    public BulletController Target { get; set; }
    public Action OnTargetDetected { get; set; }



    protected override void OnBulletVelocity(BulletController.VelocityData velocityData)
    {
        bool isMissileActive = velocityData._rigidBody.velocity.y <= 1 && !_isTrailActive;
        bool isTargetDetected = _isTrailActive && Target != null;
        bool isTargetClose = isTargetDetected && Vector3.Distance(velocityData._rigidBody.position, Target.RigidBody.position) <= 0.5f;

        BulletLookRotation(velocityData);
        ScanTarget(isMissileActive);
        MoveTowardsTarget(isTargetDetected, velocityData);
        DestroyTarget(isTargetClose);
    }

    public virtual void ScanTarget(bool isConditionMet)
    {
        if (isConditionMet)
        {
            Target = FindObjectOfType<BulletController>();
            ActivateTrail();
            _isTrailActive = true;
        }
    }

    public void MoveTowardsTarget(bool isConditionMet, BulletController.VelocityData parameter)
    {
        if (isConditionMet)
            parameter._rigidBody.position = Vector3.MoveTowards(parameter._rigidBody.position, Target.RigidBody.position, 10 * Time.fixedDeltaTime);
    }

    public void DestroyTarget(bool isConditionMet)
    {
        if (isConditionMet)
        {
            IBulletExplosion iBulletExplosion = Get<IBulletExplosion>.From(Target.gameObject);
            iBulletExplosion?.DestroyBullet();
            OnTargetDetected?.Invoke();
        }
    }
}
