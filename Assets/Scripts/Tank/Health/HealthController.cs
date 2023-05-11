using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamage, IEndGame
{
    private TankController _tankController;

    private PlayerShields _playerShields;

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

        Health = 100;
    }

    private void OnEnable()
    {
        ManageTornadoEventSubscription(true);

        ManageShieldActivityEventSubscription(true);
    }

    private void OnDisable()
    {
        ManageTornadoEventSubscription(false);

        ManageShieldActivityEventSubscription(false);
    }

    private void ManageTornadoEventSubscription(bool isSubscribing)
    {
        if (isSubscribing)
            Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => GameSceneObjectsReferences.GameManagerBulletSerializer.OnTornado += OnTornadoDamage, () => PhotonNetwork.NetworkingClient.EventReceived += OnTornadoDamage);
        else
            Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => GameSceneObjectsReferences.GameManagerBulletSerializer.OnTornado -= OnTornadoDamage, () => PhotonNetwork.NetworkingClient.EventReceived -= OnTornadoDamage);
    }

    private void ManageShieldActivityEventSubscription(bool isSubscribing)
    {
        if (isSubscribing)
            _playerShields.onShieldActivity += OnShieldActivity;
        else
            _playerShields.onShieldActivity -= OnShieldActivity;
    }

    public void BoostSafeZone(bool isSafeZone) => IsSafeZone = isSafeZone;

    /// <summary>
    /// Use this method if damages apply by using Physics.OverlapSphere
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage, bool ignoreArmor = false)
    {
        if (Health <= 0)
            return;

        if (!IsSafeZone)
        {
            if (IsShieldActive && !ignoreArmor)
                ApplyArmorDamage(damage);
            else
                ApplyHealthDamage(damage);
        }
    }

    /// <summary>
    /// Use this method if damages are applied by interacting directly with colliders
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="damage"></param>
    public void Damage(Collider collider, int damage, bool ignoreArmor = false)
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

            PlayDamageSoundFX(2);
        }

        if(collider == _strongSpot)
        {
            newDamage = fixedDamage / 1.5f;

            PlayDamageSoundFX(2);
        }

        Damage(Mathf.RoundToInt(newDamage), ignoreArmor);
    }

    private void ApplyHealthDamage(int damage)
    {
        if (DamageValue(damage) <= 0)
            return;

        Health = (Health - DamageValue(damage)) > 0 ? Health - DamageValue(damage) : 0;
        
        OnUpdateHealthBar?.Invoke(Health);

        OnTakeDamage?.Invoke(_tankController.BasePlayer, DamageValue(damage));

        OnTankDamageFire?.Invoke(Health);

        PlayDamageSoundFX(5, true);

        AnnouncePlayerDamageFeedback();
    }

    private void ApplyArmorDamage(int damage)
    {
        if (DamageValue(damage) <= 0)
            return;

        int shieldHealth = DamageValue(damage) - ShieldHealth;

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

        PlayDamageSoundFX(5, true);

        onUpdateArmorBar?.Invoke(ShieldHealth);
    }

    private void PlayDamageSoundFX(int clipIndex, bool onlyOnLocalPlayer = false)
    {
        if (onlyOnLocalPlayer && _tankController.BasePlayer == null)
            return;

        SecondarySoundController.PlaySound(0, clipIndex);
    }

    private void AnnouncePlayerDamageFeedback()
    {
        if (_tankController.BasePlayer == null)
            return;

        GameSceneObjectsReferences.PlayerFeedbackAnnouncer.AnnounceFeedback(5, UnityEngine.Random.Range(0, SoundController.Instance.SoundsList[5]._clips.Length));
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
            Damage((int)data[1]);
    }

    private void OnTornadoDamage(object[] data) => ReceiveTornadoDamage(data);

    private void OnTornadoDamage(EventData data)
    {
        if (data.Code == EventInfo.Code_TornadoDamage)
            ReceiveTornadoDamage((object[])data.CustomData);
    }

    public void BoostHealth(int hp)
    {
        if (Health + hp <= 100)
            Health += hp;
        else
            Health = 100;

        OnUpdateHealthBar?.Invoke(Health);

        OnTankDamageFire?.Invoke(Health);
    }

    public void CameraChromaticAberrationFX()
    {
        if (_tankController.BasePlayer != null)
            FindObjectOfType<CameraChromaticAberration>().CameraGlitchFX(60);
    }

    private void OnShieldActivity(bool isActive)
    {
        if(isActive)
        {
            ShieldHealth = 100;

            onUpdateArmorBar?.Invoke(ShieldHealth);
        }
    }

    public void OnGameEnd(object[] data = null)
    {
        if (_tankController.BasePlayer != null)
            GameSceneObjectsReferences.GameplayAnnouncer.AnnounceGameResult(Health > 0);

        if (Health <= 0)
            gameObject.SetActive(false);
    }

    public void WrapUpGame(object[] data = null)
    {
        if (gameObject.activeInHierarchy)
            gameObject.SetActive(false);
    }
}
