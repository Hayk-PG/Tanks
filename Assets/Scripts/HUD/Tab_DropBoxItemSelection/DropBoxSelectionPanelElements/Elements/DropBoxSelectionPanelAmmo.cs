using System;

public class DropBoxSelectionPanelAmmo : BaseDropBoxSelectionPanelElement
{
    public event Action<int> onAmmo;

    protected override void Use()
    {
        onAmmo?.Invoke(NegativePrice);
    }
}
