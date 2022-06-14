using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAmmoTabCustomization<T> : MonoBehaviour, IGetPointsAndAmmoDataFromPlayer where T : AmmoTypeButton
{
    public CanvasGroup _container;
    protected AmmoTypeController _ammoTypeController;
    [SerializeField] protected T _buttonPrefab;
    protected List<T> _instantiatedButtons = new List<T>();
    [SerializeField]
    protected Sprite _clicked, _released;
    public Action<T> OnPlayerWeaponChanged { get; set; }
    public Action OnAmmoTypeController { get; set; }
    public Action<Action<int, List<int>>> OnGetPointsAndAmmoDataFromPlayer { get; set; }
    public Action<int> OnSendWeaponPointsToUnlock { get; set; }

    
    protected virtual void Awake()
    {
        _ammoTypeController = Get<AmmoTypeController>.From(gameObject);
    }

    protected virtual void OnEnable()
    {
        _ammoTypeController.OnInformAboutTabActivityToTabsCustomization += OnInformAboutTabActivityToTabsCustomization;
    }

    protected virtual void OnDisable()
    {
        _ammoTypeController.OnInformAboutTabActivityToTabsCustomization -= OnInformAboutTabActivityToTabsCustomization;
    }

    protected virtual void AssignProperties(AmmoTypeButton button, string title, int index, int value, int unlockPoints, Sprite icon, AmmoTypeStars stars)
    {
        button._properties.Title = title;
        button._properties.Index = index;
        button._properties.Value = value;
        button._properties.UnlockPoints = unlockPoints;
        button._properties.IconSprite = icon;
        button._ammoStars.OnSetStars(stars);
    }

    protected virtual void OnInformAboutTabActivityToTabsCustomization(bool isOpen)
    {
        if (isOpen) OnGetPointsAndAmmoDataFromPlayer?.Invoke(GetPointsAndAmmoDataFromPlayer);
    }

    public virtual void GetPointsAndAmmoDataFromPlayer(int playerPoints, List<int> bulletsCount)
    {
        if (_instantiatedButtons != null)
        {
            for (int i = 0; i < _instantiatedButtons.Count; i++)
            {
                DisplayPointsToUnlock(i, playerPoints, bulletsCount[i]);
            }
        }
    }

    protected abstract void DisplayPointsToUnlock(int index, int playerPoints, int value);
}
