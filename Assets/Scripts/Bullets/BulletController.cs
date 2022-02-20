using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Rigidbody RigidBody { get; private set; }
    public IScore OwnerScore { get; set; }

    internal TurnController _turnController;
    private WindSystemController _windSystemController;

    [Serializable]
    private struct Particles
    {
        [SerializeField] internal GameObject _trail;      
        [SerializeField] internal GameObject _muzzleFlash;
    }

    [SerializeField]
    private Particles _particles;

    private bool _isWindActivated;
    internal struct VelocityData
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

    internal Action<VelocityData> OnBulletVelocity;
    internal Action<Collision> OnCollision;
    internal Action<IScore> OnExplodeOnCollision;
    internal Action<bool> OnExplodeOnLimit;


    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();
        _turnController = FindObjectOfType<TurnController>();
        _windSystemController = FindObjectOfType<WindSystemController>();

        _particles._muzzleFlash.transform.parent = null;

        Invoke("ActivateTrail", 0.1f);
        Invoke("ActivateWindForce", 0.5f);
    }

    private void Start()
    {
        _turnController.SetNextTurn(TurnState.Other);
    }
   
    private void OnEnable()
    {
        _turnController.OnTurnChanged += OnTurnChanged;
    }
    
    private void OnDisable()
    {
        _turnController.OnTurnChanged -= OnTurnChanged;
    }

    private void FixedUpdate()
    {
        OnExplodeOnLimit?.Invoke(RigidBody.position.y < -5);

        OnBulletVelocity?.Invoke(new VelocityData(RigidBody, Quaternion.LookRotation(RigidBody.velocity), 
                                 new Vector3(_windSystemController.WindForce * Time.fixedDeltaTime, 0, 0), 
                                 _isWindActivated));       
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollision?.Invoke(collision);
        OnExplodeOnCollision?.Invoke(OwnerScore);
    }

    private void ActivateTrail() => _particles._trail.SetActive(true);

    private void ActivateWindForce() => _isWindActivated = true;

    private void OnTurnChanged(TurnState arg1, CameraMovement arg2)
    {
        if(arg1 == TurnState.Other)
        {
            arg2.SetCameraTarget(transform, 3);
        }
    }
}

    

