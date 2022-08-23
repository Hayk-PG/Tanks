using UnityEngine;

public class AIDamageData : MonoBehaviour
{
    private HealthController _healthController;



    private void Awake()
    {
        _healthController = Get<HealthController>.From(gameObject);
    }

    private void OnEnable()
    {
        _healthController.OnTankDamageFire += OnTakeDamage;
    }

    private void OnDisable()
    {
        _healthController.OnTankDamageFire -= OnTakeDamage;
    }

    private void OnTakeDamage(int health)
    {

    }
}
