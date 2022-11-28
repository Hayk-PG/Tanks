using UnityEngine;

public class Toggle_Wind : BaseSliderLevel<GameWind>
{
    private void Start()
    {
        SetDefault();
    }

    protected override void ListenSliderValueChange()
    {
        Data.Manager.SaveWind((GameWind)SliderValue);
        UpdateTitleText((GameWind)SliderValue);
    }

    protected override void UpdateTitleText(GameWind gameWind)
    {
        _title.text = Title(Result(gameWind));
    }

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

    public override void SetDefault()
    {
        SliderValue = PlayerPrefs.GetInt(Keys.MapWind, 0);
        Data.Manager.SaveWind((GameWind)SliderValue);
        UpdateTitleText((GameWind)SliderValue);
    }
}
