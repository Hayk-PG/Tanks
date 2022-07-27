using UnityEngine;

public class PlayerHUD : MonoBehaviour
{ 
    private Canvas _canvas;
    private CanvasGroup _mainCanvasGroup;
    private CanvasGroup _canvasGroupShootValues;
    private TankController _tankController;
    private TankMovement _tankMovement;
    private PhotonPlayerEnableHUDRPC _photonPlayerEnableHUDRPC;
    private VehicleFall _vehicleFall;


    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _mainCanvasGroup = GetComponent<CanvasGroup>();
        _canvasGroupShootValues = transform.Find("Tab_ShootValues").GetComponent<CanvasGroup>();
        _tankController = Get<TankController>.From(gameObject);
        _tankMovement = Get<TankMovement>.From(gameObject);
        _vehicleFall = Get<VehicleFall>.From(gameObject);
    }

    private void Start()
    {
        _canvas.worldCamera = Camera.main;
    }

    private void OnEnable()
    {
        if (_vehicleFall != null)
            _vehicleFall.OnVehicleFell += EnablePlayerHUD;

        if (_tankMovement != null)
            _tankMovement.OnDirectionValue += OnMovementDirectionValue;
    }

    private void OnDisable()
    {
        if (_vehicleFall != null)
            _vehicleFall.OnVehicleFell -= EnablePlayerHUD;

        if (_tankMovement != null)
            _tankMovement.OnDirectionValue -= OnMovementDirectionValue;
    }

    public void MainCanvasGroupActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_mainCanvasGroup, isActive);
    }

    private void MainCanvasGroupActivityInOnlineMode()
    {
        if (_tankController.BasePlayer != null && _photonPlayerEnableHUDRPC == null)
            _photonPlayerEnableHUDRPC = Get<PhotonPlayerEnableHUDRPC>.From(_tankController.BasePlayer.gameObject);

        _photonPlayerEnableHUDRPC?.CallHUDRPC(true);
    }

    private void OnMovementDirectionValue(float direction)
    {
        if(_tankController?.BasePlayer != null)
        {
            _canvasGroupShootValues.alpha = direction == 0 ? 1 : 0;
        }
    }

    private void EnablePlayerHUD()
    {
        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => MainCanvasGroupActivity(true), MainCanvasGroupActivityInOnlineMode);
    }
}
