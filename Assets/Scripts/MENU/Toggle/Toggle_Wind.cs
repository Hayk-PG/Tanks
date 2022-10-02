using UnityEngine;


public class Toggle_Wind : BaseSliderLevel<GameWind>
{
    protected override string Title(string suffix)
    {
        return "Wind " + "[" + suffix + "]";
    }

    protected override void UpdateTitleText(GameWind gameWind)
    {
        switch (gameWind)
        {
            case GameWind.On: _title.text = Title("<color=#00F510>" + "On" + "</color>"); break;
            case GameWind.Off: _title.text = Title("<color=#F51B01>" + "Off" + "</color>"); break;
        }
    }

    protected override void OnTabOpened()
    {
        _slider.value = PlayerPrefs.GetInt(Keys.MapWind, 0);
        Data.Manager.SetWind(Mathf.FloorToInt(_slider.value));
        UpdateTitleText(Data.Manager.GameWind);
    }

    public override void OnSliderValueChanged()
    {
        Data.Manager.SetWind(Mathf.FloorToInt(_slider.value));
        UpdateTitleText(Data.Manager.GameWind);
    }
}
