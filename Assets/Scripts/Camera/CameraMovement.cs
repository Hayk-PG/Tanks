﻿using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera _mainCamera;

    [Header("Second Camera")]
    [SerializeField]
    private Camera _hudCamera;

    private Vector3 _direction;
    private Transform _target;

    [Header("Parameters")]
    [SerializeField]
    private Vector3 _stabilizer;
    private Vector3 _currentVelocity;

    [SerializeField]
    private float _lerp;
    [SerializeField]
    private float _currentSize, _defaultCameraSize;
    private float _givenCameraSize;

    private delegate bool Checker();
    private delegate void CameraFunctions();

    private Checker _isTargetNull;
    private Checker _isCameraPositionMatchesToTheTargetPosition;
    private Checker _isCameraSizeSet;


    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        _isTargetNull = delegate { return _target == null; };
        _isCameraPositionMatchesToTheTargetPosition = delegate { return transform.localPosition == _direction; };
        _isCameraSizeSet = delegate { return _mainCamera.orthographicSize == _currentSize; };
    }
  
    private void FixedUpdate()
    {
        Conditions<bool>.Compare(_isTargetNull(), KeepCurrentPosition, SetTheMovement);
        Conditions<bool>.Compare(_isCameraPositionMatchesToTheTargetPosition(), null, FollowTheTarget);
        Conditions<bool>.Compare(_isCameraSizeSet(), null, SetTheCamerasSize);
    }

    private void KeepCurrentPosition()
    {
        if (_direction != transform.localPosition) _direction = transform.localPosition;
        if (_currentSize != _defaultCameraSize) _currentSize = Mathf.Lerp(_mainCamera.orthographicSize, _defaultCameraSize, _lerp * Time.fixedDeltaTime);
    }

    private void SetTheMovement()
    {
        if (_direction != _target.position) _direction = Vector3.Lerp(transform.localPosition, _target.position + _stabilizer, _lerp * Time.fixedDeltaTime);
        if (_currentSize != _givenCameraSize) _currentSize = Mathf.Lerp(_mainCamera.orthographicSize, _givenCameraSize, _lerp * Time.fixedDeltaTime);
    }

    private void FollowTheTarget()
    {
        transform.localPosition = _direction;
    }

    private void SetTheCamerasSize()
    {
        _mainCamera.orthographicSize = _currentSize;
        if(_hudCamera != null) _hudCamera.orthographicSize = _currentSize;
    }

    public void SetCameraTarget(Transform target, float cameraSize)
    {
        _target = target;
        _givenCameraSize = cameraSize;
    }
}
