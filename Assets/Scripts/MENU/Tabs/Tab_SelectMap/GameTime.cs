using UnityEngine;

public class GameTime : BaseSliderLevel<int>
{
    protected override void Activate()
    {
        _slider.value = PlayerPrefs.GetInt(Keys.GameTime, 0);
        Data.Manager.SetGameTime(Mathf.FloorToInt(_slider.value));
        UpdateTitleText(Data.Manager.GameTime);
    }

    public override void OnSliderValueChanged()
    {
        Data.Manager.SetGameTime(Mathf.FloorToInt(_slider.value));
        UpdateTitleText(Data.Manager.GameTime);
    }
   
    protected override string Title(string suffix)
    {
        return "Time " + "[" + suffix + "]";
    }

    protected override void UpdateTitleText(int t)
    {
        _title.text = Title("<color=#F5F24C>" + t + "</color>");
    }
}
