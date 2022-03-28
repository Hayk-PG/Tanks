using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseAmmoTabCustomization<T, T1> : MonoBehaviour, IGetPointsAndAmmoDataFromPlayer
{
    public CanvasGroup _container;

    protected AmmoTypeController _ammoTypeController;

    [SerializeField]
    protected T _buttonPrefab;

    protected List<T> _instantiatedButtons = new List<T>();

    [SerializeField]
    protected Sprite _clicked, _released;

    [SerializeField]
    protected T1[] _parameters;

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

    protected virtual void OnInformAboutTabActivityToTabsCustomization(bool isOpen)
    {
        if (isOpen) OnGetPointsAndAmmoDataFromPlayer?.Invoke(GetPointsAndAmmoDataFromPlayer);
    }

    public virtual void GetPointsAndAmmoDataFromPlayer(int playerPoints, List<int> bulletsCount)
    {
        
    }
}
