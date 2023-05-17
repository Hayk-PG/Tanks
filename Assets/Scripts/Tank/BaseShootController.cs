﻿using System;
using UnityEngine;

public class BaseShootController: MonoBehaviour
{
    protected Transform _canonPivotPoint;
    protected Transform _shootPoint;
    protected Transform _rocketSpawnPoint;
    protected BaseTrajectory _trajectory;
    protected AICanonRaycast _aiCanonRaycast;
    protected PlayerTurn _playerTurn;
    protected ScoreController _scoreController;
    protected Stun _stun;

    protected bool _isStunned, _isPrecisionShotActive;
    internal Transform CanonPivotPoint => _canonPivotPoint;

    [Serializable] public struct Canon
    {
        internal float _currentEulerAngleX;

        [Header("Canon rotation parameters")]
        public float _minEulerAngleX;
        public float _maxEulerAngleX;
        public float _rotationSpeed;
        public Vector3 _rotationStabilizer;
    }
    [Serializable] public struct Shoot
    {
        internal float _currentForce;
        internal float _maxForce;
        internal float _minForce;       
        internal float _smoothTime;
        internal float _maxSpeed;
        internal float _currentVelocity;
        internal bool _isApplyingForce;
        [SerializeField] internal float _rigidbodyForceMultiplier;
    }

    public Canon _canon;
    public Shoot _shoot;

    internal Action<bool> OnApplyingForce { get; set; }
    internal Action OnShoot { get; set; }
    public Action<float> OnDash { get; set; }


    protected virtual void Awake()
    {
        FindCanonPivotPoint();

        _shootPoint = Get<BaseTrajectory>.FromChild(_canonPivotPoint.gameObject, true).transform;
        _rocketSpawnPoint = transform.Find("RocketSpawnPoint");
        _trajectory = Get<BaseTrajectory>.From(_shootPoint.gameObject);
        _aiCanonRaycast = Get<AICanonRaycast>.From(_trajectory.gameObject);
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _scoreController = Get<ScoreController>.From(gameObject);
        _stun = Get<Stun>.FromChild(gameObject);
    }

    protected virtual void OnEnable()
    {
        if (_stun != null)
            _stun.OnStunEffect += OnStunEffect;
    }

    protected virtual void OnDisable()
    {
        if (_stun != null)
            _stun.OnStunEffect -= OnStunEffect;
    }

    protected virtual void FindCanonPivotPoint()
    {
        if (transform.Find("CanonPivotPoint") != null)
            _canonPivotPoint = transform.Find("CanonPivotPoint");

        else if (transform.Find("Turret") != null)
            _canonPivotPoint = transform.Find("Turret").Find("CanonPivotPoint");

        else
            _canonPivotPoint = transform.Find("Body").Find("Turret").Find("CanonPivotPoint");
    }

    protected virtual void OnStunEffect(bool isStunned) => _isStunned = isStunned;

    /// <summary>
    /// Sets the precision shot state of the projectile for the given <paramref name="baseBulletController"/>.
    /// </summary>
    /// <param name="baseBulletController">The base bullet controller to set the precision shot state for.</param>
    protected virtual void SetProjectilePrecisionShot(BaseBulletController baseBulletController) => baseBulletController.IsPrecisionShotAcitve = _isPrecisionShotActive;

    /// <summary>
    /// Sets the state of the precision shot to be active or inactive.
    /// </summary>
    /// <param name="isPrecisionShotActive">The flag indicating whether the precision shot should be active or inactive.</param>
    public void SetPrecisionShotActive(bool isPrecisionShotActive) => _isPrecisionShotActive = isPrecisionShotActive;
}
