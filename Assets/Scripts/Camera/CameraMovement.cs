using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera _mainCamera;
    private CameraTouchMovement _cameraTouchMovement;
    private MapPoints _mapPoints;

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
        _mapPoints = FindObjectOfType<MapPoints>();
    }

    private void Start()
    {
        _isTargetNull = delegate { return _target == null; };
        _isCameraSizeSet = delegate { return _mainCamera.orthographicSize == _currentSize; };
        _canZoom = delegate { return CameraWidth < Vector2.Distance(new Vector2(_mapPoints.HorizontalMin, 0), new Vector2(_mapPoints.HorizontalMax, 0)) / 2; };
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

        if (_direction != _target.position) _direction = Vector3.Lerp(transform.localPosition, _target.position + new Vector3(_updatedStabilizer.x, _updatedStabilizer.y - 1, _updatedStabilizer.z), _followLerp * Time.fixedDeltaTime);
        if (_currentSize != _givenCameraSize && _canZoom()) _currentSize = Mathf.Lerp(_mainCamera.orthographicSize, _givenCameraSize, _followLerp * Time.deltaTime);
    }

    private void UpdateStabilizer()
    {
        if (_target != null)
        {
            Vector3 newPosition = _cameraTouchMovement.TouchPosition - _target.position;
            newPosition.z = 0;
            _updatedStabilizer = Vector3.Lerp(_updatedStabilizer, newPosition, 5 * Time.fixedDeltaTime);
        }
    }

    private void Stabilizer()
    {
        _updatedStabilizer = new Vector3(_stabilizer.x, _stabilizer.y + 1, _stabilizer.z);
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
        _minPosX = _mapPoints.HorizontalMin + CameraWidth;
        _maxPosX = _mapPoints.HorizontalMax - CameraWidth;
        _newPosX = Mathf.Clamp(position.x, _minPosX, _maxPosX);
        return new Vector3(_newPosX, position.y, position.z);
    }
}
