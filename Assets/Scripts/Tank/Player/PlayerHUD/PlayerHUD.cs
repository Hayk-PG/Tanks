using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;

    private Quaternion _keepRotation = Quaternion.Euler(0, 0, 0);
    private Transform _player;
    private ShootController _shootController;
    private VehicleFall _vehicleFall;

    [SerializeField]
    private Text _angleText, _forceText;


    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();

        _player = Get<TankMovement>.From(gameObject).transform;
        _shootController = Get<ShootController>.From(gameObject);
        _vehicleFall = Get<VehicleFall>.From(gameObject);
    }

    private void Start()
    {
        _canvas.worldCamera = Camera.main;
        _canvasGroup.alpha = 0;
    }

    private void OnEnable()
    {
        if (_shootController != null) _shootController.OnUpdatePlayerHUDValues += OnUpdateValues;
        if (_vehicleFall != null) _vehicleFall.OnVehicleFell += EnablePlayerHUD;
    }

    private void OnDisable()
    {
        if (_shootController != null) _shootController.OnUpdatePlayerHUDValues -= OnUpdateValues;
        if (_vehicleFall != null) _vehicleFall.OnVehicleFell -= EnablePlayerHUD;
    }

    private void Update()
    {
        FollowThePlayer();
    }

    private void FollowThePlayer()
    {
        transform.rotation = _keepRotation;
        transform.position = new Vector3(_player.position.x - 0.5f, _player.position.y + 0.35f, 0);
    }

    private void EnablePlayerHUD()
    {
        _canvasGroup.alpha = 1;
    }

    private void OnUpdateValues(ShootController.PlayerHUDValues hudValues)
    {
        if (_angleText != null) _angleText.text = "°" + Mathf.Round(Mathf.InverseLerp(hudValues._minAngle, hudValues._maxAngle, hudValues._currentAngle) * 100);
        if (_forceText != null) _forceText.text = Mathf.Round(Mathf.InverseLerp(hudValues._minForce, hudValues._maxForce, hudValues._currentForce) * 100).ToString();
    }
}
