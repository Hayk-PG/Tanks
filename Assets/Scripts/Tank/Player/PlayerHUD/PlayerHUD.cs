using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private CanvasGroup _canvasGroupShootValues;
    private TankController _tankController;
    private VehicleFall _vehicleFall;



    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroupShootValues = transform.Find("Tab_ShootValues").GetComponent<CanvasGroup>();
        _tankController = Get<TankController>.From(gameObject);
        _vehicleFall = Get<VehicleFall>.From(gameObject);
    }

    private void Start()
    {
        _canvas.worldCamera = Camera.main;
        _canvasGroup.alpha = 0;
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
   
    private void EnablePlayerHUD()
    {
        _canvasGroup.alpha = 1;

        if (_tankController?.BasePlayer != null && _canvasGroupShootValues != null)
            _canvasGroupShootValues.alpha = 1;
    }
}
