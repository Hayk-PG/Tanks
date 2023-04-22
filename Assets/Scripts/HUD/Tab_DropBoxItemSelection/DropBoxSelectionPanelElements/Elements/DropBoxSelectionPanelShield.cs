
public class DropBoxSelectionPanelShield : BaseDropBoxSelectionPanelElement
{
    protected override void Use()
    {
        _data[0] = NegativePrice;

        DropBoxSelectionHandler.RaiseEvent(DropBoxItemType.Shield, _data);
    }
}
