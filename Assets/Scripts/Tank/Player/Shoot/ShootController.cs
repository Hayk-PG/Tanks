using System;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    private TankController _tankController;
    private PhotonPlayerShootRPC _photonPlayerShootRPC;
    private Rigidbody _rigidBody;
    private PlayerTurn _playerTurn;
    private GameManagerBulletSerializer _gameManagerBulletSerializer;
    private IScore _iScore;
    private PlayerAmmoType _playerAmmoType;
        
    [Serializable] private struct Canon
    {
        internal float _currentEulerAngleX;
        [SerializeField] internal float _minEulerAngleX, _maxEulerAngleX;
        [SerializeField] internal float _rotationSpeed;

        [SerializeField] internal Transform _canonPivotPoint;
        [SerializeField] internal Vector3 _rotationStabilizer;
    }
    [Serializable] private struct Shoot
    {
        [Header("Force")]
        [SerializeField] internal float _currentForce;
        [SerializeField] internal float _minForce, _maxForce;
        [SerializeField] internal float _smoothTime, _maxSpeed;
        internal float _currentVelocity;
        internal bool _isApplyingForce;

        [Header("Bullet")]
        [SerializeField] internal Transform _shootPoint;
    }
    internal class PlayerHUDValues
    {
        internal float _currentAngle, _minAngle, _maxAngle;
        internal float _currentForce, _minForce, _maxForce;

        internal PlayerHUDValues(float _currentAngle, float _minAngle, float _maxAngle, float _currentForce, float _minForce, float _maxForce)
        {
            this._currentAngle = _currentAngle;
            this._minAngle = _minAngle;
            this._maxAngle = _maxAngle;
            this._currentForce = _currentForce;
            this._minForce = _minForce;
            this._maxForce = _maxForce;
        }
    }
    [SerializeField] private Canon _canon;
    [SerializeField] private Shoot _shoot;
    [SerializeField] private PlayerShootTrajectory _playerShootTrajectory;
    [SerializeField] private BulletController _instantiatedBullet;

    public BulletController Bullet
    {
        get => _instantiatedBullet;
        set => _instantiatedBullet = value;
    }
    public float Direction { get; set; }
    public float CurrentForce
    {
        get => _shoot._currentForce;
        set => _shoot._currentForce = value;
    }
    public int ActiveAmmoIndex { get; set; }
    public Transform CanonPivotPoint
    {
        get => _canon._canonPivotPoint;
    }
    public bool IsApplyingForce
    {
        get => _shoot._isApplyingForce;
        set => _shoot._isApplyingForce = value;
    }
   
    public Action<bool> OnCanonRotation;
    internal Action<bool> OnApplyingForce;
    internal event Action<PlayerHUDValues> OnUpdatePlayerHUDValues;


    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);
        _rigidBody = GetComponent<Rigidbody>();
        _playerTurn = GetComponent<PlayerTurn>();
        _gameManagerBulletSerializer = FindObjectOfType<GameManagerBulletSerializer>();
        _iScore = Get<IScore>.From(gameObject);
        _playerAmmoType = Get<PlayerAmmoType>.From(gameObject);
    }

    private void OnEnable()
    {
        _tankController.OnVerticalJoystick += OnVerticalJoystick;
        _tankController.OnShootButtonPointer += OnShootButtonPointer;
        _tankController.OnShootButtonClick += OnShootButtonClick;
    }

    private void OnDisable()
    {
        _tankController.OnVerticalJoystick -= OnVerticalJoystick;
        _tankController.OnShootButtonPointer -= OnShootButtonPointer;
        _tankController.OnShootButtonClick -= OnShootButtonClick;
    }

    private void Update()
    {
        if (_playerTurn.IsMyTurn)
        {
            RotateCanon();
            ApplyForce();
            OnUpdatePlayerHUDValues?.Invoke(new PlayerHUDValues(Converter.AngleConverter(_canon._canonPivotPoint.localEulerAngles.x), _canon._minEulerAngleX, _canon._maxEulerAngleX, _shoot._currentForce, _shoot._minForce, _shoot._maxForce));
        }
    }

    private void OnVerticalJoystick(float value)
    {
        Direction = -value;
    }

    public void RotateCanon()
    {
        _canon._currentEulerAngleX = _canon._canonPivotPoint.localEulerAngles.x;

        if (Direction > 0 && Converter.AngleConverter(_canon._currentEulerAngleX) > _canon._maxEulerAngleX) return;
        if (Direction < 0 && Converter.AngleConverter(_canon._currentEulerAngleX) < _canon._minEulerAngleX) return;

        _canon._canonPivotPoint.localEulerAngles = new Vector3(_canon._currentEulerAngleX += (_canon._rotationSpeed * Direction * Time.deltaTime), 0, 0) + _canon._rotationStabilizer;

        OnCanonRotation?.Invoke(Direction != 0);
    }
    
    private void OnShootButtonPointer(bool isTrue)
    {
        IsApplyingForce = isTrue;
    }

    private void OnShootButtonClick(bool isTrue)
    {
        if (_playerTurn.IsMyTurn)
        {
            Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => ShootBullet(CurrentForce), () => ShootBulletRPC(CurrentForce));
            AmmoUpdate();
        }
    }

    private void ApplyForce()
    {
        Conditions<bool>.Compare(IsApplyingForce, OnForceApplied, OnForceReleased);
        _playerShootTrajectory.TrajectoryPrediction(CurrentForce);
    }

    private void OnForceApplied()
    {
        CurrentForce = Mathf.SmoothDamp(CurrentForce, _shoot._maxForce, ref _shoot._currentVelocity, _shoot._smoothTime * Time.deltaTime, _shoot._maxSpeed);
        OnApplyingForce?.Invoke(true);
    }

    private void OnForceReleased()
    {
        if (CurrentForce != _shoot._minForce) CurrentForce = _shoot._minForce;
        OnApplyingForce?.Invoke(false);
    }

    public void ShootBullet(float force)
    {
        if(_playerAmmoType._weaponsBulletsCount[ActiveAmmoIndex] > 0)
        {
            Bullet = Instantiate(_playerAmmoType._weapons[ActiveAmmoIndex]._bulletPrefab, _shoot._shootPoint.position, _canon._canonPivotPoint.rotation);
            Bullet.OwnerScore = _iScore;
            Bullet.RigidBody.velocity = Bullet.transform.forward * force;
            _gameManagerBulletSerializer.BulletController = Bullet;
            _rigidBody.AddForce(transform.forward * force * 1000, ForceMode.Impulse);
        }
    }

    private void AmmoUpdate()
    {
        if (_playerAmmoType._weaponsBulletsCount[ActiveAmmoIndex] > 0)
        {
            _playerAmmoType._weaponsBulletsCount[ActiveAmmoIndex] -= 1;
            _playerAmmoType.UpdateDisplayedWeapon(ActiveAmmoIndex);
            _playerAmmoType.SwitchToDefaultWeapon(ActiveAmmoIndex);
        }
    }

    private void ShootBulletRPC(float force)
    {
        if (_photonPlayerShootRPC == null)
            _photonPlayerShootRPC = _tankController.BasePlayer.GetComponent<PhotonPlayerShootRPC>();

        _photonPlayerShootRPC?.CallShootRPC(force);
    }  
}
