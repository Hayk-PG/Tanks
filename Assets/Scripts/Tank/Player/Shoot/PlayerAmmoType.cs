using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class PlayerAmmoType : MonoBehaviour
{
    private TankController _tankController;

    private ShootController _shootController;

    private ScoreController _scoreController;

    
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
    }

    private void OnEnable()
    {
        _tankController.OnInitialize += OnInitialize;

        GameSceneObjectsReferences.DropBoxSelectionPanelAmmo.onAmmo += UpdateAmmoFromDropBoxPanel;

        GlobalFunctions.Loop<DropBoxSelectionPanelRocket>.Foreach(GameSceneObjectsReferences.DropBoxSelectionPanelRockets, rocket =>
        {
            rocket.onRocket += delegate (WeaponProperties weaponProperties, int id, int price) { AddWeapon(weaponProperties, price); };
        });
    }
   
    private void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;

        GameSceneObjectsReferences.AmmoTabCustomization.OnPlayerWeaponChanged -= OnPlayerWeaponChanged;

        GameSceneObjectsReferences.DropBoxSelectionPanelAmmo.onAmmo -= UpdateAmmoFromDropBoxPanel;

        GlobalFunctions.Loop<DropBoxSelectionPanelRocket>.Foreach(GameSceneObjectsReferences.DropBoxSelectionPanelRockets, rocket =>
        {
            rocket.onRocket -= delegate (WeaponProperties weaponProperties, int id, int price) { AddWeapon(weaponProperties, price); };
        });
    }

    private void OnInitialize()
    {
        GameSceneObjectsReferences.AmmoTabCustomization.OnPlayerWeaponChanged += OnPlayerWeaponChanged;

        InitializeBulletsCountList();

        InstantiateAmmoTypeButton();       
    }

    private void InitializeBulletsCountList()
    {
        GlobalFunctions.Loop<WeaponProperties>.Foreach(_weapons, weapon => { _weaponsBulletsCount.Add(weapon._value); });
    }

    private void InstantiateAmmoTypeButton()
    {
        if(_weapons != null)
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
        if (ammoTypeButton._properties.Index < _weapons.Length)
            _shootController.ActiveAmmoIndex = ammoTypeButton._properties.Index;
        else
            _shootController.ActiveAmmoIndex = _weapons.Length - 1;

        GetMoreBullets(ammoTypeButton);

        UpdateDisplayedWeapon(_shootController.ActiveAmmoIndex);

        SetBulletSpecs(ammoTypeButton);

        OnPlayerSelectedRocket(ammoTypeButton);

        OnWeaponChanged?.Invoke(_shootController.ActiveAmmoIndex);
    }

    private void OnPlayerSelectedRocket(AmmoTypeButton ammoTypeButton)
    {
        if(ammoTypeButton._properties._buttonType == ButtonType.Rocket && ammoTypeButton._properties.Quantity > 0)
        {
            print("Rocket is selected: ");

            for (int i = 0; i < GameSceneObjectsReferences.DropBoxSelectionPanelRockets.Length; i++)
            {
                print(ammoTypeButton._properties.Index + ") " + "Searching the ID...: ");

                if (GameSceneObjectsReferences.DropBoxSelectionPanelRockets[i].Weapon == _weapons[ammoTypeButton._properties.Index])
                {
                    int id = GameSceneObjectsReferences.DropBoxSelectionPanelRockets[i].Id;

                    print("Found ID: " + id);

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

    private void RaiseOnRocketSelectedEvent(bool isSelected, int id = 0)
    {
        onRocketSelected?.Invoke(isSelected, id);
    }

    private void GetMoreBullets(AmmoTypeButton ammoTypeButton)
    {
        if (_weaponsBulletsCount[_shootController.ActiveAmmoIndex] <= 0)
            _weaponsBulletsCount[_shootController.ActiveAmmoIndex] += ammoTypeButton._properties.Quantity;
    }

    public void UpdateDisplayedWeapon(int index)
    {
        GameSceneObjectsReferences.AmmoTabCustomization.OnUpdateDisplayedWeapon?.Invoke(_weapons[index], _weaponsBulletsCount[index]); 
    }

    public void SetBulletSpecs(AmmoTypeButton ammoTypeButton)
    {
        _shootController._shoot._minForce = 3;
        _shootController._shoot._smoothTime = 1;
        _shootController._shoot._maxForce = ammoTypeButton._properties.BulletMaxForce;
        _shootController._shoot._maxSpeed = ammoTypeButton._properties.BulletForceMaxSpeed;
    }

    public void SwitchToDefaultWeapon(int index)
    {
        if (_weaponsBulletsCount[index] <= 0)
            GameSceneObjectsReferences.AmmoTabCustomization.SetDefaultAmmo(null);
    }

    public void AddWeapon(WeaponProperties weaponProperties, int price = 0)
    {
        if (_tankController.BasePlayer == null)
            return;

        WeaponProperties[] weaponsTempCollection = new WeaponProperties[_weapons.Length + 1];

        for (int i = 0; i < _weapons.Length; i++)
            weaponsTempCollection[i] = _weapons[i];

        weaponsTempCollection[weaponsTempCollection.Length - 1] = weaponProperties;

        _weapons = weaponsTempCollection;

        _weaponsBulletsCount.Add(weaponProperties._value);

        _scoreController.GetScore(price, null);

        GameSceneObjectsReferences.AmmoTabCustomization.InstantiateAmmoTypeButton(weaponProperties, 1);

        GameSceneObjectsReferences.AmmoTabButtonNotification.NewAvailableWeaponNotificationHolder();
    }

    private void UpdateAmmoFromDropBoxPanel(int price)
    {
        if (_tankController.BasePlayer != null)
        {
            for (int i = 0; i < _defaultWeaponsLength; i++)
                _weaponsBulletsCount[i] += UnityEngine.Random.Range(0, 10);

            _scoreController.GetScore(price, null);

            UpdateDisplayedWeapon(_shootController.ActiveAmmoIndex);
        }
    }
}
