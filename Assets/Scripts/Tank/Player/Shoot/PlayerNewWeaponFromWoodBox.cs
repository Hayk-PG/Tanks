using UnityEngine;

public class PlayerNewWeaponFromWoodBox : MonoBehaviour
{
    private TankController _tankController;
    private PlayerAmmoType _playerAmmoType;
    private NewWeaponFromWoodBox _newWeaponFromWoodBox;

    private WeaponProperties[] _weapons;
    private WeaponProperties[] _updatedWeapons;


    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);
        _playerAmmoType = Get<PlayerAmmoType>.From(gameObject);
        _newWeaponFromWoodBox = FindObjectOfType<NewWeaponFromWoodBox>();
    }

    private void OnEnable()
    {
        _tankController.OnInitialize += OnInitialize;        
    }

    private void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        _newWeaponFromWoodBox.OnAddNewWeaponFromWeadBox -= OnAddNewWeaponFromWeadBox;
    }

    private void OnInitialize()
    {
        _newWeaponFromWoodBox.OnAddNewWeaponFromWeadBox += OnAddNewWeaponFromWeadBox;
    }

    private void PrepareUpdatedWeapons()
    {
        _weapons = _playerAmmoType._weapons;
        _updatedWeapons = new WeaponProperties[_weapons.Length + 1];
    }

    private void AddNewWeaponToList(WeaponProperties newWeaponProperty)
    {
        for (int i = 0; i < _updatedWeapons.Length; i++)
        {
            if (i < _weapons.Length)
                _updatedWeapons[i] = _weapons[i];
            else
                _updatedWeapons[i] = newWeaponProperty;
        }
    }

    private void PlayerWeaponsData(WeaponProperties newWeaponProperty)
    {
        _playerAmmoType._weapons = _updatedWeapons;
        _playerAmmoType._weaponsBulletsCount.Add(0);
    }

    private void OnAddNewWeaponFromWeadBox(WeaponProperties newWeaponProperty)
    {
        PrepareUpdatedWeapons();
        AddNewWeaponToList(newWeaponProperty);
        PlayerWeaponsData(newWeaponProperty);
    }
}
