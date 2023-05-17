
public class DropBoxSelectionPanelAccuracy : BaseDropBoxSelectionPanelElement
{
    protected override void Use()
    {
        _data[0] = NegativePrice;
        _data[1] = _turns;

        StartCoroutine(RaiseEventAfterDelay(DropBoxItemType.Accuracy, _data));
    }

    public override void DisplayAbility()
    {
        _textsDropBoxSelectionPanelElement.Ability.text = $"Disables external factors from affecting the trajectory of the tank's shell for {_turns} turn.";
    }
}
