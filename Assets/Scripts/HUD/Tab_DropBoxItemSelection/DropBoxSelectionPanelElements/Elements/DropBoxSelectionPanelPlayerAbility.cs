using UnityEngine;

public class DropBoxSelectionPanelPlayerAbility : BaseDropBoxSelectionPanelElement
{
    [SerializeField] [Space]
    private int _abilityIndex;

    private IPlayerAbility _observer;




    protected override void Use()
    {
        _data[0] = _observer;
        _data[1] = _turns;
        _data[2] = NegativePrice;

        DropBoxSelectionHandler.RaiseEvent(DropBoxItemType.Ability, _data);
    }

    //For player's abilities
    public void Initialize(IPlayerAbility playerAbility, string title, string ability, int price, int quantity, int usageFrequency, int turns)
    {
        _observer = playerAbility;

        _title = title;
        _ability = ability;

        _price = price;
        _quantity = quantity;
        _usageFrequency = usageFrequency;
        _turns = turns;
    }
}
