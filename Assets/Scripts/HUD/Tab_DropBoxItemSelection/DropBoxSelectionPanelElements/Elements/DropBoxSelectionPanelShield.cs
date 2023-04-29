
public class DropBoxSelectionPanelShield : BaseDropBoxSelectionPanelElement
{
    protected override void Use()
    {
        _data[0] = NegativePrice;

        StartCoroutine(RaiseEventAfterDelay(DropBoxItemType.Shield, _data));
    }
}
