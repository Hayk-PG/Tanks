using UnityEngine;

public class DropBoxSelectionPanelHealth : BaseDropBoxSelectionPanelElement
{
    [SerializeField] [Space]
    private int _health;


    protected override void Use()
    {
        _data[0] = NegativePrice;
        _data[1] = _health;

        DropBoxSelectionHandler.RaiseEvent(DropBoxItemType.HpBoost, _data);
    }
}
