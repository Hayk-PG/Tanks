using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Platform : MonoBehaviourPun
{
    private enum Direction { ToEndPoint, ToStartPoint}
    private enum PlatformType { Horizontal, Vertical}

    [SerializeField] private Direction _direction;
    [SerializeField] private PlatformType _platformType;
    [SerializeField] private Rigidbody _rigidBody;

    private PlatformSensors[] _platformSensors;
    private TankController _collidedTanksController;

    private object[] _horizontalPlatformData;
    private object[] _verticalPlatformData;

    private Vector3 _startPoint, _endPoint;
    private Vector3 _target;
    private Vector3 _currentVelocity;
    private Vector3 _platformPositionFromMasterClientSide;

    [SerializeField] private float _smoothTime, _maxSpeed;

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

        _platformPositionFromMasterClientSide = _rigidBody.position;
    }

    private void OnEnable()
    {
        GlobalFunctions.Loop<PlatformSensors>.Foreach(_platformSensors, platformSensor => 
        {
            platformSensor.OnTriggerEntered += OnPlatformSensorTriggerEntered;
        });

        PhotonNetwork.NetworkingClient.EventReceived += GetEventData;
    }

    private void OnDisable()
    {
        GlobalFunctions.Loop<PlatformSensors>.Foreach(_platformSensors, platformSensor =>
        {
            platformSensor.OnTriggerEntered -= OnPlatformSensorTriggerEntered;
        });

        PhotonNetwork.NetworkingClient.EventReceived -= GetEventData;
    }

    private void FixedUpdate()
    {
        if (MyPhotonNetwork.IsOfflineMode || MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
        {
            UpdatePositionOnMasterClient();
        }

        if (!MyPhotonNetwork.IsOfflineMode && MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
        {
            RaiseEvent();
        }

        if(!MyPhotonNetwork.IsOfflineMode && !MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
        {
            _rigidBody.position = Vector3.SmoothDamp(_rigidBody.position, _platformPositionFromMasterClientSide, ref _currentVelocity, _smoothTime, _maxSpeed);
            _rigidBody.rotation = Quaternion.identity;
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

    private void UpdatePositionOnMasterClient()
    {
        if (DistanceFromStartPoint <= 0.1f && _direction != Direction.ToEndPoint)
            _direction = Direction.ToEndPoint;

        if (DistanceFromEndPoint <= 0.1f && _direction != Direction.ToStartPoint)
            _direction = Direction.ToStartPoint;

        _target = _direction == Direction.ToEndPoint ? _endPoint : _direction == Direction.ToStartPoint ? _startPoint : _rigidBody.position;
        _rigidBody.position = Vector3.SmoothDamp(_rigidBody.position, _target, ref _currentVelocity, _smoothTime, _maxSpeed);
        _rigidBody.rotation = Quaternion.identity;
    }

    private void RaiseEvent()
    {
        if (_platformType == PlatformType.Horizontal)
        {
            EventInfo.Content_HorizontalPlatform = new object[] { _rigidBody.position, _direction };
            PhotonNetwork.RaiseEvent(EventInfo.Code_HorizontalPlatform, EventInfo.Content_HorizontalPlatform, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);
        }

        if (_platformType == PlatformType.Vertical)
        {
            EventInfo.Content_VerticalPlatform = new object[] { _rigidBody.position, _direction };
            PhotonNetwork.RaiseEvent(EventInfo.Code_VerticalPlatform, EventInfo.Content_VerticalPlatform, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);
        }
    }

    private void GetEventData(EventData eventData)
    {
        if(_platformType == PlatformType.Horizontal && eventData.Code == EventInfo.Code_HorizontalPlatform)
        {
            _horizontalPlatformData = (object[])eventData.CustomData;
            _platformPositionFromMasterClientSide = (Vector3)_horizontalPlatformData[0];
            _direction = (Direction)_horizontalPlatformData[1];
        }
        if (_platformType == PlatformType.Vertical && eventData.Code == EventInfo.Code_VerticalPlatform)
        {
            _verticalPlatformData = (object[])eventData.CustomData;
            _platformPositionFromMasterClientSide = (Vector3)_verticalPlatformData[0];
            _direction = (Direction)_verticalPlatformData[1];
        }
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
