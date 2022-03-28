using System;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTabButtonNotification : MonoBehaviour
{
    public List<int> _weaponsPointsToUnlock = new List<int>();

    private AmmoTabCustomization _ammoTabCustomization;
    private ScoreController _playerScoreController;
    private AmmoTypeController _ammoTypeController;

    [SerializeField]
    private GameObject _notificationsIcon;

    private bool _isReversed;
    private bool _isNewWeaponAvailable;
    private bool _isAmmoTabOpen;
    private int _weaponsPointsToUnlockDeleteRange;

    public Action OnNewAwailableWeaponNotification;


    private void Awake()
    {
        _ammoTabCustomization = FindObjectOfType<AmmoTabCustomization>();
        _ammoTypeController = FindObjectOfType<AmmoTypeController>();
    }

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

    private void CacheWeaponsPointsToUnlock(int pointsToUnlock)
    {
        if(pointsToUnlock > 0) _weaponsPointsToUnlock.Add(pointsToUnlock);
    }

    public void GetPlayerScoreAndSubscibeToEvent(ScoreController scoreController)
    {
        _playerScoreController = scoreController;
        _playerScoreController.OnPlayerGetsPoints += PlayerGetsPoints;
    }

    private void PlayerGetsPoints(int points)
    {
        _isNewWeaponAvailable = false;

        Conditions<List<int>>.CheckNull(_weaponsPointsToUnlock, null, () => CheckForNewAvailableWeapon(points));
    }

    private void CheckForNewAvailableWeapon(int points)
    {
        for (int i = _weaponsPointsToUnlock.Count - 1; i >= 0; i--)
        {
            if (points >= _weaponsPointsToUnlock[i])
            {
                _weaponsPointsToUnlockDeleteRange = i;
                _isNewWeaponAvailable = true;

                break;
            }
        }

        Conditions<bool>.Compare(_isNewWeaponAvailable, OnNewAvailableWeaponNotification, null);
    }

    private void OnNewAvailableWeaponNotification()
    {
        _weaponsPointsToUnlock.RemoveRange(0, _weaponsPointsToUnlockDeleteRange + 1);
        OnNotificationIcon(true && !_isAmmoTabOpen);
        OnNewAwailableWeaponNotification?.Invoke();
    }

    private void OnNotificationIcon(bool isActive)
    {
        _notificationsIcon.SetActive(isActive);
    }

    private void OnWeaponsTabActivity(bool isOpen)
    {
        _isAmmoTabOpen = isOpen;

        if (_isAmmoTabOpen) OnNotificationIcon(false);
    }
}
