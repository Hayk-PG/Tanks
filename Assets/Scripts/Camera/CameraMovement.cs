using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera _camera;
    private Vector3 _direction;
    private Transform _target;

    [SerializeField]
    private Vector3 _stabilizer;
    private Vector3 _currentVelocity;

    [SerializeField]
    private float _lerp;
    [SerializeField]
    private float _currentSize, _defaultCameraSize;
    private float _givenCameraSize;


    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        if(_target == null)
        {
            if (_direction != transform.localPosition) _direction = transform.localPosition;
            if (_currentSize != _defaultCameraSize) _currentSize = Mathf.Lerp(_camera.orthographicSize, _defaultCameraSize, _lerp * Time.fixedDeltaTime);
        }
        else
        {
            if (_direction != _target.position) _direction = Vector3.Lerp(transform.localPosition, _target.position + _stabilizer, _lerp * Time.fixedDeltaTime);
            if (_currentSize != _givenCameraSize) _currentSize = Mathf.Lerp(_camera.orthographicSize, _givenCameraSize, _lerp * Time.fixedDeltaTime);
        }

        if (transform.localPosition != _direction) transform.localPosition = _direction;
        if (_camera.orthographicSize != _currentSize) _camera.orthographicSize = _currentSize;
    }

    public void SetCameraTarget(Transform target, float cameraSize)
    {
        _target = target;
        _givenCameraSize = cameraSize;
    }
}
