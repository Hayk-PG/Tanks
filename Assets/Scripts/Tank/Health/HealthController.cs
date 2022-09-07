using System;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamage
{
    private TankController _tankController;
    private VehiclePool _vehiclePool;
    private PlayerShields _playerShields;
  
    [SerializeField][Range(0, 100)] private int _armor;
    private int _currentHealth;
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
    public Action<int> OnTankDamageFire { get; set; }


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
            OnTankDamageFire?.Invoke(Health);
        }
    }

    public int DamageValue(int damage)
    {
        float d = damage;
        float d1 = d / 100;
        float a = 100 - Armor;
        return Mathf.FloorToInt(d1 * a);        
    }

    public void GetHealthFromWoodBox(out bool isDone, out string text)
    {
        isDone = false;
        text = "";

        if (_tankController.BasePlayer != null)
        {
            if (Health + 20 <= 100)
                Health += 20;
            else
                Health = 100;

            isDone = true;
            text = "+" + 20;
            OnTankDamageFire?.Invoke(Health);
        }
    }

    public void CameraChromaticAberrationFX()
    {
        if(_tankController.BasePlayer != null)
        {
            FindObjectOfType<CameraChromaticAberration>().CameraGlitchFX(60);
        }
    }
}
