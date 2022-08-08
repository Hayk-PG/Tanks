using System;
using UnityEngine;

public class SupportsTabCustomization : BaseAmmoTabCustomization<AmmoTypeButton>
{
    [Header("Bombers")]
    [SerializeField] private BomberProperties[] _bombers;

    public Action OnCallBomber { get; set; }



    private void Start()
    {
        InstantiateBombers();
    }

    private void InstantiateBombers()
    {
        foreach (var bomber in _bombers)
        {
            AmmoTypeButton button = Instantiate(_buttonPrefab, _container.transform);
            GlobalFunctions.CanvasGroupActivity(button._properties.CanvasGroupSupportTab, true);

            AssignProperties(button, new BaseAmmoTabCustomization<AmmoTypeButton>.Properties
            {
                _buttonType = bomber._buttonType,
                _index = bomber._index,
                _value = bomber._value,
                _requiredScoreAmmount = bomber._requiredScoreAmmount,
                _damageValue = bomber._damageValue,
                _minutes = bomber._minutes,
                _seconds = bomber._seconds,
                _massWalue = bomber._radius,
                _weaponType = bomber._weaponType,
                _supportType = bomber._supportType,
                _icon = bomber._icon
            }, bomber._ammoTypeStars);

            _instantiatedButtons.Add(button);
            button.OnClickSupportTypeButton += OnClickSupportTypeButton;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        GlobalFunctions.Loop<AmmoTypeButton>.Foreach(_instantiatedButtons, OnLoop => 
        {
            OnLoop.OnClickSupportTypeButton -= OnClickSupportTypeButton;
        });
    }

    private void OnClickSupportTypeButton(AmmoTypeButton supportTypeButton)
    {
        if (supportTypeButton._properties.SupportOrPropsType == Names.AirSupport)
            OnCallBomber?.Invoke();

        OnAmmoTypeController?.Invoke();
    }

    protected override void DisplayPointsToUnlock(int index, int playerPoints, int value)
    {
        _instantiatedButtons[index].DisplayScoresToUnlock(playerPoints, 0);
    }
}
