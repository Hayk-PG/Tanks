using System;
using System.Collections.Generic;

public class AmmoTabCustomization : BaseAmmoTabCustomization<AmmoTypeButton, AmmoParameters>
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

        button._properties.Index = weaponProperty._ammoIndex;
        button._properties.Value = weaponProperty._value;
        button._properties.UnlockPoints = weaponProperty._unlockPoints;
        button._properties.IconSprite = weaponProperty._icon;
        button._ammoStars.OnSetStars(weaponProperty._ammoTypeStars);

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

    public override void GetPointsAndAmmoDataFromPlayer(int playerPoints, List<int> bulletsCount)
    {

        if (_instantiatedButtons != null)
        {
            for (int i = 0; i < _instantiatedButtons.Count; i++)
            {
                _instantiatedButtons[i].DisplayPointsToUnlock(playerPoints, bulletsCount[i]);
            }
        }
    }
}
