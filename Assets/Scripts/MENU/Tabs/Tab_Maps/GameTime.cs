using UnityEngine;

public class GameTime : BaseSliderLevel<int>
{
    private void Start()
    {
        SetDefault();
    }

    protected override void UpdateTitleText(int t)
    {
        _title.text = Title(Result(t));
    }

    protected override void ListenSliderValueChange()
    {
        Data.Manager.SaveRoundDuration((RoundDuration)SliderValue);
        UpdateTitleText(Data.Manager.RoundDuration);
    }

    protected override string Title(string suffix)
    {
        return "Time " + "[" + suffix + "]";
    }

    protected override string Result(int t)
    {
        return t.ToString();
    }

    public override void SetDefault()
    {
        SliderValue = PlayerPrefs.GetInt(Keys.GameTime, 0);
        Data.Manager.SaveRoundDuration((RoundDuration)SliderValue);
        UpdateTitleText(Data.Manager.RoundDuration);
    }
}
