using System;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamage
{
    private TankController _tankController;
    private VehiclePool _vehiclePool;
    private PlayerShields _playerShields;

    [SerializeField] private int _currentHealth;
    [SerializeField][Range(0, 100)] private int _armor;
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
  
    public Action<BasePlayer, int> OnTakeDamage { get; set; }
    public Action<int> OnUpdateHealthBar { get; set; }

    private void Awake()
    {
        Health = _maxHealth;
        _tankController = Get<TankController>.From(gameObject);
        _vehiclePool = Get<VehiclePool>.FromChild(gameObject);
        _playerShields = Get<PlayerShields>.From(gameObject);
    }

    public void Damage(int damage)
    {
        if (_playerShields == null || !_playerShields.IsShieldActive)
        {
            Health = (Health - DamageValue(damage)) > 0 ? Health - DamageValue(damage) : 0;
            _vehiclePool.Pool(0, null);

            OnUpdateHealthBar?.Invoke(Health);
            OnTakeDamage?.Invoke(_tankController.BasePlayer ,DamageValue(DamageValue(damage)));
        }
    }

    public int DamageValue(int damage)
    {
        float d = damage;
        float d1 = d / 100;
        float a = 100 - Armor;
        return Mathf.FloorToInt(d1 * a);        
    }
}
