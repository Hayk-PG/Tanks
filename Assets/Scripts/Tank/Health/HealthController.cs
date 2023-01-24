using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamage
{
    private TankController _tankController;
    private PlayerShields _playerShields;
    private GameManagerBulletSerializer _gameManagerBulletSerializer;
  
    [SerializeField][Range(0, 50)] [Tooltip("Light => 0 - 10: Medium => 15 - 30: Heavy => 35 - 50")] private int _armor;

    public int Health { get; set; }
    public int ShieldHealth { get; set; }
    public int Armor
    {
        get => _armor;
        set => _armor = value;
    }
    public bool IsShieldActive
    {
        get
        {
            bool isShieldActiive = _playerShields != null && _playerShields.IsShieldActive ? true :
                                   _playerShields == null || _playerShields != null && !_playerShields.IsShieldActive ? false : false;

            return isShieldActiive;
        }
    }

    public Action<BasePlayer, int> OnTakeDamage { get; set; }
    public Action<int> OnUpdateHealthBar { get; set; }
    public event Action<int> onUpdateArmorBar;
    public Action<int> OnTankDamageFire { get; set; }




    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);
        _playerShields = Get<PlayerShields>.From(gameObject);
        _gameManagerBulletSerializer = FindObjectOfType<GameManagerBulletSerializer>();

        Health = 100;
    }

    private void OnEnable()
    {
        if (MyPhotonNetwork.IsOfflineMode)
            _gameManagerBulletSerializer.OnTornado += OnTornadoDamage;
        else
            PhotonNetwork.NetworkingClient.EventReceived += OnTornadoDamage;

        _playerShields.onShieldActivity += OnShieldActivity;
    }

    private void OnDisable()
    {
        if (MyPhotonNetwork.IsOfflineMode)
            _gameManagerBulletSerializer.OnTornado -= OnTornadoDamage;
        else
            PhotonNetwork.NetworkingClient.EventReceived -= OnTornadoDamage;

        _playerShields.onShieldActivity -= OnShieldActivity;
    }

    public void Damage(int damage)
    {
        if (Health <= 0)
            return;

        if (IsShieldActive)
            ApplyArmorDamage(damage);
        else
            ApplyHealthDamage(damage);
    }

    private void ApplyHealthDamage(int damage)
    {
        Health = (Health - DamageValue(damage)) > 0 ? Health - DamageValue(damage) : 0;       
        OnUpdateHealthBar?.Invoke(Health);
        OnTakeDamage?.Invoke(_tankController.BasePlayer, DamageValue(DamageValue(damage)));
        OnTankDamageFire?.Invoke(Health);
        PlayDamageSoundFX();
    }

    private void PlayDamageSoundFX()
    {
        if (_tankController.BasePlayer != null)
            SecondarySoundController.PlaySound(0, 5);
    }

    private void ApplyArmorDamage(int damage)
    {
        int shieldHealth = damage - ShieldHealth;

        Conditions<int>.Compare(shieldHealth, 0,
            delegate 
            {
                ShieldHealth = 0;
                _playerShields.DeactivateShields();
            }, 
            delegate 
            {
                ShieldHealth = 0;
                ApplyHealthDamage(shieldHealth);
                _playerShields.DeactivateShields();
            }, 
            delegate
            {
                ShieldHealth = Mathf.Abs(shieldHealth);
            });

        onUpdateArmorBar?.Invoke(ShieldHealth);
    }

    public int DamageValue(int damage)
    {
        float d = damage;
        float d1 = d / 100;
        float a = 100 - Armor;
        return Mathf.FloorToInt(d1 * a);        
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
        if (data.Code == EventInfo.Code_TornadoDamage)
        {
            ReceiveTornadoDamage((object[])data.CustomData);
        }
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

    private void OnShieldActivity(bool isActive)
    {
        if(isActive)
        {
            ShieldHealth = 100;
            onUpdateArmorBar?.Invoke(ShieldHealth);
        }
    }
}
