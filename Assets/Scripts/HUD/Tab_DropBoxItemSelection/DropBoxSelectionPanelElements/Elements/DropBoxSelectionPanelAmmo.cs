
public class DropBoxSelectionPanelAmmo : BaseDropBoxSelectionPanelElement
{
    protected override void Use()
    {
        _data[0] = NegativePrice;

        DropBoxSelectionHandler.RaiseEvent(DropBoxItemType.Ammo, _data);
    }
}
