using System;

public class DropBoxSelectionPanelShield : BaseDropBoxSelectionPanelElement
{
    public event Action onShield;

    protected override void Use()
    {
        onShield?.Invoke();

        CanUse = false;
    }
}
