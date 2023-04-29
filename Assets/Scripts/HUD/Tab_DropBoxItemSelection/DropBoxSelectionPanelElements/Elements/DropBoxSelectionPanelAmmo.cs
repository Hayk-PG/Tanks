
public class DropBoxSelectionPanelAmmo : BaseDropBoxSelectionPanelElement
{
    protected override void Use()
    {
        _data[0] = NegativePrice;

        StartCoroutine(RaiseEventAfterDelay(DropBoxItemType.Ammo, _data));
    }
}
