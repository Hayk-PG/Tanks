using System;
using UnityEngine;

public class BulletController : MonoBehaviour, IBulletCollision, IBulletLimit, IBulletVelocity<BulletController.VelocityData>, ITurnController
{
    public Rigidbody RigidBody { get; protected set; }
    public IScore OwnerScore { get; set; }

    public Action<Collision, IScore> OnCollision { get; set; }
    public Action<IScore> OnExplodeOnCollision { get; set; }
    public Action<bool> OnExplodeOnLimit { get; set; }
    public Action<VelocityData> OnBulletVelocity { get; set; }

    public TurnController TurnController { get; set; }
    public CameraMovement CameraMovement { get; set; }

    protected WindSystemController _windSystemController;
    protected bool _isWindActivated;
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
    }

    protected virtual void Start()
    {
        TurnController.SetNextTurn(TurnState.Other);

        Invoke("ActivateWindForce", 0.5f);
    }
   
    protected virtual void OnEnable()
    {
        TurnController.OnTurnChanged += OnTurnChanged;
    }

    protected virtual void OnDisable()
    {
        TurnController.OnTurnChanged -= OnTurnChanged;
    }

    protected virtual void FixedUpdate()
    {
        ExplodeOnLimit();
        BulletVelocity();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        OnCollision?.Invoke(collision, OwnerScore);
        OnExplodeOnCollision?.Invoke(OwnerScore);
    }

    protected virtual void ExplodeOnLimit()
    {
        OnExplodeOnLimit?.Invoke(RigidBody.position.y < -5);
    }

    protected virtual void BulletVelocity()
    {
        OnBulletVelocity?.Invoke(new VelocityData(RigidBody, Quaternion.LookRotation(RigidBody.velocity),
                                 new Vector3(_windSystemController.WindForce * Time.fixedDeltaTime, 0, 0),
                                 _isWindActivated));
    } 

    private void ActivateWindForce() => _isWindActivated = true;

    private void OnTurnChanged(TurnState arg1, CameraMovement arg2)
    {
        if(arg1 == TurnState.Other)
        {
            arg2.SetCameraTarget(transform, 10, 4);
        }
    }
}

    

