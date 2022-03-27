using UnityEngine;
using System.Collections.Generic;

public class PlayerAmmoType : MonoBehaviour
{
    private ShootController _shootController;
    private GameManager _gameManager;
    private AmmoTabCustomization _ammoTabCustomization;

    [Header("Scriptable objects")]
    public WeaponProperties[] _weapons;

    [Header("Cached bullets count from scriptable objects")]
    public List<int> _weaponsBulletsCount;



    private void Awake()
    {
        _shootController = Get<ShootController>.From(gameObject);
        _gameManager = FindObjectOfType<GameManager>();
        _ammoTabCustomization = FindObjectOfType<AmmoTabCustomization>();

        InitializeBulletsCountList();
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStart;
        _ammoTabCustomization.OnPlayerWeaponChanged += OnPlayerWeaponChanged;
    }
   
    private void OnDisable()
    {
        _gameManager.OnGameStarted -= OnGameStart;
        _ammoTabCustomization.OnPlayerWeaponChanged -= OnPlayerWeaponChanged;
    }

    private void InitializeBulletsCountList()
    {
        foreach (var item in _weapons)
        {
            _weaponsBulletsCount.Add(0);
        }
    }

    private void OnGameStart()
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
        if(ammoTypeButton._properties.Index < _weapons.Length)
        {
            _shootController.ActiveAmmoIndex = ammoTypeButton._properties.Index;            
        }
        else
        {
            _shootController.ActiveAmmoIndex = _weapons.Length - 1;
        }

        GetMoreBullets(ammoTypeButton);
        UpdateDisplayedWeapon(_shootController.ActiveAmmoIndex);
    }

    private void GetMoreBullets(AmmoTypeButton ammoTypeButton)
    {
        if (_weaponsBulletsCount[_shootController.ActiveAmmoIndex] <= 0)
        {
            _weaponsBulletsCount[_shootController.ActiveAmmoIndex] += ammoTypeButton._properties.Value;
        }
    }

    public void UpdateDisplayedWeapon(int index)
    {
        _ammoTabCustomization.OnUpdateDisplayedWeapon?.Invoke(_weapons[index], _weaponsBulletsCount[index]); 
    }

    public void SwitchToDefaultWeapon(int index)
    {
        if (_weaponsBulletsCount[index] <= 0)
        {
            _ammoTabCustomization.SetDefaultAmmo(null);
        }
    }
}
