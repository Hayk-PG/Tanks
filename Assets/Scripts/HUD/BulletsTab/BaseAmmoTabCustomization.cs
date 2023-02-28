using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseAmmoTabCustomization<T> : MonoBehaviour, IGetPointsAndAmmoDataFromPlayer where T : AmmoTypeButton
{
    [SerializeField] 
    protected T _buttonPrefab;
    [SerializeField] 
    protected Sprite _clicked, _released;
  
    protected AmmoTypeController _ammoTypeController;
    public CanvasGroup _container;
    public List<T> _instantiatedButtons = new List<T>();

    public T ButtonPrefab { get => _buttonPrefab; }
    public Transform Container { get => _container.transform; }

    
    public Action<T> OnPlayerWeaponChanged { get; set; }
    public Action<T> OnSupportOrPropsChanged { get; set; }
    public Action OnAmmoTypeController { get; set; }
    public Action<Action<int, List<int>>> OnGetPointsAndAmmoDataFromPlayer { get; set; }
    public Action<AmmoTypeButton> OnSendWeaponPointsToUnlock { get; set; }

    public struct Properties
    {
        public ButtonType _buttonType;      
        public int? _index;
        public int? _value;
        public int? _requiredScoreAmmount;
        public int? _damageValue;
        public int? _minutes;
        public int? _seconds;
        public float? _bulletMaxForce;
        public float? _bulletForceMaxSpeed;
        public float? _radius;
        public string _weaponType;
        public string _supportType;       
        public Sprite _icon;

        public Properties(ButtonType buttonType, int? index, int? value, int? requiredScoreAmmount, int? damageValue, int? minutes, int? seconds, float? bulletMaxForce, float? bulletForceMaxSpeed, float? radius, string weaponType, string supportType, Sprite icon)
        {
            _buttonType = buttonType;
            _index = index;
            _value = value;
            _requiredScoreAmmount = requiredScoreAmmount;
            _damageValue = damageValue;
            _minutes = minutes;
            _seconds = seconds;
            _bulletMaxForce = bulletMaxForce;
            _bulletForceMaxSpeed = bulletForceMaxSpeed;
            _radius = radius;
            _weaponType = weaponType;
            _supportType = supportType;
            _icon = icon;
        }
    }

    
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

    public virtual void AssignProperties(AmmoTypeButton button, Properties properties, AmmoTypeStars stars)
    {
        button._properties._buttonType = properties._buttonType;

        if (properties._icon != null)
            button._properties.Image = properties._icon;

        if (properties._weaponType != null)
            button._properties.Name = properties._weaponType;

        if (properties._index.HasValue) 
            button._properties.Index = properties._index.Value;

        if (properties._value.HasValue) 
            button._properties.Quantity = properties._value.Value;

        if (properties._requiredScoreAmmount.HasValue) 
            button._properties.Price = properties._requiredScoreAmmount.Value;

        if (properties._bulletMaxForce.HasValue)
            button._properties.BulletMaxForce = properties._bulletMaxForce.Value;

        if (properties._bulletForceMaxSpeed.HasValue)
            button._properties.BulletForceMaxSpeed = properties._bulletForceMaxSpeed.Value;

        button._ammoStars.OnSetStars(stars);
    }

    protected virtual void OnInformAboutTabActivityToTabsCustomization(bool isOpen)
    {
        if (isOpen) 
            OnGetPointsAndAmmoDataFromPlayer?.Invoke(GetPointsAndAmmoDataFromPlayer);
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
