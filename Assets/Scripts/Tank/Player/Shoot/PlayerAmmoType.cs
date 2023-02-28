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

    private WeaponProperties[] _tempWeaponsIncludedNewFromWoodBox;

    [Header("Cached bullets count from scriptable objects")]

    public List<int> _weaponsBulletsCount;

    //Serialized
    public int[] WeaponsBulletsCount
    {
        get => _weaponsBulletsCount.ToArray();
        set => _weaponsBulletsCount = value.ToList();
    }

    internal Action<int> OnWeaponChanged { get; set; }





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
    }
   
    private void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;

        GameSceneObjectsReferences.AmmoTabCustomization.OnPlayerWeaponChanged -= OnPlayerWeaponChanged;

        GameSceneObjectsReferences.DropBoxSelectionPanelAmmo.onAmmo -= UpdateAmmoFromDropBoxPanel;
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
                GameSceneObjectsReferences.AmmoTabCustomization.InstantiateAmmoTypeButton(_weapons[i], i);
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

        OnWeaponChanged?.Invoke(_shootController.ActiveAmmoIndex);
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

    private void UpdateAmmoFromDropBoxPanel(int price)
    {
        if (_tankController.BasePlayer != null)
        {
            int active = _shootController.ActiveAmmoIndex;

            int bulletsCount = _weapons[active]._value / 4 > 0 ? _weapons[active]._value / 4 : 1;

            _weaponsBulletsCount[_shootController.ActiveAmmoIndex] += bulletsCount;

            _scoreController.GetScore(price, null);

            UpdateDisplayedWeapon(_shootController.ActiveAmmoIndex);
        }
    }

    private void OnAddNewWeaponFromWoodBox(WeaponProperties newWeaponProperty)
    {
        _tempWeaponsIncludedNewFromWoodBox = new WeaponProperties[_weapons.Length + 1];

        for (int i = 0; i < _weapons.Length; i++)
        {
            _tempWeaponsIncludedNewFromWoodBox[i] = _weapons[i];
        }

        _tempWeaponsIncludedNewFromWoodBox[_weapons.Length] = newWeaponProperty;

        _weapons = _tempWeaponsIncludedNewFromWoodBox;

        _weaponsBulletsCount.Add(0);
    }
}
