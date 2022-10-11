using UnityEngine;

public class Platform : MonoBehaviour
{
    private enum Direction { ToEndPoint, ToStartPoint}
    private enum PlatformType { Horizontal, Vertical}

    [SerializeField] 
    private Direction _direction;

    [SerializeField]
    private PlatformType _platformType;

    [SerializeField]
    private Rigidbody _rigidBody;

    private PlatformSensors[] _platformSensors;
    private PlatformSerializer _platformSerializer;
    private TankController _collidedTanksController;

    [SerializeField]
    private Vector3 _startPoint, _endPoint;
    private Vector3 _target;
    private Vector3 _currentVelocity;

    [SerializeField]
    private float _smoothTime, _maxSpeed;

    private float DistanceFromStartPoint
    {
        get
        {
            return Vector3.Distance(_rigidBody.transform.position, _startPoint);
        }
    }
    private float DistanceFromEndPoint
    {
        get
        {
            return Vector3.Distance(_rigidBody.transform.position, _endPoint);
        }
    }


    private void Awake()
    {
        _rigidBody.useGravity = false;
        _rigidBody.transform.position = _startPoint;
        _platformSensors = GetComponentsInChildren<PlatformSensors>();
        _platformSerializer = FindObjectOfType<PlatformSerializer>();
    }

    private void Start()
    {
        InitializePlatformSerializer();
    }

    private void OnEnable()
    {
        GlobalFunctions.Loop<PlatformSensors>.Foreach(_platformSensors, platformSensor => 
        {
            platformSensor.OnTriggerEntered += OnPlatformSensorTriggerEntered;
        });
    }

    private void OnDisable()
    {
        GlobalFunctions.Loop<PlatformSensors>.Foreach(_platformSensors, platformSensor =>
        {
            platformSensor.OnTriggerEntered -= OnPlatformSensorTriggerEntered;
        });
    }

    private void FixedUpdate()
    {
        if (DistanceFromStartPoint <= 0.1f && _direction != Direction.ToEndPoint)
            _direction = Direction.ToEndPoint;

        if(DistanceFromEndPoint <= 0.1f && _direction != Direction.ToStartPoint)
            _direction = Direction.ToStartPoint;

        _target = _direction == Direction.ToEndPoint ? _endPoint : _direction == Direction.ToStartPoint ? _startPoint : _rigidBody.transform.position;
        _rigidBody.transform.position = Vector3.SmoothDamp(_rigidBody.transform.position, _target, ref _currentVelocity, _smoothTime, _maxSpeed);
        _rigidBody.rotation = Quaternion.identity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_collidedTanksController == null)
        {
            _collidedTanksController = Get<TankController>.From(other.gameObject);
            _collidedTanksController?.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(_collidedTanksController != null && _collidedTanksController == Get<TankController>.From(other.gameObject))
        {
            _collidedTanksController.transform.SetParent(null);
            _collidedTanksController = null;
        }
    }

    private void InitializePlatformSerializer()
    {
        if (_platformType == PlatformType.Horizontal)
            _platformSerializer.RigidbodyPlatformHor = _rigidBody;

        else
            _platformSerializer.RigidbodyPlatformVert = _rigidBody;
    }

    private void OnPlatformSensorTriggerEntered(PlatformSensors.SensorDirection sensorDirection, Vector3 newPosition)
    {
        if (_platformType == PlatformType.Horizontal)
        {
            if (sensorDirection == PlatformSensors.SensorDirection.Bottom)
            {
                _startPoint += newPosition;
                _endPoint += newPosition;
            }

            if (sensorDirection == PlatformSensors.SensorDirection.Left)
                _startPoint += newPosition;

            if (sensorDirection == PlatformSensors.SensorDirection.Right)
                _endPoint += newPosition;
        }
        else
        {
            if (sensorDirection == PlatformSensors.SensorDirection.Bottom)
            {
                _startPoint += newPosition;
            }

            if (sensorDirection == PlatformSensors.SensorDirection.Left || sensorDirection == PlatformSensors.SensorDirection.Right)
            {
                _endPoint += newPosition;
                _startPoint += newPosition;
            }
        }
    }
}
