using System;
using UnityEngine;

public class PropsTabCustomization : BaseAmmoTabCustomization<PropsTypeButton>
{
    [Header("Props")]
    [SerializeField] private PropsProperties[] _props;

    public Action OnInstantiateSandbags { get; set; }



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

        OnAmmoTypeController?.Invoke();
    }

    protected override void DisplayPointsToUnlock(int index, int playerPoints, int value)
    {
        _instantiatedButtons[index].DisplayPointsToUnlock(playerPoints, 0);
    }
}
