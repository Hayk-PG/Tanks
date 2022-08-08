using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerAmmoType : MonoBehaviour
{
    private TankController _tankController;
    private ShootController _shootController;
    private AmmoTabCustomization _ammoTabCustomization;

    [Header("Scriptable objects")]
    public WeaponProperties[] _weapons;

    [Header("Cached bullets count from scriptable objects")]
    public List<int> _weaponsBulletsCount;

    //Serialized
    public int[] WeaponsBulletsCount
    {
        get => _weaponsBulletsCount.ToArray();
        set => _weaponsBulletsCount = value.ToList();
    }

    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);
        _shootController = Get<ShootController>.From(gameObject);
        _ammoTabCustomization = FindObjectOfType<AmmoTabCustomization>();
    }

    private void OnEnable()
    {
        _tankController.OnInitialize += OnInitialize;
    }
   
    private void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        _ammoTabCustomization.OnPlayerWeaponChanged -= OnPlayerWeaponChanged;
    }

    private void OnInitialize()
    {
        _ammoTabCustomization.OnPlayerWeaponChanged += OnPlayerWeaponChanged;
        InitializeBulletsCountList();
        InstantiateAmmoTypeButton();       
    }

    private void InitializeBulletsCountList()
    {
        GlobalFunctions.Loop<WeaponProperties>.Foreach(_weapons, weapon => { _weaponsBulletsCount.Add(0); });
    }

    private void InstantiateAmmoTypeButton()
    {
        if(_weapons != null)
        {
            for (int i = 0; i < _weapons.Length; i++)
            {
                _ammoTabCustomization.InstantiateAmmoTypeButton(_weapons[i], i);
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
    }

    private void GetMoreBullets(AmmoTypeButton ammoTypeButton)
    {
        if (_weaponsBulletsCount[_shootController.ActiveAmmoIndex] <= 0)
            _weaponsBulletsCount[_shootController.ActiveAmmoIndex] += ammoTypeButton._properties.Value;
    }

    public void UpdateDisplayedWeapon(int index)
    {
        _ammoTabCustomization.OnUpdateDisplayedWeapon?.Invoke(_weapons[index], _weaponsBulletsCount[index]); 
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
            _ammoTabCustomization.SetDefaultAmmo(null);
    }

    public void GetMoreBulletsFromWoodBox(out bool isDone, out string text)
    {
        isDone = false;
        text = "";

        if (_tankController.BasePlayer != null)
        {
            int active = _shootController.ActiveAmmoIndex;
            int bulletsCount = _weapons[active]._value / 4 > 0 ? _weapons[active]._value / 4 : 1;
            _weaponsBulletsCount[_shootController.ActiveAmmoIndex] += bulletsCount;
            UpdateDisplayedWeapon(_shootController.ActiveAmmoIndex);
            isDone = true;
            text = "+" + bulletsCount;
        }
    }
}
