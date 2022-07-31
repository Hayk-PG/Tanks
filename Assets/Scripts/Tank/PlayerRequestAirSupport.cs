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
    private AmmoTypeButton _relatedSupportTabButton;
    private Bomber _bomber;
    public IScore _iScore;

    private bool _isAirSupportRequested;


    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _iScore = Get<IScore>.From(gameObject);
        _airSupport = FindObjectOfType<AirSupport>();
        _supportsTabCustomization = FindObjectOfType<SupportsTabCustomization>();
        _shootButton = FindObjectOfType<ShootButton>();
        _relatedSupportTabButton = GlobalFunctions.ObjectsOfType<AmmoTypeButton>.Find(button => button._properties.SupportOrPropsType == Names.AirSupport);
    }

    private void OnEnable()
    {
        _tankController.OnInitialize += OnInitialize;
    }
   
    private void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        _supportsTabCustomization.OnCallBomber -= CallBomber;
        _shootButton.OnClick -= OnShootButtonClick;
    }

    private void OnInitialize()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _supportsTabCustomization.OnCallBomber += CallBomber;
        _shootButton.OnClick += OnShootButtonClick;
    }

    public void RequestAirSupport()
    {
        _airSupport.Call(out Bomber bomber, _playerTurn);
        _bomber = bomber;
        _isAirSupportRequested = true;
    }

    private void RequestAirSupportRPC()
    {
        if (_photonPlayerRequestAirSupportRPC == null)
            _photonPlayerRequestAirSupportRPC = _tankController.BasePlayer.GetComponent<PhotonPlayerRequestAirSupportRPC>();

        _photonPlayerRequestAirSupportRPC?.CallAirSupportRPC();
    }

    private void CallBomber()
    {
        if (_playerTurn.IsMyTurn)
        {
            Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => RequestAirSupport(), RequestAirSupportRPC);
            _supportsTabCustomization.OnSupportOrPropsChanged?.Invoke(_relatedSupportTabButton);
        }
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
