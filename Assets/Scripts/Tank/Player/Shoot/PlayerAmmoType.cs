using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerAmmoType : MonoBehaviour
{
    private ShootController _shootController;
    private GameManager _gameManager;
    private AmmoTabCustomization _ammoTabCustomization;

    [Header("Scriptable objects")]
    public WeaponProperties[] _weapons;

    [Header("Cached bullets count from scriptable objects")]
    public List<int> _weaponsBulletsCount = new List<int>();



    private void Awake()
    {
        _shootController = Get<ShootController>.From(gameObject);
        _gameManager = FindObjectOfType<GameManager>();
        _ammoTabCustomization = FindObjectOfType<AmmoTabCustomization>();

        CacheBulletsCount();
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStart;
        _ammoTabCustomization.OnSendActiveAmmoToPlayer += OnGetActiveAmmoType;
    }
   
    private void OnDisable()
    {
        _gameManager.OnGameStarted -= OnGameStart;
        _ammoTabCustomization.OnSendActiveAmmoToPlayer -= OnGetActiveAmmoType;
    }

    private void CacheBulletsCount()
    {
        if (_weapons != null)
        {
            for (int i = 0; i < _weapons.Length; i++)
            {
                _weaponsBulletsCount.Add(_weapons[i]._bulletsLeft);
            }
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

    private void OnGetActiveAmmoType(AmmoTypeButton ammoTypeButton)
    {
        _shootController.ActiveAmmoIndex = ammoTypeButton._properties.Index < _weapons.Length ?
                                           ammoTypeButton._properties.Index : _weapons.Length - 1;

        UpdateDisplayedWeapon(_shootController.ActiveAmmoIndex);
    }

    public void UpdateDisplayedWeapon(int index)
    {
        _ammoTabCustomization.OnUpdateDisplayedWeapon?.Invoke(_weapons[index], _weaponsBulletsCount[index]);
    }
}
