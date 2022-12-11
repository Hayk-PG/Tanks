using System;
using UnityEngine;

public class BulletController : MonoBehaviour, IBulletCollision, IBulletLimit, IBulletVelocity<BulletController.VelocityData>, ITurnController
{
    public Rigidbody RigidBody { get; protected set; }
    public IScore OwnerScore { get; set; }
    public Vector3 StartPosition { get; set; }
    public float Distance
    {
        get => Vector3.Distance(StartPosition, transform.position);
    }
    public bool IsLastShellOfBarrage { get; set; }

    public Action<Collider, IScore, float> OnCollision { get; set; }
    public Action<IScore, float> OnExplodeOnCollision { get; set; }
    public Action<bool> OnExplodeOnLimit { get; set; }
    public Action<VelocityData> OnBulletVelocity { get; set; }
    public Action OnExitCollision { get; set; }

    public TurnController TurnController { get; set; }

    protected WindSystemController _windSystemController;
    protected bool _isWindActivated;
    protected int _collisionsCount;
    public struct VelocityData
    {
        internal Rigidbody _rigidBody;

        internal Quaternion _lookRotation;
        internal Vector3 _windVelocity;

        internal bool _isWindActivated;

        internal VelocityData(Rigidbody rb, Quaternion lookR, Vector3 _windVel, bool isWindActivated)
        {
            _rigidBody = rb;
            _lookRotation = lookR;
            _windVelocity = _windVel;
            _isWindActivated = isWindActivated;
        }
    }



    protected virtual void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();
        TurnController = FindObjectOfType<TurnController>();
        _windSystemController = FindObjectOfType<WindSystemController>();

        StartPosition = transform.position;
    }

    protected virtual void Start()
    {
        TurnController.SetNextTurn(TurnState.Other);

        Invoke("ActivateWindForce", 0.5f);
    }
   
    protected virtual void OnEnable()
    {
        
    }

    protected virtual void OnDisable()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        ExplodeOnLimit();
        BulletVelocity();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        _collisionsCount++;

        if (_collisionsCount <= 1)
        {
            OnCollision?.Invoke(collision.collider, OwnerScore, Distance);
            OnExplodeOnCollision?.Invoke(OwnerScore, Distance);
        }
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        _collisionsCount = 0;
        OnExitCollision?.Invoke();
    }
 
    protected virtual void ExplodeOnLimit() => OnExplodeOnLimit?.Invoke(RigidBody.position.y <= VerticalLimit.Min);

    protected virtual void BulletVelocity()
    {
        OnBulletVelocity?.Invoke(new VelocityData(RigidBody, Quaternion.LookRotation(RigidBody.velocity),
                                 new Vector3(_windSystemController.WindForce * Time.fixedDeltaTime, 0, 0),
                                 _isWindActivated));
    } 

    protected virtual void ActivateWindForce() => _isWindActivated = true;
}

    

