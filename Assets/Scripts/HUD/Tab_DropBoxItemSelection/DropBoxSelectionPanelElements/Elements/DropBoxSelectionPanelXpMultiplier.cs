using UnityEngine;


public class DropBoxSelectionPanelXpMultiplier : BaseDropBoxSelectionPanelElement
{
    [SerializeField] [Space]
    private int _multiplier;



    protected override void Use()
    {
        DropBoxSelectionHandler.RaiseEvent(_multiplier <= 2 ? DropBoxItemType.Xp2: DropBoxItemType.Xp3, new object[] { _price, _quantity, _multiplier });

        CanUse = false;
    }
}
