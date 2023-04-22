using UnityEngine;

public class Platform : MonoBehaviour
{
    private enum Direction { ToEndPoint, ToStartPoint }
    private enum PlatformType { Horizontal, Vertical }

    [SerializeField] private Direction _direction;
    [SerializeField] private PlatformType _platformType;

    private PlatformSensors[] _platformSensors;
    private TankController _collidedTanksController;

    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private Vector3 _target;

    private float DistanceFromStartPoint => Vector3.Distance(transform.position, _startPoint);
    private float DistanceFromEndPoint => Vector3.Distance(transform.position, _endPoint);

    public Vector3? SynchedPosition { get; set; }



    private void Awake()
    {
        transform.position = _startPoint;
        _platformSensors = GetComponentsInChildren<PlatformSensors>();
    }

    private void OnEnable()
    {
        //GlobalFunctions.Loop<PlatformSensors>.Foreach(_platformSensors, platformSensor =>
        //{
        //    platformSensor.OnTriggerEntered += OnPlatformSensorTriggerEntered;
        //});
    }

    private void OnDisable()
    {
        //GlobalFunctions.Loop<PlatformSensors>.Foreach(_platformSensors, platformSensor =>
        //{
        //    platformSensor.OnTriggerEntered -= OnPlatformSensorTriggerEntered;
        //});
    }

    private void Update()
    {
        if (MyPhotonNetwork.IsOfflineMode || MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
        {
            if (DistanceFromStartPoint <= 0.1f && _direction != Direction.ToEndPoint)
                _direction = Direction.ToEndPoint;

            if (DistanceFromEndPoint <= 0.1f && _direction != Direction.ToStartPoint)
                _direction = Direction.ToStartPoint;

            _target = _direction == Direction.ToEndPoint ? _endPoint : _direction == Direction.ToStartPoint ? _startPoint : transform.position;
            transform.position = Vector3.MoveTowards(transform.position, _target, 1 * Time.deltaTime);
            transform.rotation = Quaternion.identity;
            SynchedPosition = transform.position;
        }
        else if (!MyPhotonNetwork.IsOfflineMode && !MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
        {
            if(SynchedPosition.HasValue)
            {
                if(Vector3.Distance(transform.position, SynchedPosition.Value) <= 1)
                {
                    transform.position = Vector3.MoveTowards(transform.position, SynchedPosition.Value, 1 * Time.deltaTime);
                }
                else
                {
                    transform.position = SynchedPosition.Value;
                }
             
                transform.rotation = Quaternion.identity;
            }
        }
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
        if (_collidedTanksController != null && _collidedTanksController == Get<TankController>.From(other.gameObject))
        {
            _collidedTanksController.transform.SetParent(null);
            _collidedTanksController = null;
        }
    }

    public void Set(Vector3 start, Vector3 end)
    {
        _startPoint = start;
        _endPoint = end;
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