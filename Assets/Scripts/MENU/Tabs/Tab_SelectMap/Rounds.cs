using UnityEngine;

public class Rounds : BaseSliderLevel<int>
{
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

    protected override void UpdateTitleText(int index)
    {
        _title.text = Title(Result(RoundsCount(index)));
    }

    public override void OnSliderValueChanged()
    {
        UpdateTitleText(SliderValue);
    }

    public override void SetDefault()
    {
        UpdateTitleText(SliderValue = 0);
    }

    public string TextResultOnline()
    {
        return Keys.GameRounds + GlobalFunctions.BlueColorText(RoundsCount(SliderValue).ToString()) + "\n";
    }

    public string TextResultOffline()
    {
        return "\n";
    }
}
