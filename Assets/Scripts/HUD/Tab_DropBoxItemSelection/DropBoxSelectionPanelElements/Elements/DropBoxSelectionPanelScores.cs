using UnityEngine;

public class DropBoxSelectionPanelScores : BaseDropBoxSelectionPanelElement
{
    private int _randomXp = Random.Range(0, 101);


    protected override void Use()
    {
        _data[0] = _randomXp;
        _data[1] = NegativePrice;

        DropBoxSelectionHandler.RaiseEvent(DropBoxItemType.XpUpgrade, _data);
    }
}
