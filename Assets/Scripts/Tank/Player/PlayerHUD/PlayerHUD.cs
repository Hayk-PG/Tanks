using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;

    private VehicleFall _vehicleFall;



    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();

        _vehicleFall = Get<VehicleFall>.From(gameObject);
    }

    private void Start()
    {
        _canvas.worldCamera = Camera.main;
        _canvasGroup.alpha = 0;
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
    }
}
