using System;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamage
{
    [SerializeField]
    private int _currentHealth;
    private int _minHealth = 0;
    private int _maxHealth = 100;

    private VehiclePool _vehiclePool;

    public int Health
    {
        get => _currentHealth;
        set => _currentHealth = value;
    }
    public Action OnTakeDamage { get; set; }
    public Action<int> OnUpdateHealthBar { get; set; }

    private void Awake()
    {
        Health = _maxHealth;
        _vehiclePool = Get<VehiclePool>.FromChild(gameObject);
    }

    public void Damage(int damage)
    {
        Health = (Health - damage) > 0 ? Health - damage : 0;
        _vehiclePool.Pool(0, null);

        OnUpdateHealthBar?.Invoke(Health);       
        OnTakeDamage?.Invoke();
    }
}
