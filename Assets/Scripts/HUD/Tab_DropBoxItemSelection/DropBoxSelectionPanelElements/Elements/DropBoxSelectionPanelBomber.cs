using UnityEngine;


public class DropBoxSelectionPanelBomber : BaseDropBoxSelectionPanelElement
{
    [SerializeField]
    protected BomberType _bomberType;


    protected override void Use()
    {
        _data[0] = _bomberType;
        _data[1] = NegativePrice;
        _data[2] = _quantity;

        DropBoxSelectionHandler.RaiseEvent(DropBoxItemType.Bomber, _data);

        CanUse = false;
    }
}
