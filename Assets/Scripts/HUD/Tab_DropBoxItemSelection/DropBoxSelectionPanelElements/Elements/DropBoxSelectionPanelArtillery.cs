using System;


public class DropBoxSelectionPanelArtillery : BaseDropBoxSelectionPanelElement
{
    public event Action<int, int> onArtillery;

    protected override void Use()
    {
        onArtillery?.Invoke(-_price, _quantity);

        CanUse = false;
    }
}
