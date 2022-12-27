using UnityEngine;
using TMPro;

public class PlayerNewHUDHealth : MonoBehaviour
{
    [SerializeField] private TMP_Text _textHealth;
    private HealthController _healthController;



    private void Awake()
    {
        _healthController = Get<HealthController>.From(gameObject);
    }

    private void OnEnable()
    {
        _healthController.OnUpdateHealthBar += OnUpdateHealthText;
    }

    private void OnDisable()
    {
        _healthController.OnUpdateHealthBar -= OnUpdateHealthText;
    }

    private void OnUpdateHealthText(int health)
    {
        _textHealth.text = health.ToString();
    }
}
