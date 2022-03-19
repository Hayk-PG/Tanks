using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseAmmoTabCustomization<T, T1> : MonoBehaviour, IGetPlayerPoints
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

    public Action OnAmmoTypeController { get; set; }
    public Action<Action<int>> OnGetPlayerPointsCount { get; set; }

    
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
        if (isOpen) OnGetPlayerPointsCount?.Invoke(GetPlayerPoints);
    }

    public virtual void GetPlayerPoints(int playerPoints)
    {
        
    }
}
