using System;

public class DropBoxSelectionPanelAmmo : BaseDropBoxSelectionPanelElement
{
    public event Action onAmmo;

    protected override void Use()
    {
        onAmmo?.Invoke();
    }
}
