using UnityEngine;
using System;

public class BaseBulletVelocity : MonoBehaviour
{
    [SerializeField]
    protected BaseBulletController _baseBulletController;
    [Space]

    [SerializeField] protected float _gravityForcePercentage;
    [SerializeField] protected float _windForcePercentage;
    [Space]
    [SerializeField] protected float _trailTime;

    protected bool _isWindForceApplied;
    
    protected virtual Vector3 WindForce
    {
        get => new Vector3(GameSceneObjectsReferences.WindSystemController.CurrentWindForce * Time.fixedDeltaTime, 0, 0);
    }
    protected virtual float GravityForce
    {
        get => _baseBulletController.RigidBody.velocity.y + (_baseBulletController.RigidBody.velocity.y / 100 * GravityForcePercentage) * Time.fixedDeltaTime;
    }
    public virtual float GravityForcePercentage
    { 
        get => _gravityForcePercentage; 
        set => _gravityForcePercentage = value;
    }
    public virtual float WindForcePercentage 
    { 
        get => _windForcePercentage; 
        set => _windForcePercentage = value; 
    }  

    public event Action onTrail;
    public event Action onVerticalLimit;




    protected virtual void Start()
    {
        Invoke("ActivateTrail", _trailTime);
        Invoke("ApplyWindForce", 0.5f);
    }

    protected virtual void FixedUpdate()
    {
        ControlLookRotation();
        ControlMovement();
        ControlGravitation();
        OnVerticalLimit();
    }

    protected virtual void ActivateTrail()
    {
        onTrail?.Invoke();
    }

    protected virtual void ApplyWindForce()
    {
        _isWindForceApplied = true;
    }

    protected virtual void ControlLookRotation()
    {
        _baseBulletController.RigidBody.rotation = Quaternion.LookRotation(_baseBulletController.RigidBody.velocity);
    }

    protected virtual void ControlMovement()
    {
        if (_isWindForceApplied)
            _baseBulletController.RigidBody.velocity += new Vector3((WindForce.x / 100 * WindForcePercentage), WindForce.y, WindForce.z);
    }

    protected virtual void ControlGravitation()
    {
        if (_baseBulletController.RigidBody.velocity.y <= 0)
            _baseBulletController.RigidBody.velocity = new Vector3(_baseBulletController.RigidBody.velocity.x, GravityForce, _baseBulletController.RigidBody.velocity.z);
    }

    protected virtual void OnVerticalLimit()
    {
        if(_baseBulletController.RigidBody.position.y <= VerticalLimit.Min)
        {
            onVerticalLimit?.Invoke();
        }
    }
}