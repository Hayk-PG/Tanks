using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider _healthBar;

    private HealthController _healthController;


    private void Awake()
    {
        _healthController = Get<HealthController>.From(gameObject);
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
        //_sliders[index]._lastHealthFill.OnUpdate(_sliders[index]._slider.value / 100);       
    }
}
