using System;

public class AmmoTabCustomization : BaseAmmoTabCustomization<AmmoTypeButton>
{
    public Action<WeaponProperties, int> OnUpdateDisplayedWeapon { get; set; }
    public AmmoTypeButton DefaultAmmoTypeButton { get; set; }


    protected override void OnDisable()
    {
        base.OnDisable();

        UnSubscribeFromAmmoTypeButtonEvents();
    }

    public void InstantiatedButton(out AmmoTypeButton button) => button = Instantiate(_buttonPrefab, _container.transform);
    public void UnhideInstantiatedButtonWeaponsTab(AmmoTypeButton button) => GlobalFunctions.CanvasGroupActivity(button._properties.CanvasGroupWeaponsTab, true);

    public void AssignProperties(AmmoTypeButton button, WeaponProperties weaponProperty)
    {
        AssignProperties(button, new BaseAmmoTabCustomization<AmmoTypeButton>.Properties
        {
            _buttonType = weaponProperty._buttonType,
            _index = weaponProperty._index,
            _value = weaponProperty._value,
            _requiredScoreAmmount = weaponProperty._requiredScoreAmmount,
            _minutes = weaponProperty._minutes,
            _seconds = weaponProperty._seconds,
            _damageValue = weaponProperty._damageValue,
            _bulletMaxForce = weaponProperty._bulletMaxForce,
            _bulletForceMaxSpeed = weaponProperty._bulletForceMaxSpeed,
            _radius = weaponProperty._radius,
            _weaponType = weaponProperty._weaponType,
            _supportType = weaponProperty._supportType,
            _icon = weaponProperty._icon
        }, weaponProperty._ammoTypeStars);
    }

    public void InstantiateAmmoTypeButton(WeaponProperties weaponProperty, int loopIndex)
    {
        InstantiatedButton(out AmmoTypeButton button);
        UnhideInstantiatedButtonWeaponsTab(button);
        AssignProperties(button, weaponProperty);
        OnSendWeaponPointsToUnlock?.Invoke(button);
        Conditions<bool>.Compare(loopIndex == 0, () => SetDefaultAmmo(button), null);
        CacheAmmoTypeButtons(button);
    }

    public void SetDefaultAmmo(AmmoTypeButton button)
    {
        if(DefaultAmmoTypeButton == null)
        {
            DefaultAmmoTypeButton = button;
        }

        OnSelectedAmmoTypeButton(DefaultAmmoTypeButton);
        OnPlayerWeaponChanged?.Invoke(DefaultAmmoTypeButton);
    }

    private void SubscribeToAmmoTypeButtonEvents(AmmoTypeButton button)
    {
        button.OnClickAmmoTypeButton += OnClickAmmoTypeButton;
    }

    private void UnSubscribeFromAmmoTypeButtonEvents()
    {
        if(_instantiatedButtons != null)
        {
            foreach (var button in _instantiatedButtons)
            {
                button.OnClickAmmoTypeButton -= OnClickAmmoTypeButton;
            }
        }
    }

    public void CacheAmmoTypeButtons(AmmoTypeButton button)
    {
        _instantiatedButtons.Add(button);
        SubscribeToAmmoTypeButtonEvents(button);
    }
    
    private void OnClickAmmoTypeButton(AmmoTypeButton ammoTypeButton)
    {
        OnSelectedAmmoTypeButton(ammoTypeButton);
        OnPlayerWeaponChanged?.Invoke(ammoTypeButton);
        OnAmmoTypeController?.Invoke();
    }

    private void OnSelectedAmmoTypeButton(AmmoTypeButton ammoTypeButton)
    {
        GlobalFunctions.Loop<AmmoTypeButton>.Foreach(_instantiatedButtons,
            OnAmmoTypeButtons =>
            {
                OnAmmoTypeButtons._properties.IsSelected = false;
            });

        ammoTypeButton._properties.IsSelected = _clicked;
        ammoTypeButton._properties.IsSelected = true;
    }

    protected override void DisplayPointsToUnlock(int index, int playerPoints, int value)
    {
        _instantiatedButtons[index].DisplayScoresToUnlock(playerPoints, value);
    }
}
