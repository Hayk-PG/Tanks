using System;

public class DropBoxSelectionPanelShield : BaseDropBoxSelectionPanelElement
{
    public event Action<int> onShield;

    protected override void Use()
    {
        onShield?.Invoke(NegativePrice);

        CanUse = false;
    }
}
