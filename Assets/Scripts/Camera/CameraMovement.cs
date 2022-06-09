﻿using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera _mainCamera;
    private CameraTouchMovement _cameraTouchMovement;
    private LevelGenerator _levelGenerator;

    [Header("Second Camera")]
    [SerializeField]
    private Camera _hudCamera;

    [Header("Third Camera")]
    [SerializeField]
    private Camera _vehicleCamera;

    private Vector3 _direction;
    private Transform _target;

    [Header("Parameters")]
    [SerializeField]
    private Vector3 _stabilizer;
    private Vector3 _updatedStabilizer;
    private Vector3 _currentVelocity;

    [SerializeField]
    private float _defaultLerp;
    private float _followLerp;

    [SerializeField]
    private float _currentSize, _defaultCameraSize;
    private float _givenCameraSize;
    private float _minPosX, _maxPosX, _newPosX;

    private delegate bool Checker();
    private delegate void CameraFunctions();

    private Checker _isTargetNull;
    private Checker _isCameraSizeSet;
    private Checker _canZoom;

    private float CameraWidth => _mainCamera.orthographicSize * _mainCamera.aspect;


    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
        _cameraTouchMovement = GetComponent<CameraTouchMovement>();
        _levelGenerator = FindObjectOfType<LevelGenerator>();
    }

    private void Start()
    {
        _isTargetNull = delegate { return _target == null; };
        _isCameraSizeSet = delegate { return _mainCamera.orthographicSize == _currentSize; };
        _canZoom = delegate { return CameraWidth < Vector2.Distance(new Vector2(_levelGenerator.MapHorizontalStartPoint, 0), new Vector2(_levelGenerator.MapHorizontalEndPoint, 0)) / 2; };
    }

    private void FixedUpdate()
    {
        Conditions<bool>.Compare(_isTargetNull(), KeepCurrentPosition, SetTheMovement);
        FollowTheTarget();
        SetTheCamerasSize();       
    }

    private void KeepCurrentPosition()
    {
        if (_direction != transform.localPosition) _direction = transform.localPosition;
        if (_currentSize != _defaultCameraSize) _currentSize = Mathf.Lerp(_mainCamera.orthographicSize, _defaultCameraSize, _defaultLerp * Time.fixedDeltaTime);
    }

    private void SetTheMovement()
    {
        Conditions<bool>.Compare(_cameraTouchMovement.IsCameraMoving, UpdateStabilizer, Stabilizer);

        if (_direction != _target.position) _direction = Vector3.Lerp(transform.localPosition, _target.position + _updatedStabilizer, _followLerp * Time.fixedDeltaTime);
        if (_currentSize != _givenCameraSize && _canZoom()) _currentSize = Mathf.Lerp(_mainCamera.orthographicSize, _givenCameraSize, _followLerp * Time.deltaTime);
    }

    private void UpdateStabilizer()
    {
        _updatedStabilizer += _cameraTouchMovement.TouchPosition;
    }

    private void Stabilizer()
    {
        _updatedStabilizer = _stabilizer;
    }

    private void FollowTheTarget()
    {
        transform.localPosition = ClampPosition(_direction);
    }

    private void SetTheCamerasSize()
    {
        _mainCamera.orthographicSize = _currentSize;

        if (_hudCamera != null) _hudCamera.orthographicSize = _currentSize;
        if (_vehicleCamera != null) _vehicleCamera.orthographicSize = _currentSize;
    }

    public void SetCameraTarget(Transform target, float lerp, float cameraSize)
    {
        _target = target;
        _followLerp = lerp;
        _givenCameraSize = cameraSize;
    }

    private Vector3 ClampPosition(Vector3 position)
    {
        _minPosX = _levelGenerator.MapHorizontalStartPoint + CameraWidth;
        _maxPosX = _levelGenerator.MapHorizontalEndPoint - CameraWidth;
        _newPosX = Mathf.Clamp(position.x, _minPosX, _maxPosX);
        return new Vector3(_newPosX, position.y, position.z);
    }
}
