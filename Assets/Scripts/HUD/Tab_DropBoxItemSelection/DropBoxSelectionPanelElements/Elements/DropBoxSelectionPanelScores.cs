using UnityEngine;

public class DropBoxSelectionPanelScores : BaseDropBoxSelectionPanelElement
{
    protected override void Use()
    {
        _data[0] = Random.Range(0, 101);
        _data[1] = NegativePrice;

        DropBoxSelectionHandler.RaiseEvent(DropBoxItemType.XpUpgrade, _data);
    }
}
