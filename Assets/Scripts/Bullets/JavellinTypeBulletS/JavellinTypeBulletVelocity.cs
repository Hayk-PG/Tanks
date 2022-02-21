using UnityEngine;
using System.Linq;
using System;

public class JavellinTypeBulletVelocity : BulletVelocity, ILockedMissile, IBulletTrail
{
    private Transform _targetPlayer;

    private delegate bool Checker();
    private delegate bool CheckerVelocity(BulletController.VelocityData velocityData);
    private delegate void Target();
    private delegate void Velocity(BulletController.VelocityData velocityData);
    private delegate Vector3 Vector();

    private Checker _isTargetSet;
    private CheckerVelocity _isGettingCloseToTheTarget;
    private Checker _isMissileAboveTheTarget;
    private Target _lockTheTarget;
    private Target _setTheTarget;
    private Velocity _startMovingUp;
    private Velocity _addForce;
    private Velocity _moveTowardsTheTarget;
    private Vector _targetPosition;

    private bool _changeVelocityMode;
    private bool _isTargetLocked;
    private bool _hasAddedForce;

    public Action<float> OnTargetLocked { get; set; }

    protected override void Start()
    {
        Invoke("ChangeVelocityMode", 0.5f);

        OnSubscription();
    }

    private void OnSubscription()
    {
        _isTargetSet = delegate
        {
            return _targetPlayer != null;
        };

        _isGettingCloseToTheTarget = delegate (BulletController.VelocityData velocityData)
        {
            return velocityData._rigidBody.velocity.y < 2;
        };

        _targetPosition = delegate
        {
            return _targetPlayer.position - transform.position;
        };

        _isMissileAboveTheTarget = delegate
        {
            return transform.position.x > _targetPlayer.position.x - 5.1f;
        };

        _lockTheTarget = delegate
        {
            _isTargetLocked = true;
        };

        _startMovingUp = delegate (BulletController.VelocityData velocityData)
        {
            velocityData._rigidBody.velocity += Vector3.up * 100 * Time.fixedDeltaTime;
        };

        _setTheTarget = delegate
        {
            if (!_isTargetSet())
            {
                _targetPlayer = FindObjectsOfType<PlayerTurn>().ToList().Find(turn => turn.MyTurn == TurnState.Player2).transform;
            };
        };

        _addForce = delegate (BulletController.VelocityData velocityData)
        {
            if (_targetPlayer != null && _isTargetLocked && !_hasAddedForce)
            {
                velocityData._rigidBody.AddForce(((_targetPlayer.position - new Vector3(0.75f, 0, 0)) - transform.position) * 300 * Time.fixedDeltaTime, ForceMode.Impulse);
                _hasAddedForce = true;
            };
        };

        _moveTowardsTheTarget = delegate (BulletController.VelocityData velocityData)
        {
            velocityData._rigidBody.velocity += _targetPosition() * 5 * Time.fixedDeltaTime;
        };
    }
   
    protected override void OnBulletVelocity(BulletController.VelocityData velocityData)
    {
        BulletLookRotation(velocityData);

        if (_changeVelocityMode)
        {
            Conditions<bool>.Compare(!_isTargetLocked && _isGettingCloseToTheTarget(velocityData), delegate { _startMovingUp(velocityData); _setTheTarget(); ActivateTrail(); }, null);
            Conditions<bool>.Compare(_isTargetSet() && !_isTargetLocked && _isMissileAboveTheTarget(), delegate { _lockTheTarget(); _moveTowardsTheTarget(velocityData); } , null);
            Conditions<bool>.Compare(_isTargetSet() && _isTargetLocked, () => OnTargetLocked?.Invoke(_targetPosition().magnitude), null);
        }                     
    }

    private void ChangeVelocityMode()
    {
        _changeVelocityMode = true;
    }
}
