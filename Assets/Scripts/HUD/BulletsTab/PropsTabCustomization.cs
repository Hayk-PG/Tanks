using System;
using System.Collections.Generic;
using UnityEngine;

public class PropsTabCustomization : BaseAmmoTabCustomization<AmmoTypeButton>
{
    [Header("Props")]
    [SerializeField] private PropsProperties[] _props;

    public AmmoTypeButton[] InstantiatedTypeButtons
    {
        get => _instantiatedButtons.ToArray();
    }

    public Action OnInstantiateSandbags { get; set; }
    public Action OnInstantiateMetalCube { get; set; }
    public Action OnChangeToMetalGround { get; set; }
    public Action OnActivateShields { get; set; }
    public Action OnArtillery { get; set; }
    public Action OnModifyGround { get; set; }
    public Action OnSkipTurn { get; set; }
 

    private void Start()
    {
        InstantiateProps();
    }

    protected override void OnDisable()
    {
        GlobalFunctions.Loop<AmmoTypeButton>.Foreach(_instantiatedButtons, button => 
        {
            button.OnClickPropsTypeButton -= OnClickPropsTypeButton;
        });
    }

    private void InstantiateProps()
    {
        foreach (var prop in _props)
        {
            AmmoTypeButton button = Instantiate(_buttonPrefab, _container.transform);
            GlobalFunctions.CanvasGroupActivity(button._properties.CanvasGroupSupportTab, true);

            AssignProperties(button, new BaseAmmoTabCustomization<AmmoTypeButton>.Properties
            {
                _buttonType = prop._buttonType,
                _index = prop._index,
                _value = prop._value,
                _requiredScoreAmmount = prop._requiredScoreAmmount,
                _damageValue = prop._damageValue,
                _minutes = prop._minutes,
                _seconds = prop._seconds,
                _radius = prop._radius,
                _weaponType = prop._weaponType,
                _supportType = prop._supportType,
                _icon = prop._icon
            }, prop._ammoTypeStars);

            _instantiatedButtons.Add(button);
            button.OnClickPropsTypeButton += OnClickPropsTypeButton;
        }
    }

    private void OnClickPropsTypeButton(AmmoTypeButton propsTypeButton)
    {
        if (propsTypeButton._properties.SupportOrPropsType == Names.Sandbags)
            OnInstantiateSandbags?.Invoke();

        if (propsTypeButton._properties.SupportOrPropsType == Names.MetalCube)
            OnInstantiateMetalCube?.Invoke();

        if (propsTypeButton._properties.SupportOrPropsType == Names.MetalGround)
            OnChangeToMetalGround?.Invoke();

        if (propsTypeButton._properties.SupportOrPropsType == Names.Shield)
            OnActivateShields?.Invoke();

        if (propsTypeButton._properties.SupportOrPropsType == Names.LightMortarSupport)
            OnArtillery?.Invoke();

        if (propsTypeButton._properties.SupportOrPropsType == Names.ModifyGround)
            OnModifyGround?.Invoke();

        if (propsTypeButton._properties.SupportOrPropsType == Names.SkipTurn)
            OnSkipTurn?.Invoke();

        OnAmmoTypeController?.Invoke();
    }

    protected override void DisplayPointsToUnlock(int index, int playerPoints, int value)
    {
        _instantiatedButtons[index].DisplayScoresToUnlock(playerPoints, 0);
    }

    public override void GetPointsAndAmmoDataFromPlayer(int playerPoints, List<int> bulletsCount)
    {
        if (_instantiatedButtons != null)
        {
            for (int i = 0; i < _instantiatedButtons.Count; i++)
            {
                DisplayPointsToUnlock(i, playerPoints, 0);
            }
        }
    }
}
