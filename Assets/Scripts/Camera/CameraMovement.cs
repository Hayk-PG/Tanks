using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Camera _camera;
    Vector3 _direction;
    Transform _player;

    [SerializeField] Vector3 _stabilizer;
    Vector3 _currentVelocity;

    [SerializeField] float _lerp, _minCameraSize, _maxCameraSize;



    void Awake()
    {
        _camera = GetComponent<Camera>();
        _player = GameObject.FindGameObjectWithTag(Tags.Player).transform;
    }

    void FixedUpdate()
    {
        _direction = Vector3.Lerp(transform.localPosition, _player.position + _stabilizer, _lerp * Time.fixedDeltaTime);
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _minCameraSize, _lerp * Time.fixedDeltaTime);

        transform.localPosition = _direction;
    }
}
