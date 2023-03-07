using System;
using System.Collections.Generic;
using UnityEngine;

//USED COMMENTS
public class AmmoTabButtonNotification : MonoBehaviour
{
    public List<AmmoTypeButton> _weapons = new List<AmmoTypeButton>();
    public List<AmmoTypeButton> _availableWeapons;

    [SerializeField]
    private AmmoTabCustomization _ammoTabCustomization;

    [SerializeField] [Space]
    private AmmoTypeController _ammoTypeController;

    [SerializeField] [Space]
    private GameObject _notificationsIcon;

    private ScoreController _playerScoreController;
    
    private bool _isNewWeaponAvailable;
    private bool _isAmmoTabOpen;

    // NewAvailableWeaponNotificationHolder is used for public access to OnNewAvailableWeaponNotification
    public Action NewAvailableWeaponNotificationHolder => OnNewAvailableWeaponNotification;

    public Action OnNewAwailableWeaponNotification { get; set; }

    public Action<List<AmmoTypeButton>, bool> OnDisplayAvailableWeapons { get; set; }


    

    private void OnEnable()
    {
        _ammoTabCustomization.OnSendWeaponPointsToUnlock += CacheWeaponsPointsToUnlock;

        _ammoTypeController.OnInformAboutTabActivityToTabsCustomization += OnWeaponsTabActivity;
    }

    private void OnDisable()
    {
        _ammoTabCustomization.OnSendWeaponPointsToUnlock -= CacheWeaponsPointsToUnlock;

        _ammoTypeController.OnInformAboutTabActivityToTabsCustomization -= OnWeaponsTabActivity;

        if (_playerScoreController != null)
            _playerScoreController.OnPlayerGetsPoints -= PlayerGetsPoints;
    }

    public void CacheWeaponsPointsToUnlock(AmmoTypeButton ammoTypeButton)
    {
        if(ammoTypeButton._properties.Price > 0) 
            _weapons.Add(ammoTypeButton);
    }

    public void CallPlayerEvents(ScoreController scoreController)
    {
        _playerScoreController = scoreController;
        _playerScoreController.OnPlayerGetsPoints += PlayerGetsPoints;
    }

    public void PlayerGetsPoints(int points)
    {
        _isNewWeaponAvailable = false;

        Conditions<List<int>>.CheckNull(_weapons, null, () => CheckForNewAvailableWeapon(points));
    }

    private void CheckForNewAvailableWeapon(int points)
    {
        _availableWeapons = new List<AmmoTypeButton>();

        for (int i = _weapons.Count - 1; i >= 0; i--)
        {
            if (points >= _weapons[i]._properties.Price)
            {
                _availableWeapons.Add(_weapons[i]);

                _weapons.RemoveAt(i);

                _isNewWeaponAvailable = true;

                break;
            }
        }

        Conditions<bool>.Compare(_isNewWeaponAvailable, OnNewAvailableWeaponNotification, null);
    }

    // This method is held by NewAvailableWeaponNotificationHolder delegate for public use only

    private void OnNewAvailableWeaponNotification()
    {
        OnNotificationIcon(true && !_isAmmoTabOpen);

        OnNewAwailableWeaponNotification?.Invoke();
    }

    private void OnNotificationIcon(bool isActive)
    {
        _notificationsIcon.SetActive(isActive);

        OnDisplayAvailableWeapons?.Invoke(_availableWeapons, isActive);
    }

    private void OnWeaponsTabActivity(bool isOpen)
    {
        _isAmmoTabOpen = isOpen;

        if (_isAmmoTabOpen)
        {
            OnNotificationIcon(false);

            OnDisplayAvailableWeapons?.Invoke(_availableWeapons, false);
        }
    }
}
