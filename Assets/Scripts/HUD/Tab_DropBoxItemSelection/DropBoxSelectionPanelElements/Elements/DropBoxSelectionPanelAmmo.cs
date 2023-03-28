
public class DropBoxSelectionPanelAmmo : BaseDropBoxSelectionPanelElement
{
    protected override void Use() => DropBoxSelectionHandler.RaiseEvent(DropBoxItemType.Ammo, new object[] { NegativePrice });
}
