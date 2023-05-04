using System;
using UnityEngine;

public class ShootController : BaseShootController, IEndGame
{
    public class PlayerHUDValues
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

    protected TankController _tankController;

    protected TankMovement _tankMovement;

    protected PhotonPlayerShootRPC _photonPlayerShootRPC;

    protected Rigidbody _rigidBody;

    protected IScore _iScore;

    protected IShoot _iShoot;

    protected PlayerAmmoType _playerAmmoType;

    protected GameplayAnnouncer _gameplayAnnouncer;

    protected ButtonType _currentWeaponType;
    
    [HideInInspector] [SerializeField] 
    protected BaseBulletController _instantiatedBullet;

    [HideInInspector] [SerializeField] 
    protected int _activeAmmoIndex;

    protected bool _hasShot, _isGameEnd;

    public BaseBulletController Bullet
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

    public int ActiveRocketId { get; protected set; }

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

    protected bool _isSandbagsTriggered;
    protected bool _isGameplayAnnounced;

    public Action<bool> OnCanonRotation { get; set; }
    public Action<PlayerHUDValues> OnUpdatePlayerHUDValues { get; set; }


   

    protected override void Awake()
    {
        base.Awake();

        ShootPointGameobjectActivity(false);

        _tankController = Get<TankController>.From(gameObject);

        _tankMovement = Get<TankMovement>.From(gameObject);

        _rigidBody = GetComponent<Rigidbody>();    

        _iScore = Get<IScore>.From(gameObject);     
        
        _playerAmmoType = Get<PlayerAmmoType>.From(gameObject);

        _gameplayAnnouncer = FindObjectOfType<GameplayAnnouncer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _tankController.OnInitialize += OnInitialize;
        _tankController.OnControllers += OnControllers;
        _tankController.OnShootButtonClick += OnShootButtonClick;

        _tankMovement.OnDirectionValue += OnMovementDirectionValue;

        _playerAmmoType.onRocketSelected += OnRocketSelected;

        GameSceneObjectsReferences.GameplayAnnouncer.onGameStartAnnouncement += delegate { _isGameplayAnnounced = true; };

        GameSceneObjectsReferences.TurnController.OnTurnChanged += OnTurnChanged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _tankController.OnInitialize -= OnInitialize;
        _tankController.OnControllers -= OnControllers;
        _tankController.OnShootButtonClick -= OnShootButtonClick;

        _tankMovement.OnDirectionValue -= OnMovementDirectionValue;

        GameSceneObjectsReferences.GameplayAnnouncer.onGameStartAnnouncement -= delegate { _isGameplayAnnounced = true; };

        GameSceneObjectsReferences.TurnController.OnTurnChanged += OnTurnChanged;
    }

    protected virtual void FixedUpdate()
    {
        if (_playerTurn.IsMyTurn && !_isStunned)
        {
            RotateCanon();

            ApplyForce();

            OnUpdatePlayerHUDValues?.Invoke(new PlayerHUDValues(Converter.AngleConverter(_canonPivotPoint.localEulerAngles.x), _canon._minEulerAngleX, _canon._maxEulerAngleX, _shoot._currentForce, _shoot._minForce, _shoot._maxForce));

            _trajectory?.PointsOverlapSphere(_tankController.BasePlayer != null);
        }
        else
        {
            ShootPointGameobjectActivity(false);
        }
    }

    protected virtual void OnInitialize() => _iShoot = Get<IShoot>.From(_tankController.BasePlayer.gameObject);

    public void GetActiveWeaponType(ButtonType weaponType)
    {
        _currentWeaponType = weaponType;

        print($"Active weapon type: {_currentWeaponType}");
    }

    protected virtual void OnRocketSelected(bool isSelected, int id)
    {
        _currentWeaponType = isSelected ? ButtonType.Rocket : ButtonType.Shell;

        ActiveRocketId = id;

        ShootPointGameobjectActivity();

        GameSceneObjectsReferences.Controllers.SetControllersActive(!isSelected);
    }

    protected virtual void ShootPointGameobjectActivity(bool isActive = false)
    {
        if(_currentWeaponType == ButtonType.Rocket || _isGameEnd)
        {
            _shootPoint.gameObject.SetActive(false);

            return;
        }

        if (_shootPoint.gameObject.activeInHierarchy != isActive && _isGameplayAnnounced)
            _shootPoint.gameObject.SetActive(isActive);
    }

    protected virtual void OnMovementDirectionValue(float direction)
    {
        if (_tankController.BasePlayer != null)
        {
            ShootPointGameobjectActivity(direction == 0);
        }
    }

    protected virtual void OnControllers(Vector2 values)
    {
        Direction = -values.y;
        CurrentForce = Mathf.Clamp(CurrentForce + (_playerTurn.MyTurn == TurnState.Player1 ? values.x: -values.x) * 2 * Time.deltaTime, _shoot._minForce, _shoot._maxForce);       
    }

    public virtual void RotateCanon()
    {
        _canon._currentEulerAngleX = _canonPivotPoint.localEulerAngles.x;

        if (Direction > 0 && Converter.AngleConverter(_canon._currentEulerAngleX) > _canon._maxEulerAngleX || Direction < 0 && Converter.AngleConverter(_canon._currentEulerAngleX) < _canon._minEulerAngleX) 
            return;

        _canonPivotPoint.localEulerAngles = new Vector3(_canon._currentEulerAngleX += (_canon._rotationSpeed * Direction * Time.deltaTime), 0, 0) + _canon._rotationStabilizer;
    }

    protected virtual void ApplyForce()
    {
        if (_shootPoint != null && _shootPoint.gameObject.activeInHierarchy)
            _trajectory?.PredictedTrajectory(_currentWeaponType == ButtonType.Railgun ? Vector3.zero : Physics.gravity, CurrentForce);
    }

    protected virtual void OnTurnChanged(TurnState turnState) => _hasShot = false;

    protected virtual void OnShootButtonClick(bool isTrue)
    {
        if (HaveEnoughBullets() && _playerTurn.IsMyTurn && !_isStunned && !_hasShot)
        {
            Conditions<bool>.Compare(_currentWeaponType == ButtonType.Shell || _currentWeaponType == ButtonType.Railgun, () => _iShoot?.Shoot(CurrentForce), () => _iShoot.LaunchRocket(ActiveRocketId));
            
            AmmoUpdate();

            _hasShot = true;
        }
    }

    public virtual void ShootBullet(float force)
    {
        InstantiateBullet(force);

        AddForce(_tankMovement.Direction == 0 ? force : _tankMovement.Direction != 0 && force <= 3 ? _tankMovement.Direction * force : _tankMovement.Direction * 3);

        OnShoot?.Invoke();

        OnDash?.Invoke(_tankMovement.Direction);
    }

    protected virtual void InstantiateBullet(float force)
    {
        Bullet = Instantiate(_playerAmmoType._weapons[ActiveAmmoIndex]._prefab, _shootPoint.position, _canonPivotPoint.rotation);
        Bullet.OwnerScore = _iScore;
        Bullet.RigidBody.velocity = Bullet.transform.forward * force;

        GameSceneObjectsReferences.GameManagerBulletSerializer.BaseBulletController = Bullet;

        GameSceneObjectsReferences.MainCameraController.GetProjectileRigidbody(Bullet.RigidBody);

        //GameSceneObjectsReferences.MainCameraController.CameraOffset(_playerTurn, Bullet.RigidBody, 5, null);
    }

    public virtual void LaunchRocket(int id)
    {
        BaseBulletController rocketType = GlobalFunctions.ObjectsOfType<DropBoxSelectionPanelRocket>.Find(r => r.Id == id, true).Weapon._prefab;

        if (rocketType == null)
            return;

        BaseBulletController rocket = Instantiate(rocketType, _rocketSpawnPoint?.position ?? transform.position, _rocketSpawnPoint?.rotation ?? transform.rotation);

        rocket.OwnerScore = _iScore;

        GameSceneObjectsReferences.GameManagerBulletSerializer.BaseBulletController = rocket;

        GameSceneObjectsReferences.MainCameraController.CameraOffset(_playerTurn, rocket.RigidBody, 6, null);
    }

    protected virtual bool HaveEnoughBullets()
    {
        return _playerAmmoType._weaponsBulletsCount[ActiveAmmoIndex] > 0;
    }

    protected virtual void AmmoUpdate()
    {
        if (_playerAmmoType._weaponsBulletsCount[ActiveAmmoIndex] > 0)
        {
            _playerAmmoType._weaponsBulletsCount[ActiveAmmoIndex] -= 1;
            _playerAmmoType.UpdateDisplayedWeapon(ActiveAmmoIndex);
            _playerAmmoType.SwitchToDefaultWeapon(ActiveAmmoIndex);
        }
    }

    protected virtual void AddForce(float force)
    {
        if(!_isSandbagsTriggered)
            _rigidBody.AddForce(transform.forward * force * _shoot._rigidbodyForceMultiplier, ForceMode.Impulse);
    }

    public virtual void OnEnteredSandbagsTrigger(bool isEntered)
    {
        _isSandbagsTriggered = isEntered;
    }

    public virtual void OnGameEnd(object[] data = null)
    {
        ShootPointGameobjectActivity(_isGameEnd = true);
    }

    public virtual void WrapUpGame(object[] data = null)
    {
        
    }
}
