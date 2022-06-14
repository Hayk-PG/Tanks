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

    public void InstantiateAmmoTypeButton(WeaponProperties weaponProperty, int loopIndex)
    {
        AmmoTypeButton button = Instantiate(_buttonPrefab, _container.transform);
        AssignProperties(button, weaponProperty._title, weaponProperty._ammoIndex, weaponProperty._value, weaponProperty._unlockPoints, weaponProperty._icon, weaponProperty._ammoTypeStars);
        OnSendWeaponPointsToUnlock?.Invoke(weaponProperty._unlockPoints);
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

    private void CacheAmmoTypeButtons(AmmoTypeButton button)
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
                OnAmmoTypeButtons._properties.ButtonSprite = _released;
                OnAmmoTypeButtons._properties.IsSelected = false;
            });

        ammoTypeButton._properties.ButtonSprite = _clicked;
        ammoTypeButton._properties.IsSelected = true;
    }

    protected override void DisplayPointsToUnlock(int index, int playerPoints, int value)
    {
        _instantiatedButtons[index].DisplayPointsToUnlock(playerPoints, value);
    }
}
