using UnityEngine;


public class DropBoxSelectionPanelXpMultiplier : BaseDropBoxSelectionPanelElement
{
    [SerializeField] [Space]
    private int _multiplier, _rounds;

    protected override void Use()
    {
        DropBoxSelectionHandler.RaiseEvent(DropBoxSelectionHandler.DropBoxItemType.XpMultiplier, new object[] { _multiplier, _rounds });
    }
}
