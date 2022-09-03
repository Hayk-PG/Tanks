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
    private TankMovement _tankMovement;
    private PhotonPlayerShootRPC _photonPlayerShootRPC;
    private Rigidbody _rigidBody;   
    private GameManagerBulletSerializer _gameManagerBulletSerializer;
    private IScore _iScore;
    private IShoot _iShoot;
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
    private bool _isSandbagsTriggered;

    public Action<bool> OnCanonRotation { get; set; }
    internal event Action<PlayerHUDValues> OnUpdatePlayerHUDValues;
   

    protected override void Awake()
    {
        base.Awake();
        ShootPointGameobjectActivity(false);
        _tankController = Get<TankController>.From(gameObject);
        _tankMovement = Get<TankMovement>.From(gameObject);
        _rigidBody = GetComponent<Rigidbody>();       
        _gameManagerBulletSerializer = FindObjectOfType<GameManagerBulletSerializer>();
        _iScore = Get<IScore>.From(gameObject);       
        _playerAmmoType = Get<PlayerAmmoType>.From(gameObject);        
    }

    private void OnEnable()
    {
        _tankController.OnInitialize += OnInitialize;
        _tankController.OnControllers += OnControllers;
        _tankController.OnShootButtonClick += OnShootButtonClick;
        _tankMovement.OnDirectionValue += OnMovementDirectionValue;
    }

    private void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        _tankController.OnControllers -= OnControllers;
        _tankController.OnShootButtonClick -= OnShootButtonClick;
        _tankMovement.OnDirectionValue -= OnMovementDirectionValue;
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

    private void OnInitialize()
    {
        _iShoot = Get<IShoot>.From(_tankController.BasePlayer.gameObject);
        ShootPointGameobjectActivity(true);
    }

    private void ShootPointGameobjectActivity(bool isActive)
    {
        if (_shootPoint.gameObject.activeInHierarchy != isActive)
            _shootPoint.gameObject.SetActive(isActive);
    }

    private void OnMovementDirectionValue(float direction)
    {
        if (_tankController.BasePlayer != null)
        {
            ShootPointGameobjectActivity(direction == 0);
        }
    }

    private void OnControllers(Vector2 values)
    {
        Direction = -values.y;
        CurrentForce = Mathf.Clamp(CurrentForce + (_playerTurn.MyTurn == TurnState.Player1 ? values.x: -values.x) * 2 * Time.deltaTime, _shoot._minForce, _shoot._maxForce);       
    }

    public void RotateCanon()
    {
        _canon._currentEulerAngleX = _canonPivotPoint.localEulerAngles.x;

        if (Direction > 0 && Converter.AngleConverter(_canon._currentEulerAngleX) > _canon._maxEulerAngleX) return;
        if (Direction < 0 && Converter.AngleConverter(_canon._currentEulerAngleX) < _canon._minEulerAngleX) return;

        _canonPivotPoint.localEulerAngles = new Vector3(_canon._currentEulerAngleX += (_canon._rotationSpeed * Direction * Time.deltaTime), 0, 0) + _canon._rotationStabilizer;
    }

    private void ApplyForce()
    {
        if(_shootPoint != null && _shootPoint.gameObject.activeInHierarchy)
            _trajectory.PredictedTrajectory(CurrentForce);
    }

    private void OnShootButtonClick(bool isTrue)
    {
        if (_playerTurn.IsMyTurn)
        {
            _iShoot?.Shoot(CurrentForce);           
            AmmoUpdate();
        }
    }

    private void InstantiateBullet(float force)
    {
        Bullet = Instantiate(_playerAmmoType._weapons[ActiveAmmoIndex]._prefab, _shootPoint.position, _canonPivotPoint.rotation);
        Bullet.OwnerScore = _iScore;
        Bullet.RigidBody.velocity = Bullet.transform.forward * force;
        _gameManagerBulletSerializer.BulletController = Bullet;
        mainCameraController.CameraOffset(_playerTurn, Bullet.transform, null, null);
    }

    public void ShootBullet(float force)
    {
        if(_playerAmmoType._weaponsBulletsCount[ActiveAmmoIndex] > 0)
        {
            InstantiateBullet(force);
            AddForce(force);
            OnShoot?.Invoke();
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

    private void AddForce(float force)
    {
        if(!_isSandbagsTriggered)
            _rigidBody.AddForce(transform.forward * force * 1000, ForceMode.Impulse);
    }

    public void OnEnteredSandbagsTrigger(bool isEntered)
    {
        _isSandbagsTriggered = isEntered;
    }
}
