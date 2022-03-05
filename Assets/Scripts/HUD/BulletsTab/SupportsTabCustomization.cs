using System;

public class SupportsTabCustomization : BaseAmmoTabCustomization<SupportsTypeButton, SupportParameters>
{
    private const string Bomber = "Bomber";

    public event Action OnCallBomber;

    private void Start()
    {
        for (int i = 0; i < _parameters.Length; i++)
        {
            SupportsTypeButton button = Instantiate(_buttonPrefab, _container.transform);
            button._properties.Index = i;
            button._properties.Title = i == 0 ? Bomber : "";
            button._properties.IconSprite = _parameters[i]._icon;
            button._ammoStars.OnSetStars(_parameters[i]._ammoTypeStars);
            button.OnClickSupportTypeButton += OnClickSupportTypeButton;
            _instantiatedButtons.Add(button);
        }
    }

    private void OnDisable()
    {
        GlobalFunctions.Loop<SupportsTypeButton>.Foreach(_instantiatedButtons, OnLoop => 
        {
            OnLoop.OnClickSupportTypeButton -= OnClickSupportTypeButton;
        });
    }

    private void OnClickSupportTypeButton(SupportsTypeButton supportTypeButton)
    {
        if (supportTypeButton._properties.Title == Bomber) OnCallBomber?.Invoke();

        OnAmmoTypeController?.Invoke();
    }
}
