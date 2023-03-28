using UnityEngine;


public class DropBoxSelectionPanelXpMultiplier : BaseDropBoxSelectionPanelElement
{
    [SerializeField] [Space]
    private int _multiplier;



    protected override void Use()
    {
        _data[0] = NegativePrice;
        _data[1] = _quantity;
        _data[2] = _multiplier;

        DropBoxSelectionHandler.RaiseEvent(_multiplier <= 2 ? DropBoxItemType.Xp2: DropBoxItemType.Xp3, _data);

        CanUse = false;
    }
}
