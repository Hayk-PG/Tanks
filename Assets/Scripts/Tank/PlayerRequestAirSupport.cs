using UnityEngine;
using System;

public class PlayerRequestAirSupport : MonoBehaviour
{
    private TankController _tankController;
    private PhotonPlayerRequestAirSupportRPC _photonPlayerRequestAirSupportRPC;
    private AirSupport _airSupport;
    private SupportsTabCustomization _supportsTabCustomization;
    private PlayerTurn _playerTurn;
    private ShootButton _shootButton;

    private Bomber _bomber;
    public IScore _iScore;

    private bool _isAirSupportRequested;
    private delegate Vector3 Vector();
    private delegate Quaternion Rotation();
    private Vector _bomberPosition;
    private Rotation _bomberRotation;
    private Vector3 _rightSpwnPos = new Vector3(15, 3, 0);
    private Vector3 _leftSpwnPos = new Vector3(-15, 3, 0);


    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _iScore = Get<IScore>.From(gameObject);
        _airSupport = FindObjectOfType<AirSupport>();
        _supportsTabCustomization = FindObjectOfType<SupportsTabCustomization>();
        _shootButton = FindObjectOfType<ShootButton>();
    }

    private void OnEnable()
    {
        _tankController.OnInitialize += OnInitialize;
    }
   
    private void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        _airSupport.OnRequestAirSupport += OnRequestAirSupport;
        _supportsTabCustomization.OnCallBomber -= CallBomber;
        _shootButton.OnClick -= OnShootButtonClick;
    }

    private void OnInitialize()
    {
        BomberCoordinates();
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _airSupport.OnRequestAirSupport += OnRequestAirSupport;
        _supportsTabCustomization.OnCallBomber += CallBomber;
        _shootButton.OnClick += OnShootButtonClick;
    }

    private void BomberCoordinates()
    {
        _bomberPosition = delegate
        {
            return _playerTurn.MyTurn == TurnState.Player1 ? _rightSpwnPos : _leftSpwnPos;
        };
        _bomberRotation = delegate
        {
            return _playerTurn.MyTurn == TurnState.Player1 ? Quaternion.Euler(-90, -90, 0) : Quaternion.Euler(-90, 90, 0);
        };
    }

    private void CallBomber()
    {
        if (_playerTurn.IsMyTurn)
        {
            Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, RequestAirSupport, RequestAirSupportRPC);
        }
    }

    public void RequestAirSupport()
    {
        _airSupport.CallOnRequestAirSupport();
        _isAirSupportRequested = true;
    }

    private void RequestAirSupportRPC()
    {
        if (_photonPlayerRequestAirSupportRPC == null)
            _photonPlayerRequestAirSupportRPC = _tankController.BasePlayer.GetComponent<PhotonPlayerRequestAirSupportRPC>();

        _photonPlayerRequestAirSupportRPC?.CallAirSupportRPC();
    }

    private void OnRequestAirSupport(Bomber bomber, Action<AirSupport.InstantiateProperties> ActivateBomber)
    {       
        ActivateBomber?.Invoke(new AirSupport.InstantiateProperties(_bomberPosition(), _bomberRotation()));

        _bomber = bomber;
        _bomber.distanceX = _rightSpwnPos.x;
    }

    private void OnShootButtonClick(bool isTrue)
    {
        if (isTrue && _isAirSupportRequested)
        {
            Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, DropBomb, DropBombRPC);
        }
    }

    public void DropBomb()
    {
        _isAirSupportRequested = false;
        _bomber?.DropBomb(_iScore);
    }

    private void DropBombRPC()
    {
        if (_photonPlayerRequestAirSupportRPC == null)
            _photonPlayerRequestAirSupportRPC = _tankController.BasePlayer.GetComponent<PhotonPlayerRequestAirSupportRPC>();

        _photonPlayerRequestAirSupportRPC?.CallDropBombRPC();
    }
}
