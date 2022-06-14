using System;
using UnityEngine;

public class ShootController : BaseShootController
{
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
    private TankController _tankController;
    private PhotonPlayerShootRPC _photonPlayerShootRPC;
    private Rigidbody _rigidBody;
    private PlayerTurn _playerTurn;
    private GameManagerBulletSerializer _gameManagerBulletSerializer;
    private IScore _iScore;
    private PlayerAmmoType _playerAmmoType;
 
    [HideInInspector] [SerializeField] private BulletController _instantiatedBullet;
    [HideInInspector] [SerializeField] private int _activeAmmoIndex;

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
    public int ActiveAmmoIndex
    {
        get => _activeAmmoIndex;
        set => _activeAmmoIndex = value;
    }
    public Vector3 CanonPivotPointEulerAngles
    {
        get => _canonPivotPoint.eulerAngles;
        set => _canonPivotPoint.eulerAngles = value;
    }
    public bool IsApplyingForce
    {
        get => _shoot._isApplyingForce;
        set => _shoot._isApplyingForce = value;
    }
 
    public Action<bool> OnCanonRotation;
    internal Action<bool> OnApplyingForce;
    internal event Action<PlayerHUDValues> OnUpdatePlayerHUDValues;
   

    protected override void Awake()
    {
        base.Awake();
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
            OnUpdatePlayerHUDValues?.Invoke(new PlayerHUDValues(Converter.AngleConverter(_canonPivotPoint.localEulerAngles.x), _canon._minEulerAngleX, _canon._maxEulerAngleX, _shoot._currentForce, _shoot._minForce, _shoot._maxForce));
        }
    }

    private void OnVerticalJoystick(float value)
    {
        Direction = -value;
    }

    public void RotateCanon()
    {
        _canon._currentEulerAngleX = _canonPivotPoint.localEulerAngles.x;

        if (Direction > 0 && Converter.AngleConverter(_canon._currentEulerAngleX) > _canon._maxEulerAngleX) return;
        if (Direction < 0 && Converter.AngleConverter(_canon._currentEulerAngleX) < _canon._minEulerAngleX) return;

        _canonPivotPoint.localEulerAngles = new Vector3(_canon._currentEulerAngleX += (_canon._rotationSpeed * Direction * Time.deltaTime), 0, 0) + _canon._rotationStabilizer;

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
        _trajectory.PredictedTrajectory(CurrentForce);
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

    private void InstantiateBullet(float force)
    {
        Bullet = Instantiate(_playerAmmoType._weapons[ActiveAmmoIndex]._prefab, _shootPoint.position, _canonPivotPoint.rotation);
        Bullet.OwnerScore = _iScore;
        Bullet.RigidBody.velocity = Bullet.transform.forward * force;
        _gameManagerBulletSerializer.BulletController = Bullet;
    }

    public void ShootBullet(float force)
    {
        if(_playerAmmoType._weaponsBulletsCount[ActiveAmmoIndex] > 0)
        {
            InstantiateBullet(force);
            AddForce(force);
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
    
    private void AddForce(float force)
    {
        _rigidBody.AddForce(transform.forward * force * 1000, ForceMode.Impulse);
    }
}
