﻿using System;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    FixedJoystick _joystick;
    ShootButton _shootButton;
    Rigidbody _rigidBody;

    [SerializeField] PlayerShootTrajectory _playerShootTrajectory;

    public float Direction => -_joystick.Vertical;

    [Serializable] struct Canon
    {
        internal float _currentEulerAngleX;
        [SerializeField] internal float _minEulerAngleX, _maxEulerAngleX;
        [SerializeField] internal float _rotationSpeed;

        [SerializeField] internal Transform _canonPivotPoint;
        [SerializeField] internal Vector3 _rotationStabilizer;
    }
    [Serializable] struct Shoot
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

    [SerializeField] Canon _canon;
    [SerializeField] Shoot _shoot;

    

    void Awake()
    {
        _joystick = GameObject.Find(Names.VerticalJoystick).GetComponent<FixedJoystick>();
        _shootButton = FindObjectOfType<ShootButton>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        _shootButton.OnPointer += OnShootButtonPointer;
        _shootButton.OnClick += OnShootButtonClick;
    }
   
    void OnDisable()
    {
        _shootButton.OnPointer -= OnShootButtonPointer;
        _shootButton.OnClick -= OnShootButtonClick;
    }

    void Update()
    {
        RotateCanon();
        ApplyForce();
    }

    public void RotateCanon()
    {
        _canon._currentEulerAngleX = _canon._canonPivotPoint.localEulerAngles.x;

        if (Direction > 0 && Converter.AngleConverter(_canon._currentEulerAngleX) > _canon._maxEulerAngleX) return;
        if (Direction < 0 && Converter.AngleConverter(_canon._currentEulerAngleX) < _canon._minEulerAngleX) return;

        _canon._canonPivotPoint.localEulerAngles = new Vector3(_canon._currentEulerAngleX += (_canon._rotationSpeed * Direction * Time.deltaTime), 0, 0) + _canon._rotationStabilizer;
    }

    void OnShootButtonClick(bool isTrue)
    {
        ShootBullet();
    }

    void OnShootButtonPointer(bool isTrue)
    {
        _shoot._isApplyingForce = isTrue;
    }

    void ShootBullet()
    {
        BulletController bullet = Instantiate(_shoot._bulletsPrefab[_shoot._activeBulletIndex], _shoot._shootPoint.position, _canon._canonPivotPoint.rotation);
        bullet.RigidBody.velocity = bullet.transform.forward * _shoot._currentForce;
        _rigidBody.AddForce(transform.forward * _shoot._currentForce * 1000, ForceMode.Impulse);
    }

    void ApplyForce()
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
