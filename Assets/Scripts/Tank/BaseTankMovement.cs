using System;
using UnityEngine;

[Serializable]
public class BaseTankMovement : MonoBehaviour
{
    [Header("Movement parameters")]
    [Range(0, 1000)] public float _normalSpeed;
    public float _accelerated;    
    public float _maxBrake;
    public Vector3 _normalCenterOfMass;
    protected float _currentBrake;
    protected float _currentSpeed;

    protected Rigidbody _rigidBody;
    protected PlayerTurn _playerTurn;
    protected HealthController _healthController;
    protected WheelColliderController _wheelColliderController;   
    protected Raycasts _rayCasts;
    protected VehicleRigidbodyPosition _vehicleRigidbodyPosition;
        
    protected bool _isOnRightSlope, _isOnLeftSlope;
    protected string[] _slopesNames;
    protected Vector3 _vectorRight;
    protected Vector3 _vectorLeft;

    public virtual float Speed
    {
        get => _currentSpeed;
        set => _currentSpeed = value / 100 * _healthController.Health;
    }
    public virtual float Direction { get; set; }
    protected float RotationXAxis { get; set; }
    protected float InitialRotationYAxis { get; set; }   
    protected float _speedBlocker;

    internal Action<float> OnVehicleMove { get; set; }
    public Action<Rigidbody> OnRigidbodyPosition { get; set; }
    public Action<float, bool> OnFuel { get; set; }


    protected virtual void Awake()
    {
        _rigidBody = Get<Rigidbody>.From(gameObject);
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _healthController = Get<HealthController>.From(gameObject);
        _wheelColliderController = Get<WheelColliderController>.FromChild(gameObject);
        _rayCasts = Get<Raycasts>.FromChild(gameObject);
        _vehicleRigidbodyPosition = Get<VehicleRigidbodyPosition>.From(gameObject);

        Initialize();
    }

    protected virtual void OnEnable()
    {
        _vehicleRigidbodyPosition.OnAllowingPlayerToMoveOnlyFromLeftToRight += OnAllowingPlayerToMoveOnlyFromLeftToRight;
    }

    protected virtual void OnDisable()
    {
        _vehicleRigidbodyPosition.OnAllowingPlayerToMoveOnlyFromLeftToRight -= OnAllowingPlayerToMoveOnlyFromLeftToRight;
    }

    void Initialize()
    {
        InitialRotationYAxis = _rigidBody.position.x < 0 ? 90 : -90;

        string right = InitialRotationYAxis > 0 ? "RS" : "LS";
        string left = InitialRotationYAxis > 0 ? "LS" : "RS";
        _slopesNames = new string[2] { right, left };

        _vectorRight = InitialRotationYAxis > 0 ? Vector3.right : Vector3.left;
        _vectorLeft = InitialRotationYAxis > 0 ? Vector3.left : Vector3.right;
    }

    protected virtual void OnAllowingPlayerToMoveOnlyFromLeftToRight(bool? mustMoveFromLeftToRightOnly)
    {

    }
}
