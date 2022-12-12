using UnityEngine;

public class GameTime : BaseSliderLevel<int>
{
    protected override void UpdateTitleText(int t) => _title.text = Title(Result(t));

    protected override void ListenSliderValueChange()
    {
        Data.Manager.SaveRoundTime((RoundTimeStates)SliderValue);
        UpdateTitleText(Data.Manager.RoundTime);
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
        int roundTime = PlayerPrefs.GetInt(Keys.RoundTime, Data.Manager.ConvertRoundTimeStates(RoundTimeStates.s30));
        SliderValue = (int)Data.Manager.ConvertRoundTimeStates(roundTime);
        Data.Manager.SaveRoundTime(Data.Manager.ConvertRoundTimeStates(roundTime)); 
        UpdateTitleText(roundTime);
    }
}
