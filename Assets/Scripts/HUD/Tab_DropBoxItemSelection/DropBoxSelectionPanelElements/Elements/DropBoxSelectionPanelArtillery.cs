using UnityEngine;


public class DropBoxSelectionPanelArtillery : BaseDropBoxSelectionPanelElement
{
    [SerializeField] [Space]
    private float _shellsSpreadValue;


    protected override void Use()
    {
        _data[0] = _shellsSpreadValue;
        _data[1] = NegativePrice;
        _data[2] = _quantity;

        StartCoroutine(RaiseEventAfterDelay(DropBoxItemType.Artillery, _data));
    }
}
