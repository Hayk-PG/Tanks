using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

public class PlayerAmmoType : MonoBehaviour
{
    private TankController _tankController;

    private ShootController _shootController;

    private ScoreController _scoreController;

    private PlayerTurn _playerTurn;

    
    [Header("Scriptable objects")]

    public WeaponProperties[] _weapons;


    [Header("Cached bullets count from scriptable objects")]

    public List<int> _weaponsBulletsCount;

    private int _defaultWeaponsLength;

    //Serialized
    public int[] WeaponsBulletsCount
    {
        get => _weaponsBulletsCount.ToArray();
        set => _weaponsBulletsCount = value.ToList();
    }

    internal Action<int> OnWeaponChanged { get; set; }

    internal event Action<bool, int> onRocketSelected;







    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);

        _shootController = Get<ShootController>.From(gameObject);

        _scoreController = Get<ScoreController>.From(gameObject);

        _playerTurn = Get<PlayerTurn>.From(gameObject);

        GameSceneObjectsReferences.TurnController.OnTurnChanged += Reload;     
    }

    private void OnEnable()
    {
        _tankController.OnInitialize += OnInitialize;

        DropBoxSelectionHandler.onItemSelect += AddWeaponFromDropBoxPanel;

        DropBoxSelectionHandler.onItemSelect += UpdateAmmoFromDropBoxPanel;
    }

    private void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;

        GameSceneObjectsReferences.AmmoTabCustomization.OnPlayerWeaponChanged -= OnPlayerWeaponChanged;

        DropBoxSelectionHandler.onItemSelect -= AddWeaponFromDropBoxPanel;

        DropBoxSelectionHandler.onItemSelect -= UpdateAmmoFromDropBoxPanel;
    }

    private void OnInitialize()
    {
        SubscribeToAmmoTabCustomizationEvent();

        InitializeBulletsCountList();

        InstantiateAmmoTypeButton();       
    }

    private void SubscribeToAmmoTabCustomizationEvent() => GameSceneObjectsReferences.AmmoTabCustomization.OnPlayerWeaponChanged += OnPlayerWeaponChanged;

    private void InitializeBulletsCountList()
    {
        GlobalFunctions.Loop<WeaponProperties>.Foreach(_weapons, weapon => { _weaponsBulletsCount.Add(weapon._value); });
    }

    private void InstantiateAmmoTypeButton()
    {
        bool _hasWeapons = _weapons != null;

        if (_hasWeapons)
        {
            for (int i = 0; i < _weapons.Length; i++)
            {
                GameSceneObjectsReferences.AmmoTabCustomization.InstantiateAmmoTypeButton(_weapons[i], i);

                _defaultWeaponsLength++;
            }
        }
    }

    private void OnPlayerWeaponChanged(AmmoTypeButton ammoTypeButton)
    {
        bool isWeaponIndexWithinRange = ammoTypeButton._properties.Index < _weapons.Length;

        if (isWeaponIndexWithinRange)
            _shootController.ActiveAmmoIndex = ammoTypeButton._properties.Index;
        else
            _shootController.ActiveAmmoIndex = _weapons.Length - 1;

        GetMoreBullets(ammoTypeButton);

        UpdateDisplayedWeapon(_shootController.ActiveAmmoIndex);

        SetBulletSpecs(ammoTypeButton);

        OnPlayerSelectedRocket(ammoTypeButton);

        SetActiveWeaponType(ammoTypeButton._properties._buttonType);

        PlaySoundEffectOnWeaponChange();

        OnWeaponChanged?.Invoke(_shootController.ActiveAmmoIndex);
    }

    private void OnPlayerSelectedRocket(AmmoTypeButton ammoTypeButton)
    {
        bool hasAvailableRockets = ammoTypeButton._properties._buttonType == ButtonType.Rocket && ammoTypeButton._properties.Quantity > 0;

        if (hasAvailableRockets)
        {
            for (int i = 0; i < GameSceneObjectsReferences.DropBoxSelectionPanelRockets.Length; i++)
            {
                bool isWeaponMatchingSelectedRocket = GameSceneObjectsReferences.DropBoxSelectionPanelRockets[i].Weapon == _weapons[ammoTypeButton._properties.Index];

                if (isWeaponMatchingSelectedRocket)
                {
                    int id = GameSceneObjectsReferences.DropBoxSelectionPanelRockets[i].Id;

                    RaiseOnRocketSelectedEvent(true, id);

                    break;
                }
                else
                    RaiseOnRocketSelectedEvent(false);
            }
        }
        else
            RaiseOnRocketSelectedEvent(false);
    }

    private void RaiseOnRocketSelectedEvent(bool isSelected, int id = 0) => onRocketSelected?.Invoke(isSelected, id);

    // Don't need GetMoreBullets method for now.
    // Will come to this later.

    private void GetMoreBullets(AmmoTypeButton ammoTypeButton)
    {
        //if (_weaponsBulletsCount[_shootController.ActiveAmmoIndex] <= 0)
        //    _weaponsBulletsCount[_shootController.ActiveAmmoIndex] += ammoTypeButton._properties.Quantity;
    }

    public void UpdateDisplayedWeapon(int index)
    {
        GameSceneObjectsReferences.AmmoTabCustomization.OnUpdateDisplayedWeapon?.Invoke(_weapons[index], _weaponsBulletsCount[index]); 
    }

    public void SetBulletSpecs(AmmoTypeButton ammoTypeButton)
    {
        bool isCurrentWeaponRailgun = ammoTypeButton._properties._buttonType == ButtonType.Railgun;

        // To maintain the projectile's high velocity, set the minForce value equal to maxForce if the current weapon is a Railgun.

        float minForce = isCurrentWeaponRailgun ? ammoTypeButton._properties.BulletMaxForce : 3;

        _shootController._shoot._minForce = minForce;
        _shootController._shoot._smoothTime = 1;
        _shootController._shoot._maxForce = ammoTypeButton._properties.BulletMaxForce;
        _shootController._shoot._maxSpeed = ammoTypeButton._properties.BulletForceMaxSpeed;
    }

    private void SetActiveWeaponType(ButtonType weaponType) => _shootController.GetActiveWeaponType(weaponType);

    private void PlaySoundEffectOnWeaponChange()
    {
        int clipIndex = _shootController.ActiveAmmoIndex < 3 ? _shootController.ActiveAmmoIndex : UnityEngine.Random.Range(0, 3);

        PlayProjectileChangeSoundEffectAfterDelay(clipIndex);
    }

    private void Reload(TurnState turnState)
    {
        bool canReload = GameSceneObjectsReferences.GameManager.IsGameStarted && !GameSceneObjectsReferences.GameManager.IsGameEnded && _playerTurn.IsMyTurn;

        if (canReload)
            PlayProjectileChangeSoundEffectAfterDelay(UnityEngine.Random.Range(4, 9), 0.4f);
    }

    public void SwitchToDefaultWeapon(int index)
    {
        bool noAmmoAvailable = _weaponsBulletsCount[index] <= 0;

        if (noAmmoAvailable)
        {
            GameSceneObjectsReferences.AmmoTabCustomization.SetDefaultAmmo(null);

            PlayProjectileChangeSoundEffectAfterDelay(3);
        }
    }

    public void AddWeaponFromDropBoxPanel(DropBoxItemType dropBoxItemType, object[] data)
    {
        bool hasLocalPlayerPickedRocket = dropBoxItemType == DropBoxItemType.Rocket && _tankController.BasePlayer != null;

        if (hasLocalPlayerPickedRocket)
        {
            WeaponProperties newWeapon = (WeaponProperties)data[0];

            int id = (int)data[1];
            int price = (int)data[2];

            WeaponProperties[] weaponsTempCollection = new WeaponProperties[_weapons.Length + 1];

            for (int i = 0; i < _weapons.Length; i++)
                weaponsTempCollection[i] = _weapons[i];

            weaponsTempCollection[weaponsTempCollection.Length - 1] = newWeapon;

            _weapons = weaponsTempCollection;

            _weaponsBulletsCount.Add(newWeapon._value);

            _scoreController.GetScore(price, null);

            GameSceneObjectsReferences.AmmoTabCustomization.InstantiateAmmoTypeButton(newWeapon, 1);

            GameSceneObjectsReferences.AmmoTabButtonNotification.NewAvailableWeaponNotificationHolder();
        }
    }

    private void UpdateAmmoFromDropBoxPanel(DropBoxItemType dropBoxItemType, object[] data)
    {
        bool hasLocalPlayerPickAmmo = dropBoxItemType == DropBoxItemType.Ammo && _tankController.BasePlayer != null;

        if (hasLocalPlayerPickAmmo)
        {
            int price = (int)data[0];

            for (int i = 0; i < _defaultWeaponsLength; i++)
                _weaponsBulletsCount[i] += UnityEngine.Random.Range(0, 10);

            _scoreController.GetScore(price, null);

            UpdateDisplayedWeapon(_shootController.ActiveAmmoIndex);
        }
    }

    public void PlayProjectileChangeSoundEffectAfterDelay(int clipIndex, float delay = 0.2f) => StartCoroutine(PlayProjectileChangeSoundEffectAfterDelayCoroutine(clipIndex, delay));

    private IEnumerator PlayProjectileChangeSoundEffectAfterDelayCoroutine(int clipIndex, float delay = 0.2f)
    {
        yield return new WaitForSeconds(delay);

        SecondarySoundController.PlaySound(12, clipIndex);
    }
}
