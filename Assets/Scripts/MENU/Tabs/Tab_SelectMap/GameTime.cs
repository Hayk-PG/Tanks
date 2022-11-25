using UnityEngine;

public class GameTime : BaseSliderLevel<int>
{
    protected override string Title(string suffix)
    {
        return "Time " + "[" + suffix + "]";
    }

    protected override string Result(int t)
    {
        return GlobalFunctions.BlueColorText(t.ToString());
    }

    protected override void UpdateTitleText(int t)
    {
        _title.text = Title(Result(t));
    }
    
    public override void OnSliderValueChanged()
    {
        Data.Manager.SetGameTime(SliderValue);
        UpdateTitleText(Data.Manager.GameTime);
    }

    public override void SetDefault()
    {
        SliderValue = PlayerPrefs.GetInt(Keys.GameTime, 0);
        Data.Manager.SetGameTime(SliderValue);
        UpdateTitleText(Data.Manager.GameTime);
    }

    public string TextResultOnline()
    {
        return Keys.GameTime + GlobalFunctions.GreenColorText(Result(Data.Manager.GameTime)) + "\n";
    }

    public string TextResultOffline()
    {
        return TextResultOnline();
    }
}
