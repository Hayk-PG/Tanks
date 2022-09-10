using UnityEngine;

public class PlayerNewHUD : PlayerHUD
{
    protected override void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _mainCanvasGroup = GetComponent<CanvasGroup>();
        _vehicleFall = Get<VehicleFall>.From(gameObject);
    }

    protected override void OnEnable()
    {
        if (_vehicleFall != null)
            _vehicleFall.OnVehicleFell += EnablePlayerHUD;
    }

    protected override void OnDisable()
    {
        if (_vehicleFall != null)
            _vehicleFall.OnVehicleFell -= EnablePlayerHUD;
    }

    protected override void EnablePlayerHUD()
    {
        MainCanvasGroupActivity(true);
    }
}
