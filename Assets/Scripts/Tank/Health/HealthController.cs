using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamage
{
    private TankController _tankController;

    private PlayerShields _playerShields;

    private GameManagerBulletSerializer _gameManagerBulletSerializer;

    [SerializeField]
    private Collider _weakSpot, _center, _strongSpot;

    [SerializeField] [Space] [Range(0, 50)] [Tooltip("Light => 0 - 10: Medium => 15 - 30: Heavy => 35 - 50")]
    private int _armor;

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
    public bool IsSafeZone { get; set; }

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
        ManageTornadoEventSubscription(true);

        ManageShieldActivityEventSubscription(true);

        ManageDropBoxSelectionPanelHealthEventSubscription(true);
    }

    private void OnDisable()
    {
        ManageTornadoEventSubscription(false);

        ManageShieldActivityEventSubscription(false);

        ManageDropBoxSelectionPanelHealthEventSubscription(false);
    }

    private void ManageTornadoEventSubscription(bool isSubscribing)
    {
        if (isSubscribing)
            Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => _gameManagerBulletSerializer.OnTornado += OnTornadoDamage, () => PhotonNetwork.NetworkingClient.EventReceived += OnTornadoDamage);
        else
            Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => _gameManagerBulletSerializer.OnTornado -= OnTornadoDamage, () => PhotonNetwork.NetworkingClient.EventReceived -= OnTornadoDamage);
    }

    private void ManageShieldActivityEventSubscription(bool isSubscribing)
    {
        if (isSubscribing)
            _playerShields.onShieldActivity += OnShieldActivity;
        else
            _playerShields.onShieldActivity -= OnShieldActivity;
    }

    private void ManageDropBoxSelectionPanelHealthEventSubscription(bool isSubscribing)
    {
        if (isSubscribing)
        {
            GlobalFunctions.Loop<DropBoxSelectionPanelHealth>.Foreach(GameSceneObjectsReferences.DropBoxSelectionPanelHealth, dropBoxSelectionPanelHealth =>
            {
                dropBoxSelectionPanelHealth.onUpdateHealth += GetHealthFromDropBoxSelectionPanel;
            });
        }
        else
        {
            GlobalFunctions.Loop<DropBoxSelectionPanelHealth>.Foreach(GameSceneObjectsReferences.DropBoxSelectionPanelHealth, dropBoxSelectionPanelHealth =>
            {
                dropBoxSelectionPanelHealth.onUpdateHealth -= GetHealthFromDropBoxSelectionPanel;
            });
        }
    }

    public void BoostSafeZone(bool isSafeZone) => IsSafeZone = isSafeZone;

    public void Damage(int damage)
    {
        if (Health <= 0)
            return;

        if (!IsSafeZone)
        {
            if (IsShieldActive)
                ApplyArmorDamage(damage);
            else
                ApplyHealthDamage(damage);
        }
    }

    public void Damage(Collider collider, int damage)
    {
        float fixedDamage = damage;
        float newDamage = 0;

        if (collider == _weakSpot)
        {
            newDamage = fixedDamage * 1.5f;
        }

        if(collider == _center)
        {
            newDamage = fixedDamage;

            SecondarySoundController.PlaySound(0, 2);
        }

        if(collider == _strongSpot)
        {
            newDamage = fixedDamage / 1.5f;

            SecondarySoundController.PlaySound(0, 2);
        }

        Damage(Mathf.RoundToInt(newDamage));
    }

    private void ApplyHealthDamage(int damage)
    {
        Health = (Health - DamageValue(damage)) > 0 ? Health - DamageValue(damage) : 0;
        
        OnUpdateHealthBar?.Invoke(Health);
        OnTakeDamage?.Invoke(_tankController.BasePlayer, DamageValue(DamageValue(damage)));
        OnTankDamageFire?.Invoke(Health);

        PlayDamageSoundFX();
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

        PlayDamageSoundFX();

        onUpdateArmorBar?.Invoke(ShieldHealth);
    }

    private void PlayDamageSoundFX()
    {
        if (_tankController.BasePlayer != null)
            SecondarySoundController.PlaySound(0, 5);
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

    private void GetHealthFromDropBoxSelectionPanel(int health)
    {
        if (_tankController.BasePlayer != null)
        {
            if (Health + health <= 100)
                Health += health;
            else
                Health = 100;

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
