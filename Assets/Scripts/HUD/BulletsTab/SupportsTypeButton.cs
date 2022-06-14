using System;

public class SupportsTypeButton : AmmoTypeButton
{
    public Action<SupportsTypeButton> OnClickSupportTypeButton { get; set; }

    public override void OnClickButton()
    {
        OnClickSupportTypeButton?.Invoke(this);
    }
}
