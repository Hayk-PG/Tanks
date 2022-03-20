using System;


public class AmmoTabCustomization : BaseAmmoTabCustomization<AmmoTypeButton, AmmoParameters>
{
    public event Action<AmmoTypeButton> OnSendActiveAmmoToPlayer;
    public Action<WeaponProperties, int> OnUpdateDisplayedWeapon { get; set; }


    protected override void OnDisable()
    {
        base.OnDisable();

        UnSubscribeFromAmmoTypeButtonEvents();
    }

    public void InstantiateAmmoTypeButton(WeaponProperties weaponProperty, int loopIndex)
    {
        AmmoTypeButton button = Instantiate(_buttonPrefab, _container.transform);

        button._properties.Index = weaponProperty._ammoIndex;
        button._properties.UnlockPoints = weaponProperty._unlockPoints;
        button._properties.IconSprite = weaponProperty._icon;
        button._ammoStars.OnSetStars(weaponProperty._ammoTypeStars);

        Conditions<bool>.Compare(loopIndex == 0, () => SetDefaultAmmo(button, weaponProperty), null);
        CacheAmmoTypeButtons(button);
    }

    private void SetDefaultAmmo(AmmoTypeButton button, WeaponProperties weaponProperty)
    {
        button._properties.ButtonSprite = _clicked;
        OnUpdateDisplayedWeapon?.Invoke(weaponProperty, weaponProperty._bulletsLeft);
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
        ChangeAmmoTypeButtonsSprite(ammoTypeButton);
        OnSendActiveAmmoToPlayer?.Invoke(ammoTypeButton);
        OnAmmoTypeController?.Invoke();
    }

    private void ChangeAmmoTypeButtonsSprite(AmmoTypeButton ammoTypeButton)
    {
        GlobalFunctions.Loop<AmmoTypeButton>.Foreach(_instantiatedButtons,
            OnAmmoTypeButtons =>
            {
                OnAmmoTypeButtons._properties.ButtonSprite = _released;
            });

        ammoTypeButton._properties.ButtonSprite = _clicked;
    }

    public override void GetPlayerPoints(int playerPoints)
    {
        if (_instantiatedButtons != null)
        {
            for (int i = 0; i < _instantiatedButtons.Count; i++)
            {
                _instantiatedButtons[i].DisplayPointsToUnlock(playerPoints);
            }
        }
    }
}
