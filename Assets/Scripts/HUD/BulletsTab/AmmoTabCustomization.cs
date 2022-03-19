using System;


public class AmmoTabCustomization : BaseAmmoTabCustomization<AmmoTypeButton, AmmoParameters>
{
    public event Action<Action<int>> OnInstantiateAmmoTypeButton;
    public event Action<AmmoTypeButton> OnSendActiveAmmoToPlayer;


    protected override void OnDisable()
    {
        base.OnDisable();

        UnSubscribeFromAmmoTypeButtonEvents();
    }

    public void CallOnInstantiateAmmoTypeButton()
    {
        OnInstantiateAmmoTypeButton?.Invoke(delegate (int index) { InstantiateAmmoTypeButton(index); });
    }

    public void InstantiateAmmoTypeButton(int index)
    {
        AmmoTypeButton button = Instantiate(_buttonPrefab, _container.transform);
        if (index == 0) button._properties.ButtonSprite = _clicked;
        Conditions<int>.Compare(index, _parameters.Length, null, null, () => SetAmmoTypeButtonProperties(button, index));
        CacheAmmoTypeButtons(button);
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

    private void SetAmmoTypeButtonProperties(AmmoTypeButton button, int index)
    {
        button._properties.Index = index;
        button._properties.UnlockPoints = _parameters[index]._unlockPoints;
        button._properties.IconSprite = _parameters[index]._icon;
        button._ammoStars.OnSetStars(_parameters[index]._ammoTypeStars);
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
