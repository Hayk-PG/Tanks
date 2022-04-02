using UnityEngine;

public class ScreenText : MonoBehaviour
{
    private PlayerDamageScreenText _playerDamageScreenText;

    private HealthController _playerHealthController;



    private void Awake()
    {
        _playerDamageScreenText = Get<PlayerDamageScreenText>.FromChild(gameObject);
    }

    private void OnDisable()
    {
        if (_playerHealthController != null)
        {
            _playerHealthController.OnTakeDamage -= OnTakeDamage;
        }
    }

    public void GetPlayerHealthControllerAndSubscribeToEvent(HealthController playerHealth)
    {
        _playerHealthController = playerHealth;

        if(_playerHealthController != null)
        {
            _playerHealthController.OnTakeDamage += OnTakeDamage;
        }
    }

    private void OnTakeDamage(int damage)
    {
        if (_playerDamageScreenText != null) _playerDamageScreenText.Display(-damage);
    }
}
