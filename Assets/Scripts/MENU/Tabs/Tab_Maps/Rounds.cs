public class Rounds : BaseSliderLevel<int>
{
    protected override void ListenSliderValueChange()
    {
        UpdateTitleText(SliderValue);
    }

    protected override void UpdateTitleText(int index)
    {
        _title.text = Title(Result(RoundsCount(index)));
    }

    protected override string Title(string suffix)
    {
        return "Rounds " + "[" + suffix + "]";
    }

    protected override string Result(int rounds)
    {
        return GlobalFunctions.BlueColorText(rounds.ToString());
    }

    private int RoundsCount(int index)
    {
        return index == 0 ? 1 : index == 1 ? 3 : 5;
    }

    public override void SetDefault()
    {
        UpdateTitleText(SliderValue = 0);
    }
}
