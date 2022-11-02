using UnityEngine;

public class GameDifficultyLevel : BaseSliderLevel<SingleGameDifficultyLevel>, IMatchmakeTextResult
{
    protected override string Title(string suffix)
    {
        return suffix;
    }

    protected override string Result(SingleGameDifficultyLevel singleGameDifficultyLevel)
    {
        return singleGameDifficultyLevel == SingleGameDifficultyLevel.Easy ?
               GlobalFunctions.BlueColorText("Easy") :
               singleGameDifficultyLevel == SingleGameDifficultyLevel.Normal ?
               GlobalFunctions.BlueColorText("Normal") :
               GlobalFunctions.BlueColorText("Hard");
    }

    protected override void UpdateTitleText(SingleGameDifficultyLevel singleGameDifficultyLevel)
    {
        _title.text = Title(Result(singleGameDifficultyLevel));
    }

    public override void OnSliderValueChanged()
    {
        Data.Manager.SetDifficultyLevel(SliderValue);
        UpdateTitleText(Data.Manager.SingleGameDifficultyLevel);
    }

    public override void SetDefault()
    {
        SliderValue = PlayerPrefs.GetInt(Keys.DifficultyLevel, 0);
        Data.Manager.SetDifficultyLevel(SliderValue);
        UpdateTitleText(Data.Manager.SingleGameDifficultyLevel);
    }

    public string TextResultOnline()
    {
        return "\n";
    }

    public string TextResultOffline()
    {
        return Keys.DifficultyLevel + Result(Data.Manager.SingleGameDifficultyLevel) + "\n";
    }
}
