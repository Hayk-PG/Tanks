using System;
using UnityEngine;

public class AAProjectileVelocity : BaseBulletVelocity
{
    private BaseBulletController _target;

    private bool _isTrailActive; 

    public event Action onTargetDetected;



    protected override void FixedUpdate()
    {
        bool isMissileActive = _baseBulletController.RigidBody.velocity.y <= 1 && !_isTrailActive;
        bool isTargetDetected = _isTrailActive && _target != null;
        bool isTargetClose = isTargetDetected && Vector3.Distance(_baseBulletController.RigidBody.position, _target.RigidBody.position) <= 0.5f;
        
        ScanTarget(isMissileActive);
        MoveTowardsTarget(isTargetDetected);
        DestroyTarget(isTargetClose);
        base.ControlLookRotation();
    }

    public virtual void ScanTarget(bool isConditionMet)
    {
        if (isConditionMet)
        {
            ActivateTrail();
            _target = GlobalFunctions.ObjectsOfType<BaseBulletController>.Find(bullet => bullet.transform.position != transform.position);
            _isTrailActive = true;
        }
    }

    public void MoveTowardsTarget(bool isConditionMet)
    {
        if (isConditionMet)
            _baseBulletController.RigidBody.position = Vector3.MoveTowards(_baseBulletController.RigidBody.position, _target.RigidBody.position, 10 * Time.fixedDeltaTime);
    }

    public void DestroyTarget(bool isConditionMet)
    {
        if (isConditionMet)
        {
            Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, DestroyTarget, () => DestroyTarget(_target.OwnerScore.PlayerTurn.gameObject.name));
            onTargetDetected?.Invoke();
        }
    }

    private void DestroyTarget()
    {
        IBulletExplosion iBulletExplosion = Get<IBulletExplosion>.From(_target.gameObject);
        iBulletExplosion?.DestroyBullet();
    }

    private void DestroyTarget(string targetOwnerName)
    {
        GameSceneObjectsReferences.PhotonNetworkAALauncher.DestroyTarget(targetOwnerName);
    }
}
