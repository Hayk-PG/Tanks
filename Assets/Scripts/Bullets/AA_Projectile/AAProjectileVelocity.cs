using System;
using UnityEngine;

public class AAProjectileVelocity : BulletVelocity, IAATargetDetector<BulletController.VelocityData>
{
    private PhotonNetworkAALauncher _photonNetworkAALauncher;

    private bool _isTrailActive;

    public BulletController Target { get; set; }
    public Action OnTargetDetected { get; set; }



    protected override void Awake()
    {
        base.Awake();
        _photonNetworkAALauncher = FindObjectOfType<PhotonNetworkAALauncher>();
    }

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
            Target = GlobalFunctions.ObjectsOfType<BulletController>.Find(bullet => bullet.transform.position != transform.position);
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
            Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, DestroyTarget, () => DestroyTarget(Target.OwnerScore.PlayerTurn.gameObject.name));
            OnTargetDetected?.Invoke();
        }
    }

    private void DestroyTarget()
    {
        IBulletExplosion iBulletExplosion = Get<IBulletExplosion>.From(Target.gameObject);
        iBulletExplosion?.DestroyBullet();
    }

    private void DestroyTarget(string targetOwnerName)
    {
        _photonNetworkAALauncher.DestroyTarget(targetOwnerName);
    }
}
