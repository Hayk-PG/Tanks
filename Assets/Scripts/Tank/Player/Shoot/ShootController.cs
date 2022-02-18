using System;
using UnityEngine;

public class ShootController : MonoBehaviour, ICanonRotation
{
    private FixedJoystick _joystick;
    private ShootButton _shootButton;
    private Rigidbody _rigidBody;
    private PlayerTurn _playerTurn;
    private IScore _iScore;

    [SerializeField]
    private PlayerShootTrajectory _playerShootTrajectory;

    public float Direction => -_joystick.Vertical;

    public Action<bool> OnCanonRotation { get; set; }

    [Serializable]
    private struct Canon
    {
        internal float _currentEulerAngleX;
        [SerializeField] internal float _minEulerAngleX, _maxEulerAngleX;
        [SerializeField] internal float _rotationSpeed;

        [SerializeField] internal Transform _canonPivotPoint;
        [SerializeField] internal Vector3 _rotationStabilizer;
    }
    [Serializable]
    private struct Shoot
    {
        [Header("Force")]
        [SerializeField] internal float _currentForce;
        [SerializeField] internal float _minForce, _maxForce;
        [SerializeField] internal float _smoothTime, _maxSpeed;
        internal float _currentVelocity;
        internal bool _isApplyingForce;

        [Header("Bullet")]
        [SerializeField] internal BulletController[] _bulletsPrefab;
        [SerializeField] internal Transform _shootPoint;
        [SerializeField] internal int _activeBulletIndex;
    }

    [SerializeField]
    private Canon _canon;
    [SerializeField]
    private Shoot _shoot;



    private void Awake()
    {
        _joystick = GameObject.Find(Names.VerticalJoystick).GetComponent<FixedJoystick>();
        _shootButton = FindObjectOfType<ShootButton>();
        _rigidBody = GetComponent<Rigidbody>();
        _playerTurn = GetComponent<PlayerTurn>();
        _iScore = Get<IScore>.From(gameObject);
    }

    private void OnEnable()
    {
        _shootButton.OnPointer += OnShootButtonPointer;
        _shootButton.OnClick += OnShootButtonClick;
    }

    private void OnDisable()
    {
        _shootButton.OnPointer -= OnShootButtonPointer;
        _shootButton.OnClick -= OnShootButtonClick;
    }

    private void Update()
    {
        if (_playerTurn.IsMyTurn)
        {
            RotateCanon();
            ApplyForce();
        }
    }

    public void RotateCanon()
    {
        _canon._currentEulerAngleX = _canon._canonPivotPoint.localEulerAngles.x;

        if (Direction > 0 && Converter.AngleConverter(_canon._currentEulerAngleX) > _canon._maxEulerAngleX) return;
        if (Direction < 0 && Converter.AngleConverter(_canon._currentEulerAngleX) < _canon._minEulerAngleX) return;

        _canon._canonPivotPoint.localEulerAngles = new Vector3(_canon._currentEulerAngleX += (_canon._rotationSpeed * Direction * Time.deltaTime), 0, 0) + _canon._rotationStabilizer;

        OnCanonRotation?.Invoke(Direction != 0);
    }

    private void OnShootButtonClick(bool isTrue)
    {
        if(_playerTurn.IsMyTurn) ShootBullet();
    }

    private void OnShootButtonPointer(bool isTrue)
    {
        _shoot._isApplyingForce = isTrue;
    }

    private void ShootBullet()
    {
        BulletController bullet = Instantiate(_shoot._bulletsPrefab[_shoot._activeBulletIndex], _shoot._shootPoint.position, _canon._canonPivotPoint.rotation);
        bullet.OwnerScore = _iScore;
        bullet.RigidBody.velocity = bullet.transform.forward * _shoot._currentForce;
        _rigidBody.AddForce(transform.forward * _shoot._currentForce * 1000, ForceMode.Impulse);
    }

    private void ApplyForce()
    {
        if (_shoot._isApplyingForce)
        {
            _shoot._currentForce = Mathf.SmoothDamp(_shoot._currentForce, _shoot._maxForce, ref _shoot._currentVelocity,_shoot._smoothTime * Time.deltaTime, _shoot._maxSpeed);
        }
        else
        {
            if(_shoot._currentForce != _shoot._minForce) _shoot._currentForce = _shoot._minForce;
        }

        _playerShootTrajectory.TrajectoryPrediction(_shoot._currentForce);
    }
}
