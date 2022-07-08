using UnityEngine;

public class PlayerHUD : MonoBehaviour
{ 
    private Canvas _canvas;
    private CanvasGroup _mainCanvasGroup;
    private CanvasGroup _canvasGroupShootValues;
    private TankController _tankController;
    private PhotonPlayerEnableHUDRPC _photonPlayerEnableHUDRPC;
    private VehicleFall _vehicleFall;


    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _mainCanvasGroup = GetComponent<CanvasGroup>();
        _canvasGroupShootValues = transform.Find("Tab_ShootValues").GetComponent<CanvasGroup>();
        _tankController = Get<TankController>.From(gameObject);
        _vehicleFall = Get<VehicleFall>.From(gameObject);
    }

    private void Start()
    {
        _canvas.worldCamera = Camera.main;
        _mainCanvasGroup.alpha = 0;
        if(_canvasGroupShootValues != null) _canvasGroupShootValues.alpha = 0;
    }

    private void OnEnable()
    {
        if (_vehicleFall != null) _vehicleFall.OnVehicleFell += EnablePlayerHUD;
    }

    private void OnDisable()
    {
        if (_vehicleFall != null) _vehicleFall.OnVehicleFell -= EnablePlayerHUD;
    }

    public void MainCanvasGroupActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_mainCanvasGroup, isActive);
    }

    private void MainCanvasGroupActivityInOnlineMode()
    {
        if (_tankController.BasePlayer != null)
        {
            if(_photonPlayerEnableHUDRPC == null)
                _photonPlayerEnableHUDRPC = Get<PhotonPlayerEnableHUDRPC>.From(_tankController.BasePlayer.gameObject);
        }

        _photonPlayerEnableHUDRPC?.CallHUDRPC(true);
    }

    private void ShootValuesCanvasGroupActivity()
    {
        if (_tankController?.BasePlayer != null && _canvasGroupShootValues != null)
            _canvasGroupShootValues.alpha = 1;
    }

    private void EnablePlayerHUD()
    {
        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, delegate { MainCanvasGroupActivity(true); ShootValuesCanvasGroupActivity(); }, MainCanvasGroupActivityInOnlineMode);
    }
}
