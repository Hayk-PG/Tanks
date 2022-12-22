using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamage
{
    private TankController _tankController;
    private VehiclePool _vehiclePool;
    private PlayerShields _playerShields;
    private GameManagerBulletSerializer _gameManagerBulletSerializer;
  
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
        _gameManagerBulletSerializer = FindObjectOfType<GameManagerBulletSerializer>();
    }

    private void OnEnable()
    {
        if (MyPhotonNetwork.IsOfflineMode)
            _gameManagerBulletSerializer.OnTornado += OnTornadoDamage;
        else
            PhotonNetwork.NetworkingClient.EventReceived += OnTornadoDamage;
    }

    private void OnDisable()
    {
        if (MyPhotonNetwork.IsOfflineMode)
            _gameManagerBulletSerializer.OnTornado -= OnTornadoDamage;
        else
            PhotonNetwork.NetworkingClient.EventReceived -= OnTornadoDamage;
    }

    private void ReceiveTornadoDamage(object[] data)
    {
        if ((string)data[0] == name)
        {           
            Damage((int)data[1]);
        }
    }

    private void OnTornadoDamage(object[] data)
    {
        ReceiveTornadoDamage(data);
    }

    private void OnTornadoDamage(EventData data)
    {
        if(data.Code == EventInfo.Code_TornadoDamage)
        {
            ReceiveTornadoDamage((object[])data.CustomData);
        }
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

            OnUpdateHealthBar?.Invoke(Health);
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
