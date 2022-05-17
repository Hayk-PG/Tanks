using System;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamage
{
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _armor;
    private int _minHealth = 0;
    private int _maxHealth = 100;
    public int Health
    {
        get => _currentHealth;
        set => _currentHealth = value;
    }
    public int Armor
    {
        get => _armor;
        set => _armor = value;
    }

    private VehiclePool _vehiclePool; 
    public Action<int> OnTakeDamage { get; set; }
    public Action<int> OnUpdateHealthBar { get; set; }

    private void Awake()
    {
        Health = _maxHealth;
        _vehiclePool = Get<VehiclePool>.FromChild(gameObject);
    }

    public void Damage(int damage)
    {
        Health = (Health - DamageValue(damage)) > 0 ? Health - DamageValue(damage) : 0;
        _vehiclePool.Pool(0, null);

        OnUpdateHealthBar?.Invoke(Health);       
        OnTakeDamage?.Invoke(DamageValue(DamageValue(damage)));
    }

    public int DamageValue(int damage)
    {
        return damage / 100 * (100 - Armor);
    }
}
