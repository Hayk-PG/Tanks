using UnityEngine;


public class Toggle_Wind : BaseSliderLevel<GameWind>, IMatchmakeTextResult, IMatchmakeData
{
    protected override string Title(string suffix)
    {
        return "Wind " + "[" + suffix + "]";
    }

    protected override string Result(GameWind gameWind)
    {
        return gameWind == GameWind.On ?
               GlobalFunctions.GreenColorText("On") :
               GlobalFunctions.RedColorText("Off");
    }

    protected override void UpdateTitleText(GameWind gameWind)
    {
        _title.text = Title(Result(gameWind));
    }

    public override void OnSliderValueChanged()
    {
        Data.Manager.SetWind(SliderValue);
        UpdateTitleText(Data.Manager.GameWind);
    }

    public override void SetDefault()
    {
        SliderValue = PlayerPrefs.GetInt(Keys.MapWind, 0);
        Data.Manager.SetWind(SliderValue);
        UpdateTitleText(Data.Manager.GameWind);
    }

    public string TextResultOnline()
    {
        return Keys.MapWind + Result(Data.Manager.GameWind) + "\n";
    }

    public string TextResultOffline()
    {
        return TextResultOnline();
    }

    public void StoreData(MatchmakeData matchmakeData)
    {
        matchmakeData.IsWindOn = Data.Manager.GameWind == GameWind.On ? true : false;
    }
}
