using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private LastHealthFill _lastHealthFill;
    private HealthController _healthController;
    private TankController _tankController;
    private PhotonPlayerLastHealthFillUpdateRPC _photonPlayerLastHealthFillUpdateRPC;

    public float Value
    {
        get => _healthBar.value;
        set => _healthBar.value = value;
    }
    public LastHealthFill LastHealthFill => _lastHealthFill;

    private void Awake()
    {
        _healthController = Get<HealthController>.From(gameObject);
        _tankController = Get<TankController>.From(gameObject);
    }

    private void OnEnable()
    {
        _healthController.OnUpdateHealthBar += OnHealthBar;
    }

    private void OnDisable()
    {
        _healthController.OnUpdateHealthBar -= OnHealthBar;
    }

    private void OnHealthBar(int newValue)
    {
        Value = newValue;
        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, UpdateLastHealthFill, UpdateLastHealthFillRPC);
    }

    private void UpdateLastHealthFill()
    {
        if (LastHealthFill != null) LastHealthFill.OnUpdate(Value / 100);
    }

    private void UpdateLastHealthFillRPC()
    {
        if (_photonPlayerLastHealthFillUpdateRPC != null)
            _photonPlayerLastHealthFillUpdateRPC = _tankController?.BasePlayer.GetComponent<PhotonPlayerLastHealthFillUpdateRPC>();

        if (LastHealthFill != null) _photonPlayerLastHealthFillUpdateRPC?.CallHealthBarLastFillUpdateRPC(Value / 100);
    }
}
