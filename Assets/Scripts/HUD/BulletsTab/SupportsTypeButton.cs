using System;

public class SupportsTypeButton : AmmoTypeButton
{
    public event Action<SupportsTypeButton> OnClickSupportTypeButton;

    public override void OnClickButton()
    {
        OnClickSupportTypeButton?.Invoke(this);
    }
}
