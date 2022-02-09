using UnityEngine;

public class HealthController : MonoBehaviour, IDamage
{
    [SerializeField]
    private float _currentHealth;
    private float _minHealth = 0;
    private float _maxHealth = 100;

    public float Health
    {
        get => _currentHealth;
        set => _currentHealth = value;
    }


    private void Awake()
    {
        Health = _maxHealth;
    }

    public void Damage(float damage)
    {
        Health -= damage;
        print(Health);
    }
}
