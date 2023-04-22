using System;
using UnityEngine;

[Serializable]
public class BaseTankMovement : MonoBehaviour
{
    [Header("Movement parameters")] 
    [Range(0, 1000)] public float _normalSpeed;
    [Range(0, 1000)] public float _maxBrake;
    [Range(0, 3000)] public float _accelerated;
    [Range(0, 100)] [Tooltip("Light => 0 - 40: Medium => 45 - 75: Heavy => 80 - 100")] public int _damageFactor;

    [Header("Movement factors")]
    public float _speedOnNormal;
    public float _speedOnRain;
    public float _speedOnSnow;

    [Space]
    public float _brakeOnNormal;
    public float _brakeOnRain;
    public float _brakeOnSnow;

    [Space] [Range(50, 500)]
    public float fuelConsumptionPercent;

    [Space]
    public Vector3 _normalCenterOfMass;
    protected float _currentBrake;
    protected float _currentSpeed;

    protected Rigidbody _rigidBody;
    protected PlayerTurn _playerTurn;
    protected HealthController _healthController;
    protected WheelColliderController _wheelColliderController;   
    protected Raycasts _rayCasts;
    protected VehicleRigidbodyPosition _vehicleRigidbodyPosition;
    protected Stun _stun;

    protected bool _isOnRightSlope, _isOnLeftSlope, _isStunned;
    protected string[] _slopesNames;
    protected Vector3 _vectorRight;
    protected Vector3 _vectorLeft;

    public virtual int DamageFactor
    {
        get
        {
            return (_healthController.Health + _damageFactor) >= 100 ? 100 : _healthController.Health + _damageFactor;
        }
    }
    public virtual float Speed
    {
        get => _currentSpeed;
        set => _currentSpeed = value / 100 * DamageFactor;
    }
    public virtual float Direction { get; set; }
    protected float RotationXAxis { get; set; }
    protected float InitialRotationYAxis { get; set; }   
    protected float _speedBlocker;

    internal Action<float> OnVehicleMove { get; set; }
    public Action<float> OnDirectionValue { get; set; }
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
        _stun = Get<Stun>.FromChild(gameObject);

        Initialize();
    }

    protected virtual void OnEnable()
    {
        _vehicleRigidbodyPosition.OnAllowingPlayerToMoveOnlyFromLeftToRight += OnAllowingPlayerToMoveOnlyFromLeftToRight;

        GameSceneObjectsReferences.WeatherManager.onWeatherActivity += OnWeatherActivity;

        if (_stun != null)
            _stun.OnStunEffect += OnStunEffect;
    }

    protected virtual void OnDisable()
    {
        _vehicleRigidbodyPosition.OnAllowingPlayerToMoveOnlyFromLeftToRight -= OnAllowingPlayerToMoveOnlyFromLeftToRight;

        GameSceneObjectsReferences.WeatherManager.onWeatherActivity -= OnWeatherActivity;

        if (_stun != null)
            _stun.OnStunEffect -= OnStunEffect;
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

    protected virtual void OnWeatherActivity(bool isRaining, bool isSnowing)
    {
        if (isRaining)
        {
            _normalSpeed = _speedOnRain;
            _maxBrake = _brakeOnRain;
        }

        else if (isSnowing)
        {
            _normalSpeed = _speedOnSnow;
            _maxBrake = _brakeOnSnow;
        }

        else
        {
            _normalSpeed = _speedOnNormal;
            _maxBrake = _brakeOnNormal;
        }
    }

    protected virtual void OnStunEffect(bool isStunned)
    {
        _isStunned = isStunned;
    }
}
