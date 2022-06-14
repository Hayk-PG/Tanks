using System;
using UnityEngine;

public class SupportsTabCustomization : BaseAmmoTabCustomization<SupportsTypeButton>
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
            SupportsTypeButton button = Instantiate(_buttonPrefab, _container.transform);
            AssignProperties(button, bomber._title, bomber._ammoIndex, bomber._value, bomber._unlockPoints, bomber._icon, bomber._ammoTypeStars);
            _instantiatedButtons.Add(button);
            button.OnClickSupportTypeButton += OnClickSupportTypeButton;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        GlobalFunctions.Loop<SupportsTypeButton>.Foreach(_instantiatedButtons, OnLoop => 
        {
            OnLoop.OnClickSupportTypeButton -= OnClickSupportTypeButton;
        });
    }

    private void OnClickSupportTypeButton(SupportsTypeButton supportTypeButton)
    {
        if (supportTypeButton._properties.Title == "Bomber") OnCallBomber?.Invoke();

        OnAmmoTypeController?.Invoke();
    }

    protected override void DisplayPointsToUnlock(int index, int playerPoints, int value)
    {
        _instantiatedButtons[index].DisplayPointsToUnlock(playerPoints, 0);
    }
}
