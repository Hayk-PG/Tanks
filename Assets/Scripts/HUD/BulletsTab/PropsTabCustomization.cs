using System;
using System.Collections.Generic;
using UnityEngine;

public class PropsTabCustomization : BaseAmmoTabCustomization<PropsTypeButton>
{
    [Header("Props")]
    [SerializeField] private PropsProperties[] _props;

    public Action OnInstantiateSandbags { get; set; }
    public Action OnActivateShields { get; set; }
    public Action OnArtillery { get; set; }



    private void Start()
    {
        InstantiateProps();
    }

    protected override void OnDisable()
    {
        GlobalFunctions.Loop<PropsTypeButton>.Foreach(_instantiatedButtons, button => 
        {
            button.OnClickPropsTypeButton -= OnClickPropsTypeButton;
        });
    }

    private void InstantiateProps()
    {
        foreach (var prop in _props)
        {
            PropsTypeButton button = Instantiate(_buttonPrefab, _container.transform);
            AssignProperties(button, prop._title, prop._ammoIndex, prop._value, prop._unlockPoints, prop._icon, prop._ammoTypeStars);
            _instantiatedButtons.Add(button);
            button.OnClickPropsTypeButton += OnClickPropsTypeButton;
        }
    }

    private void OnClickPropsTypeButton(PropsTypeButton button)
    {
        if (button._properties.Title == "Sandbags")
            OnInstantiateSandbags?.Invoke();

        if (button._properties.Title == "Shields")
            OnActivateShields?.Invoke();

        if (button._properties.Title == "Artillery")
            OnArtillery?.Invoke();

        OnAmmoTypeController?.Invoke();
    }

    protected override void DisplayPointsToUnlock(int index, int playerPoints, int value)
    {
        _instantiatedButtons[index].DisplayPointsToUnlock(playerPoints, 0);
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
