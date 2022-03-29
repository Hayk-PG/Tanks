using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider _healthBar;

    private HealthController _healthController;
    private LastHealthFill _lastHealthFill;


    private void Awake()
    {
        _healthController = Get<HealthController>.From(gameObject);
        _lastHealthFill = GetComponentInChildren<LastHealthFill>();
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
        _healthBar.value = newValue;
        if(_lastHealthFill != null) _lastHealthFill.OnUpdate(_healthBar.value / 100);       
    }
}
