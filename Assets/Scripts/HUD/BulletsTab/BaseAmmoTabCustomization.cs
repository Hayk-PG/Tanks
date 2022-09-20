using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseAmmoTabCustomization<T> : MonoBehaviour, IGetPointsAndAmmoDataFromPlayer where T : AmmoTypeButton
{
    [SerializeField] protected T _buttonPrefab;
    [SerializeField] protected Sprite _clicked, _released;

    public CanvasGroup _container;
    protected AmmoTypeController _ammoTypeController;
    protected List<T> _instantiatedButtons = new List<T>();
    
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

    protected virtual void AssignProperties(AmmoTypeButton button, Properties properties, AmmoTypeStars stars)
    {
        button._properties._buttonType = properties._buttonType;

        if (properties._index != null) button._properties.Index = (int)properties._index;
        if (properties._value != null) button._properties.Value = (int)properties._value;
        if (properties._requiredScoreAmmount != null) button._properties.RequiredScoreAmmount = (int)properties._requiredScoreAmmount;
        if (properties._damageValue != null) button._properties.DamageValue = (int)properties._damageValue;
        if (properties._minutes != null) button._properties.Minutes = (int)properties._minutes;
        if (properties._seconds != null) button._properties.Seconds = (int)properties._seconds;
        if (properties._bulletMaxForce != null) button._properties.BulletMaxForce = (float)properties._bulletMaxForce;
        if (properties._bulletForceMaxSpeed != null) button._properties.BulletForceMaxSpeed = (float)properties._bulletForceMaxSpeed;
        if (properties._radius != null) button._properties.Radius = (float)properties._radius;
        if (properties._weaponType != null) button._properties.WeaponType = properties._weaponType;
        if (properties._supportType != null) button._properties.SupportOrPropsType = properties._supportType;
        if (properties._icon != null) button._properties.Icon = properties._icon;

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

    protected virtual void SupportAndPropsTimer(T typeButton)
    {
        typeButton.StartTimerCoroutine();
    }
}
